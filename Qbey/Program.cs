using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.IO;
using Qbey.SettingsControllers;
using System.Timers;

namespace Qbey
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            DiscordSocketClient _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            var cfg = MainConfig.Instance;
            cfg.DiscordClient = _client;
            cfg.GlobalAppConfig = new GlobalConfigSettings("config.json");
            var globalCfg = cfg.GlobalAppConfig.Sett;

            _client.Ready += async () => { LoadSettigs(_client, cfg); };

            await _client.LoginAsync(TokenType.Bot, globalCfg.discordToken);
            await _client.StartAsync();

            var commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            var commHlr = new CommandHandler(_client, commandService);
            await commHlr.InstallCommandsAsync(); 

            var logs = new LoggingService(_client, commandService);

            //TODO move this away from here
            Dictionary<ulong, Timer> timers = new Dictionary<ulong, Timer>();
            foreach (var setts in cfg.GuildsSettings)
            {
                timers.Add(setts.Key, YoutubeCheckTimer.Create(setts.Key));
            }
            await Task.Delay(-1);
        }

        private void LoadSettigs(DiscordSocketClient client, MainConfig config)
        {
            foreach (var guild in client.Guilds)
            {
                try
                {
                    var newConfig = new GuildConfigSettings(guild.Id + "/config.json");
                    config.GuildsSettings.Add(guild.Id, newConfig);
                }
                catch (NullReferenceException)
                {
                    ErrorEvent?.Invoke(new LogMessage(LogSeverity.Error, "LoadSettings", $"Unable to load server settings for {guild.Name} ({guild.Id})"));
                };
                try
                {
                    var newFollows = new FollowsSettings(guild.Id + "/follows.json");
                    config.GuildsFollows.Add(guild.Id, newFollows);
                }
                catch (NullReferenceException)
                {
                    ErrorEvent?.Invoke(new LogMessage(LogSeverity.Error, "LoadSettings", $"Unable to load follows for {guild.Name} ({guild.Id})"));
                }
            }
        }

        public static event Func<LogMessage, Task> ErrorEvent;
    }
}
