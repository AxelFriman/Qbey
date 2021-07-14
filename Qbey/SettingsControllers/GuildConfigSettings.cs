using Qbey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qbey.SettingsControllers
{
    class GuildConfigSettings : BaseSettings<GuildConfigModel>
    {
        public override GuildConfigModel Sett
        {
            get => base.Sett;
            protected set
            {
                if (value.streamCheckIntervalSec < 600)
                {
                    value.streamCheckIntervalSec = 600;
                    OnError("streamCheckIntervalSec must not be less than 10 min due to the quota limit.");
                }
                base.Sett = value;
            }
        }
        public GuildConfigSettings(string jsonPath) : base(jsonPath) {}
        public GuildConfigSettings(string jsonPath, GuildConfigModel sett) : base(jsonPath, sett) {}
    }
}
