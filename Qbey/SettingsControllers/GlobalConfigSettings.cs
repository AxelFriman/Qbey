using Discord.WebSocket;
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
        private static GlobalConfigSettings _instance;
        private GlobalConfigSettings(string jsonPath) : base(jsonPath) { }
        private GlobalConfigSettings(string jsonPath, GlobalConfigModel sett) : base(jsonPath, sett) { }
        public static GlobalConfigSettings GetInstance(string jsonPath, GlobalConfigModel sett)
        {
            if (_instance is null)
            {
                _instance = new GlobalConfigSettings(jsonPath, sett);
            }

            return _instance;
        }
        public static GlobalConfigSettings GetInstance(string jsonPath)
        {
            if (_instance is null)
            {
                _instance = new GlobalConfigSettings(jsonPath);
            }

            return _instance;
        }
        public static GlobalConfigSettings GetInstance()
        {
            if (_instance is null)
            {
                _instance = new GlobalConfigSettings("asd");
            }

            return _instance;
        }
    }
}
