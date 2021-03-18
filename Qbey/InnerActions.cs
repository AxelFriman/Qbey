using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Timers;
using Discord.Rest;

namespace Qbey
{
    static class InnerActions 
    {
        static public async Task sendAlert(string textToSend, EmbedBuilder eb = null) 
        {
            var channel = SettDriver.client.GetChannel(SettDriver.Sett.anounceChannel) as ITextChannel;
            var embed = eb?.Build();
            await channel.SendMessageAsync(text: textToSend, embed: embed);
        }

        static public SocketGuildChannel findChannelByName(string channelName)
        {
            return SettDriver.client.GetGuild(SettDriver.Sett.client).Channels.First(x => x.Name == channelName);
        }

        static public async Task<List<RestGuildChannel>> createChannelsAsync(string channelName)
        {
            List<RestGuildChannel> res = new List<RestGuildChannel>(); 
            if (SettDriver.Sett.categoryToCreateTxtChannels != 0)
            {
                var newTextChannel = await SettDriver.client.GetGuild(SettDriver.Sett.client).CreateTextChannelAsync(channelName,
                    props => props.CategoryId = SettDriver.Sett.categoryToCreateTxtChannels);
                res.Add(newTextChannel);
            }
            if (SettDriver.Sett.categoryToCreateVoiceChannels != 0)
            {
                var newVoiceChannel = await SettDriver.client.GetGuild(SettDriver.Sett.client).CreateVoiceChannelAsync(channelName,
                    props => props.CategoryId = SettDriver.Sett.categoryToCreateVoiceChannels);
                res.Add(newVoiceChannel);
            }
            return res;
        }

        static public async Task deleteChannelAsync(ulong channelId)
        {
            if (channelId > 0)
            {
                await SettDriver.client.GetGuild(SettDriver.Sett.client).GetChannel(channelId)?.DeleteAsync();
            }
        }

        static public async void CheckYoutubeFollowsAsync(Object source, EventArgs e)
        {
            foreach (var channel in SettDriver.Sett.follows)
            {
                string lastVideoId = await YoutubeProcessor.getLastVideoFromWeb(channel.linkToVideosPage);
                bool isOnline;
                if (String.IsNullOrEmpty(lastVideoId))
                {
                    isOnline = false;
                }
                else
                {
                    isOnline = await YoutubeProcessor.isStream(lastVideoId);
                }
                bool wasOnline = HistoryDriver.getStatusById(channel.followId);
                if (isOnline != wasOnline)
                {
                    HistoryDriver.setStatusById(channel.followId, isOnline);
                    if (isOnline)
                    {
                        var createdChannels = await createChannelsAsync(channel.channelName);
                        string txtToEmbed = String.Empty;
                        var eb = new EmbedBuilder();
                        foreach (var ch in createdChannels)
                        {
                            if (ch is RestTextChannel)
                            {
                                txtToEmbed += $"\n<#{ch.Id}>";
                                channel.textChannel = ch.Id;
                            }
                            if (ch is RestVoiceChannel)
                            {
                                var invite = await (ch as IVoiceChannel).CreateInviteAsync();
                                txtToEmbed += $"\n[{invite.ChannelName} (Войс)]({invite.Url})";
                                channel.voiceChannel = ch.Id;
                            }
                        }
                        SettDriver.saveSett();
                        if (!String.IsNullOrEmpty(txtToEmbed))
                        {
                            txtToEmbed = "Смотреть вместе: " + txtToEmbed;
                            eb.WithDescription(txtToEmbed);
                        }
                        string textForAlert = $"Начался стрим {channel.channelName}\nhttps://www.youtube.com/watch?v={lastVideoId}";
                        if (!String.IsNullOrEmpty(channel.serverRole)/* && SettDriver.Sett.pingRolesOnAlert*/)
                        {
                            textForAlert = $"<@{channel.serverRole}> " + textForAlert;
                        }
                        await sendAlert(textForAlert, eb);
                    }
                    else
                    {
                        await deleteChannelAsync(channel.textChannel);
                        channel.textChannel = 0;
                        await deleteChannelAsync(channel.voiceChannel);
                        channel.voiceChannel = 0;
                        await sendAlert($"Стрим {channel.channelName} окончен.");
                    }
                }
            }
        }
    }
}
