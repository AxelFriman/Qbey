using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Qbey
{
    public class ClientCalls : ModuleBase<SocketCommandContext>
    {
        [Command("test")]
        public async Task test()
        {
            var eb = new EmbedBuilder();
            eb.WithDescription("Гугл[¹](https://google.com)");
            await ReplyAsync(message: "Ты " + Context.User.Username, embed: eb.Build());
        }

        [Command("getDiscordInfo")]
        [Summary("Shows Discord client ID and channel ID.")]
        public async Task getDiscordChannelId()
        {
            var eb = new EmbedBuilder();
            eb.WithDescription(Context.Channel.Id.ToString());
            await ReplyAsync(message: $"ID of this text channel:", embed: eb.Build());
            eb.WithDescription(Context.Guild.Id.ToString());
            await ReplyAsync(message: $"ID of this server:", embed: eb.Build());
        }

    }
}
