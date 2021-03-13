using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace Qbey
{
    public class ClientCalls : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task test()
        {
            var eb = new EmbedBuilder();
            eb.WithDescription("Гугл[¹](https://google.com)");
            string txt = "";
            foreach (var item in SettDriver.client.GetGuild(361294551899570176).CategoryChannels)
            {
                txt += item.Name + "\n";
            }
            await ReplyAsync(message: txt, embed: eb.Build());
                    }

        [Command("getDiscordInfo")]
        [Summary("Shows Discord client ID and channel ID.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task getDiscordChannelId()
        {
            var eb = new EmbedBuilder();

            eb.WithDescription(Context.Channel.Id.ToString());
            await ReplyAsync(message: $"ID of this text channel ({Context.Channel.Name}):", embed: eb.Build());

            eb.WithDescription(Context.Guild.Id.ToString());
            await ReplyAsync(message: $"ID of this server: ({Context.Guild.Name})", embed: eb.Build());

            //a channel doesn't know about it's category, have to find it out 
            var categ = Context.Guild.CategoryChannels.Where<SocketCategoryChannel>(categ =>
            {
                return categ.Channels.Where<SocketGuildChannel>(ch => //searching for a categ
                {
                    return ch.Equals(Context.Channel); //which contains given channel
                }) is not null;
            }).First<SocketCategoryChannel>();
            eb.WithDescription(categ.Id.ToString());
            await ReplyAsync(message: $"ID of this category ({categ.Name}):", embed: eb.Build());
        }

    }
}
