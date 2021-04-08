using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Qbey
{
    static class YoutubeCheckTimer
    {
        static public System.Timers.Timer Init()
        {
            var youtubeTimer = new System.Timers.Timer();
            youtubeTimer.Elapsed += InnerActions.CheckYoutubeFollowsAsync;
            youtubeTimer.Interval = 600000;
            youtubeTimer.Enabled = false;
            return youtubeTimer;            
        }

    }
}
