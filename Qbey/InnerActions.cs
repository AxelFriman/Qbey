using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace Qbey
{
    static class InnerActions 
    {
        static public async Task sendAlert(string textToSend) 
        {
            //await SettDriver.client.GetGuild(ulong.Parse(SettDriver.Sett.client)).GetTextChannel(ulong.Parse(SettDriver.Sett.anounceChannel))
            //    .SendMessageAsync(textToSend);
            var channel = SettDriver.client.GetChannel(ulong.Parse(SettDriver.Sett.anounceChannel)) as ITextChannel;
            //if channel null
            await channel.SendMessageAsync(textToSend);
        }

        static public SocketGuildChannel findChannelByName(string channelName)
        {
            return SettDriver.client.GetGuild(ulong.Parse(SettDriver.Sett.client)).Channels.First(x => x.Name == channelName);
        }

        static public async Task createChannelAsync(string channelName)
        {
            var textChannelProps = new TextChannelProperties();
            var test = SettDriver.client.GetChannel(1) as ITextChannel; //можно сделать getcategorychannels, прописать айди категорий в настройки, не забыть внести изменения в getDiscordInfo, 
            await SettDriver.client.GetGuild(ulong.Parse(SettDriver.Sett.client)).CreateTextChannelAsync(channelName);
            await SettDriver.client.GetGuild(ulong.Parse(SettDriver.Sett.client)).CreateVoiceChannelAsync(channelName);
        }
    }
}
