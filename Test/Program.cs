using DiscordBotLibrary;
using DiscordBotLibrary.ActivityResources;
using DiscordBotLibrary.Logging;
using DiscordBotLibrary.PresenceUpdateResources;
using Microsoft.Extensions.DependencyInjection;
using DiscordBotLibrary.ChannelResources.PartialChannel;
using DotNetEnv;
using DiscordBotLibrary.DiscordClientResources;
using DiscordBotLibrary.ChannelResources.ChannelEnums;
using DiscordBotLibrary.ChannelResources.StartThreadParamsResources;
using DiscordBotLibrary.ThreadMemberResources;
using DiscordBotLibrary.RestApiLimiterResources;

namespace Test
{
    internal class Program
    {
        private static IServiceProvider _serviceProvider = default!;
        private readonly Logger _logger = default!;

        static async Task Main()
        {
            Env.Load();
            DiscordClientConfig discordClientConfig = new()
            {
                Token = Environment.GetEnvironmentVariable("LIBRARY_TOKEN") ?? throw new ArgumentNullException("Token is null"),
                Intents = Intents.All,
            };

            DiscordClient client = new(discordClientConfig, new LoggerConfig(logLevel: LogLevel.Debug));

            client.OnPresenceUpdate += Client_OnPresenceUpdate;
            client.OnGuildCreate += Client_OnGuildCreate;
            client.OnReady += Client_OnReady;
            client.OnGuildsReceived += Client_OnGuildsReceived;

            Logger logger = await client.StartAsync();

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                logger.LogError(args.Exception, true);
                args.SetObserved();
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Exception ex = (Exception)args.ExceptionObject;
                logger.LogError(ex, true);
            };

            ServiceCollection services = new();
            services.AddSingleton(logger);
            services.AddSingleton(client);
            _serviceProvider = services.BuildServiceProvider();

            await Task.Delay(Timeout.Infinite);
        }

        private static async void Client_OnReady(DiscordClient client, ReadyEventArgs args)
        {
            Console.WriteLine("Ready!");
            await client.UpdatePresenceAsync(new SelfPresenceUpdate()
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

        private static async void Client_OnGuildsReceived(DiscordClient client, IReadOnlyDictionary<ulong, DiscordGuild> args)
        {
            const ulong crisDc = 1126185640745246731;
            const ulong familyDc = 1341844969085862021;
            const ulong cacxCordDc = 1381712720935518369;
            const ulong crisId = 912014865898549378;
            const ulong slaysId = 772060595951763476;

            DiscordGuild guild = client.GetGuild(familyDc)!;
            Channel? crisChannel = guild.GetChannelThatUserIsIn(crisId);
            VoiceChannelHandler voiceChannelHandler = client.GetVoiceChannelHandler();

            await voiceChannelHandler.ConnectToVcAsync(guild.Id, 1341856648842444892, selfDeaf: true, selfMute: false);
            await voiceChannelHandler.MuteAsync(guild.Id, true);
            await voiceChannelHandler.DisconnectFromVcAsync(guild.Id);
        }


        private static void Client_OnGuildCreate(DiscordClient discordClient, DiscordGuild args)
        {
            Console.WriteLine($"Guild received: {args.Name}");
        }

        private static void Client_OnPresenceUpdate(DiscordClient discordClient, Presence args)
        {
            
        }
    }
}
