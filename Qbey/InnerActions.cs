using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Timers;

namespace Qbey
{
    static class InnerActions 
    {
        static public async Task sendAlert(string textToSend) 
        {
            var channel = SettDriver.client.GetChannel(SettDriver.Sett.anounceChannel) as ITextChannel;
            await channel.SendMessageAsync(textToSend);
        }

        static public SocketGuildChannel findChannelByName(string channelName)
        {
            return SettDriver.client.GetGuild(SettDriver.Sett.client).Channels.First(x => x.Name == channelName);
        }

        static public async Task createChannelsAsync(string channelName)
        {
            if (SettDriver.Sett.categoryToCreateTxtChannels != 0)
            {
                await SettDriver.client.GetGuild(SettDriver.Sett.client).CreateTextChannelAsync(channelName,
                    props => props.CategoryId = SettDriver.Sett.categoryToCreateTxtChannels); 
            }
            if (SettDriver.Sett.categoryToCreateVoiceChannels != 0)
            {
                await SettDriver.client.GetGuild(SettDriver.Sett.client).CreateVoiceChannelAsync(channelName,
                    props => props.CategoryId = SettDriver.Sett.categoryToCreateVoiceChannels);
            }
        }

        static public async void CheckYoutubeFollowsAsync(Object source, EventArgs e)
        {
            foreach (var channel in SettDriver.Sett.follows)
            {
                string lastVideoId = await YoutubeProcessor.getLastVideoFromWeb(channel.linkToVideosPage);
                bool isOnline = await YoutubeProcessor.isStream(lastVideoId);
                bool wasOnline = HistoryDriver.getStatusById(channel.followId);
                if (isOnline != wasOnline)
                {
                    HistoryDriver.setStatusById(channel.followId, isOnline);
                    if (isOnline)
                    {
                        await sendAlert("Начался стрим https://www.youtube.com/watch?v=" + lastVideoId);
                        await createChannelsAsync(channel.channelName);
                    }
                    else
                    {
                        //TODO удаление каналов
                        await sendAlert($"Стрим {channel.channelName} окончен.");
                    }
                }
            }
        }
    }
}
