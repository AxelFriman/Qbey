using Qbey.SettingsControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Qbey
{
    static public class YoutubeCheckTimer
    {
        static public Timer Create(ulong guild)
        {
            var youtubeTimer = new Timer();
            youtubeTimer.Elapsed += (sender, e) => InnerActions.CheckYoutubeFollowsAsync(guild);
            youtubeTimer.Interval = MainConfig.Instance.GuildsSettings[guild].Sett.streamCheckIntervalSec * 1000;
            youtubeTimer.Enabled = MainConfig.Instance.GuildsSettings[guild].Sett.enableAutoCheck; 
            return youtubeTimer;
        }
    }
}
