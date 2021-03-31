using Qbey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qbey.SettingsControllers
{
    class GlobalConfigSettings : BaseSettings<GlobalConfigModel>
    {
        public override GlobalConfigModel Sett
        {
            get => base.Sett;
            protected set
            {
                if (string.IsNullOrWhiteSpace(value.discordToken))
                {
                    string errTxt = "Discord token must not be empty.";
                    throw new ArgumentException(errTxt);
                }
                base.Sett = value;
            }
        }
        public GlobalConfigSettings(string jsonPath) : base(jsonPath) { }
        public GlobalConfigSettings(string jsonPath, GlobalConfigModel sett) : base(jsonPath, sett) { }
    }
}
