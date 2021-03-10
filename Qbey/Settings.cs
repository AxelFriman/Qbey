using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qbey
{
    static class SettDriver
    {
        static public string PathToSett { get; set; } = Directory.GetCurrentDirectory() + "/config.json";
        static public Settings Sett { get; private set; }
        static public DiscordSocketClient client { get; set; }
        
        static public void loadSett() => Sett = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettDriver.PathToSett));
        static public void saveSett() => File.WriteAllText(PathToSett, Newtonsoft.Json.JsonConvert.SerializeObject(Sett));
    }

    internal class Settings
    {
        public string client { get; set; }
        public string anounceChannel { get; set; }
        public List<Follows> follows { get; set; }
        public string youTubeAPIURL { get; set; }
        public string youTubeAPIToken { get; set; }
        public string discordToken { get; set; }

        public class Follows
        {
            public int followId { get; set; }
            public string linkToVideosPage { get; set; }
            public string channelName { get; set; }
        }
    }
}
