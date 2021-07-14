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
using Qbey.SettingsControllers;
using Qbey.Models;

namespace Qbey
{
    static class InnerActions 
    {
        static public async Task sendAlert(ulong guild, string textToSend, EmbedBuilder eb = null)
        {
            var channel = MainConfig.Instance.DiscordClient.GetChannel(MainConfig.Instance.GuildsSettings[guild].Sett.anounceChannel) as ITextChannel;
            var embed = eb?.Build();
            await channel.SendMessageAsync(text: textToSend, embed: embed);
        }

        //static public SocketGuildChannel findChannelByName(string channelName)
        //{
        //    return SettDriver.client.GetGuild(SettDriver.Sett.client).Channels.First(x => x.Name == channelName);
        //} TODO delete this

        static public async Task<List<RestGuildChannel>> createChannelsAsync(ulong guild, string channelName)
        {
            var guildCfg = MainConfig.Instance.GuildsSettings[guild];
            var discordClient = MainConfig.Instance.DiscordClient;
            List<RestGuildChannel> res = new List<RestGuildChannel>();
            if (guildCfg.Sett.categoryToCreateTxtChannels != 0)
            {
                var newTextChannel = await discordClient.GetGuild(guild).CreateTextChannelAsync(channelName,
                    props => props.CategoryId = guildCfg.Sett.categoryToCreateTxtChannels);
                res.Add(newTextChannel);
            }
            if (guildCfg.Sett.categoryToCreateVoiceChannels != 0)
            {
                var newVoiceChannel = await discordClient.GetGuild(guild).CreateVoiceChannelAsync(channelName,
                    props => props.CategoryId = guildCfg.Sett.categoryToCreateVoiceChannels);
                res.Add(newVoiceChannel);
            }
            return res;
        }

        static public async Task deleteChannelAsync(ulong channelId)
        {
            if (channelId > 0)
            {
                var channel = MainConfig.Instance.DiscordClient.GetChannel(channelId) as ITextChannel;
                await channel?.DeleteAsync();
            }
        }

        static public async void CheckYoutubeFollowsAsync(ulong guild)
        {            
            var guildCfg = MainConfig.Instance.GuildsSettings[guild];
            var guildFollows = MainConfig.Instance.GuildsFollows[guild];
            var ytProcessor = new YoutubeProcessor(guild);
            var notNullFollows = guildFollows.Sett.follows.Where<Follows>(x => x.followId > 0); //костыль для нулевого элемента, потом придумаю как сделать нормально TODO
            foreach (var channel in notNullFollows)
            {
                string lastVideoId = await ytProcessor.getLastVideoFromWeb(channel.linkToVideosPage);
                bool isOnline;
                if (String.IsNullOrEmpty(lastVideoId))
                {
                    isOnline = false;
                }
                else
                {
                    isOnline = await ytProcessor.isStream(lastVideoId);
                }
                bool wasOnline = channel.lastStatusOnline;
                if (isOnline != wasOnline)
                {
                    channel.lastStatusOnline = isOnline;
                    if (isOnline)
                    {
                        var createdChannels = await createChannelsAsync(guild, channel.channelName);
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
                        guildFollows.saveSett();
                        if (!String.IsNullOrEmpty(txtToEmbed))
                        {
                            txtToEmbed = "Смотреть вместе: " + txtToEmbed;
                            eb.WithDescription(txtToEmbed);
                        }
                        string textForAlert = $"{channel.channelName}  начала стрим.\nhttps://www.youtube.com/watch?v={lastVideoId}";
                        if (channel.serverRoleId != 0 && guildCfg.Sett.pingRolesOnAlert)
                        {
                            textForAlert = $"<@&{channel.serverRoleId}> " + textForAlert;
                        }
                        await sendAlert(guild, textForAlert, eb);
                    }
                    else
                    {
                        await deleteChannelAsync(channel.textChannel);
                        channel.textChannel = 0;
                        await deleteChannelAsync(channel.voiceChannel);
                        channel.voiceChannel = 0;
                        await sendAlert(guild, $"{channel.channelName} закончила стрим.");
                    }
                }
            }
        }
    }
}
