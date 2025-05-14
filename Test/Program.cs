using DiscordBotLibrary;
using DiscordBotLibrary.ExternalExtraClasses;

namespace Test
{
    internal class Program
    {
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
            client.OnGuildCreate += Client_OnGuildCreate;
            client.OnReady += Client_OnReady;
            await client.StartAsync();
            
            await Task.Delay(Timeout.Infinite);
        }

        private static void Client_OnReady(DiscordClient discordClient, ReadyEventArgs args)
        {
            Console.WriteLine("Ready!");
        }

        private static void Client_OnGuildCreate(DiscordClient discordClient, DiscordGuild args)
        {
            Console.WriteLine($"Guild received: {args.Name}");
        }
    }
}
