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

namespace Qbey
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });            

            SettDriver.loadSett();
            SettDriver.client = _client;
            var token = SettDriver.Sett.discordToken;

            HistoryDriver.makeNewHistoryRecords();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            //var commSer = new CommandService();
            //await commSer.AddModulesAsync(assembly: Assembly.GetExecutingAssembly(),
            //                        services: null);
            var commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            var commHlr = new CommandHandler(_client, commandService);
            await commHlr.InstallCommandsAsync();

            //logs
            var logs = new LoggingService(_client, commandService);

            //timers 
            System.Timers.Timer youtubeTimer = YoutubeCheckTimer.Init();
            
            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
    }
}
