using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qbey.Models
{
    class GuildConfigModel
    {
        public ulong client { get; set; }
        public ulong anounceChannel { get; set; }
        public bool pingRolesOnAlert { get; set; }
        public bool enableAutoCheck { get; set; }
        public int streamCheckIntervalSec { get; set; }
        public ulong categoryToCreateTxtChannels { get; set; }
        public ulong categoryToCreateVoiceChannels { get; set; }
        //public string youTubeAPIURL { get; set; }
        public string youTubeAPIToken { get; set; }
        //public string discordToken { get; set; }
    }
}
