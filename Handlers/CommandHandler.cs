using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using OCRBot.Services;

namespace OCRBot.Handlers
{
    public class CommandHandler
    {
        private readonly DiscordShardedClient client;
        private readonly CommandService commandService;
        private readonly IServiceProvider services;
        private readonly DatabaseService database;

        public CommandHandler(DiscordShardedClient c, CommandService cs, IServiceProvider s, DatabaseService d)
        {
            client = c;
            commandService = cs;
            services = s;
            database = d;
        }

        public async Task SetupAsync()
        {
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            commandService.Log += LogAsync;
            client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage msg)
        {
            var argPos = 0;
            if (!(msg is SocketUserMessage message))
            {
                return;
            }

            var guildChannel = (SocketGuildChannel) message.Channel;

            var r = await database.LoadRecordsByGuildId(guildChannel.Guild.Id);

            if (message.HasStringPrefix(r.Prefix, ref argPos))
            {
                var context = new ShardedCommandContext(client, message);

                var result = await commandService.ExecuteAsync(context, argPos, services);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }

        private Task LogAsync(LogMessage message)
        {
            Console.WriteLine(message.Message);
            return Task.CompletedTask;
        }
    }
}