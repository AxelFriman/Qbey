﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace Qbey
{
    class InnerActions : ModuleBase<SocketCommandContext>
    {
        public async Task sendAlert(string textToSend) 
        {
            Console.WriteLine("");
            await SettDriver.client.GetGuild(ulong.Parse(SettDriver.Sett.client)).GetTextChannel(ulong.Parse(SettDriver.Sett.anounceChannel))
                .SendMessageAsync(textToSend);
        }
    }
}