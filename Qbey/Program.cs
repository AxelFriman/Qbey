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
        //internal Dictionary<ulong, List<ISettings>> guidsSettings;
        //internal GlobalConfigSettings config = new GlobalConfigSettings("config.json"); 
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            try
            {
                SettDriver.loadSett();
            }
            catch (IOException)
            {
                ErrorEvent?.Invoke(new LogMessage(LogSeverity.Warning, "Program.MainAsync", "Empty config file was created."));
            }
            catch (ArgumentOutOfRangeException e)
            {
                ErrorEvent?.Invoke(new LogMessage(LogSeverity.Warning, "Program.MainAsync", e.Message));
            }
            catch (NullReferenceException e)
            {
                ErrorEvent?.Invoke(new LogMessage(LogSeverity.Critical, "Program.MainAsync", e.Message));
                throw;
            }
            catch (ArgumentException e)
            {
                ErrorEvent?.Invoke(new LogMessage(LogSeverity.Critical, "Program.MainAsync", e.Message));
                throw;
            }

            SettDriver.client = _client;

            string token = "";

            try
            {
                token = SettDriver.Sett.discordToken;
            }
            catch (NullReferenceException)
            {
                ErrorEvent?.Invoke(new LogMessage(LogSeverity.Critical, "Program.MainAsync", "Discord Token is empty."));
                throw;
            }

            try
            {
                HistoryDriver.addFollowsToHistory();
            }
            catch (NullReferenceException)
            {
                ErrorEvent?.Invoke(new LogMessage(LogSeverity.Warning, "Program.MainAsync", "Unable to add follows to history."));
            }
            catch (IOException)
            {
                ErrorEvent?.Invoke(new LogMessage(LogSeverity.Warning, "Program.MainAsync", "Empty history file was created."));
            }

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

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

        public static event Func<LogMessage, Task> ErrorEvent;
    }
}
