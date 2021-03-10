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
        [Summary("Says Hello, user!.")]
        public async Task test()
        {
            var eb = new EmbedBuilder();
            eb.WithDescription("Вот тут можно[¹](https://google.com)");
            await ReplyAsync(message: "Тут нельзя делать ссылки текстом. " + Context.User.Username, embed: eb.Build());
        }

        [Command("getIdByName")]
        public async Task getIdByName(string name)
        {
            List<Snippet> res = await YoutubeProcessor.searchChannelByName(name);
            foreach (var item in res)
            {
                var eb = new EmbedBuilder();
                eb.WithTitle($"id: {item.channelId}");
                await ReplyAsync(message: $"Title: {item.title}\nLink: {new Uri("https://www.youtube.com/channel/" + item.channelId)}\nID:", embed: eb.Build());
            }
        }

        [Command("getDiscordInfo")]
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
