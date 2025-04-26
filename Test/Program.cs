using DiscordBotLibrary;

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
            };

            DiscordClient client = new(discordClientConfig);
            await client.Start();

            await Task.Delay(Timeout.Infinite);
        }
    }
}
