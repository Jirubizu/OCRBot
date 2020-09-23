using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using OCRBot.Handlers;
using OCRBot.Services;

namespace OCRBot
{
    public class OCRBot
    {
        private readonly DiscordShardedClient client;
        private readonly CommandService commandService;
        private ConfigService config;
        public OCRBot(DiscordShardedClient shardedClient = null, CommandService command = null)
        {
            client = shardedClient ?? new DiscordShardedClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                MessageCacheSize = 50,
                LogLevel = LogSeverity.Verbose
            });

            commandService = command ?? new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async
            });
        }
        
        public async Task SetupAsync()
        {
            config = new ConfigService("./config.json");
            
            await client.LoginAsync(TokenType.Bot, config.Config.Token);
            await client.StartAsync();
            
            client.Log += LogAsync;

            var services = SetupServices();
            
            var commandHandler = services.GetRequiredService<CommandHandler>();
            await commandHandler.SetupAsync();

            await Task.Delay(-1);
        }
        
        private IServiceProvider SetupServices() => new ServiceCollection()
            .AddSingleton(this)
            .AddSingleton(client)
            .AddSingleton(commandService)
            .AddSingleton(config)
            .AddSingleton<CommandHandler>()
            .AddSingleton<HttpService>()
            .AddSingleton<DatabaseService>()
            .AddSingleton<PaginationService>()
            .AddSingleton<OCRService>()
            .AddSingleton<TranslateService>()
            .AddSingleton<HelpService>()
            .BuildServiceProvider();
        
        private Task LogAsync(LogMessage message)
        {
            Console.WriteLine(message.Message);
            return Task.CompletedTask;
        }
    }
}