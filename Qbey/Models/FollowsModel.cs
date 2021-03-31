using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qbey.Models
{
    class FollowsModel
    {
        public List<Follows> follows { get; set; } = new List<Follows>() { new Follows() };
        public class Follows
        {
            public int followId { get; set; }
            public string linkToVideosPage { get; set; }
            public bool lastStatusOnline { get; set; }
            public string channelName { get; set; }
            public ulong voiceChannel { get; set; }
            public ulong textChannel { get; set; }
            public ulong serverRoleId { get; set; }
        }
    }
}
