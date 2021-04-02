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

namespace Qbey
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        internal Dictionary<ulong, List<ISettings>> guildsSettings;
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            GlobalConfigSettings.GetInstance("config.json");
            GlobalConfigSettings.client = _client;

            await _client.LoginAsync(TokenType.Bot, globalConfig.Sett.discordToken);
            await _client.StartAsync();

            var commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            var commHlr = new CommandHandler(_client, commandService);
            await commHlr.InstallCommandsAsync(); 

            var logs = new LoggingService(_client, commandService);

            System.Timers.Timer youtubeTimer = YoutubeCheckTimer.Init();
            
            await Task.Delay(-1);
        }

        public static event Func<LogMessage, Task> ErrorEvent;
    }
}
