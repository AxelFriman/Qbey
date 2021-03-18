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
        static public string PathToSett { get; set; } = "config.json";
        static public Settings Sett { get; private set; }
        static public DiscordSocketClient client { get; set; }

        static public void loadSett()
        {
            try
            {
                Sett = Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettDriver.PathToSett));
            }
            catch (IOException) //no file or no access
            {
                Sett = new Settings() { follows = new List<Settings.Follows>() { new Settings.Follows() } };
                saveSett();
                throw;
            }
            finally
            {
                if (Sett is null)
                {
                    throw (new NullReferenceException("Invalid config.json format."));
                }
                if (Sett.streamCheckIntervalSec < 600000)
                {
                    Sett.streamCheckIntervalSec = 600000;
                    throw (new ArgumentOutOfRangeException("Stream Check Interval was too low and has been set to 10 min due to the quota limitation."));
                }
            }
        }

        static public void saveSett() => File.WriteAllText(PathToSett, Newtonsoft.Json.JsonConvert.SerializeObject(Sett));
    }

    internal class Settings
    {
        public ulong client { get; set; } 
        public ulong anounceChannel { get; set; }
        //public bool pingRolesOnAlert { get; set; }
        public int streamCheckIntervalSec { get; set; }
        public ulong categoryToCreateTxtChannels { get; set; }
        public ulong categoryToCreateVoiceChannels { get; set; }
        public List<Follows> follows { get; set; } //TODO split follows and settings
        public string youTubeAPIURL { get; set; }
        public string youTubeAPIToken { get; set; }
        public string discordToken { get; set; }

        public class Follows
        {
            public int followId { get; set; }
            public string linkToVideosPage { get; set; }
            public string channelName { get; set; }
            public ulong voiceChannel { get; set; }
            public ulong textChannel { get; set; }
            public string serverRole { get; set; } //TODO maybe shoud've made role id
        }
    }
}
