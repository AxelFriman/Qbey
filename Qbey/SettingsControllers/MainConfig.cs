using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qbey.SettingsControllers
{
    class MainConfig
    {
        private MainConfig() { }
        public static MainConfig Instance { get; } = new MainConfig();
        public DiscordSocketClient DiscordClient { get; set; }
        public GlobalConfigSettings GlobalAppConfig { get; set; }
        public Dictionary<ulong, GuildConfigSettings> GuildsSettings { get; set; } = new Dictionary<ulong, GuildConfigSettings>();
        public Dictionary<ulong, FollowsSettings> GuildsFollows { get; set; } = new Dictionary<ulong, FollowsSettings>();
    }
}
