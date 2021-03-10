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
            youtubeTimer.Elapsed += CheckYoutubeFollowsAsync; 
            youtubeTimer.Interval = 600000;
            youtubeTimer.Enabled = false;
            InnerActions notifier = new InnerActions();
            return youtubeTimer;
        }

        static private async void CheckYoutubeFollowsAsync(Object source, ElapsedEventArgs e)
        {
            InnerActions notifier = new InnerActions();
            foreach (var channel in SettDriver.Sett.follows)
            {
                string lastVideoId = await YoutubeProcessor.getLastVideoFromWeb(channel.linkToVideosPage);
                bool isOnline = await YoutubeProcessor.isStream(lastVideoId);
                bool wasOnline = HistoryDriver.getStatusById(channel.followId);
                if (isOnline != wasOnline)
                {
                    HistoryDriver.setStatusById(channel.followId, isOnline);
                    string textToSend = string.Empty;
                    if (isOnline)
                    {
                        textToSend = "Начался стрим https://www.youtube.com/watch?v=" + lastVideoId;
                    }
                    else
                    {
                        textToSend = $"Стрим {channel.channelName} закончился.";
                    }
                    await notifier.sendAlert(textToSend);
                }
            }
        }
    }
}
