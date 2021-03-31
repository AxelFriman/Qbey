using Qbey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qbey.SettingsControllers
{
    class FollowsSettings : BaseSettings<FollowsModel>
    {
        public FollowsSettings(string jsonPath) : base(jsonPath) { }
        public FollowsSettings(string jsonPath, FollowsModel sett) : base(jsonPath, sett){ }
    }
}
