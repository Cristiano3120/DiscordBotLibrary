using DiscordBotLibrary;
using DiscordBotLibrary.ActivityResources;
using DiscordBotLibrary.ExternalExtraClasses;
using DiscordBotLibrary.PresenceUpdateResources;
using Microsoft.Extensions.DependencyInjection;

namespace Test
{
    internal class Program
    {
        private static IServiceProvider _serviceProvider = default!;
        private Logger _logger = default!;

        static async Task Main()
        {
            StreamReader streamReader = new(@"C:\Users\Crist\Desktop\LibraryTestToken.txt");
            DiscordClientConfig discordClientConfig = new()
            {
                LogLevel = LogLevel.Debug,
                Token = streamReader.ReadToEnd(),
                Intents = Intents.All,
            };

            DiscordClient client = new(discordClientConfig);

            client.OnPresenceUpdate += Client_OnPresenceUpdate;
            client.OnGuildCreate += Client_OnGuildCreate;
            client.OnReady += Client_OnReady;

            Logger logger = client.Start();
            logger.AddEnumToLog<PresenceStatus>();
            logger.AddEnumToLog<Language>();
            
            ServiceCollection services = new();
            services.AddSingleton(logger);
            services.AddSingleton(client);
            _serviceProvider = services.BuildServiceProvider();

            await Task.Delay(Timeout.Infinite);
        }

        private static async void Client_OnReady(DiscordClient client, ReadyEventArgs args)
        {
            Console.WriteLine("Ready!");
            await client.UpdatePresence(new SelfPresenceUpdate()
            {
                Status = PresenceStatus.Online,
                Activities =
                [
                    new Activity()
                    {
                        Name = "Visual Studio 2022",
                        Type = ActivityType.Playing,
                        State = "Coding...",
                    },
                ],
            });
        }

        private static void Client_OnGuildCreate(DiscordClient discordClient, DiscordGuild args)
        {
            Console.WriteLine($"Guild received: {args.Name}");
        }

        private static void Client_OnPresenceUpdate(DiscordClient discordClient, PresenceUpdate args)
        {

        }
    }
}
