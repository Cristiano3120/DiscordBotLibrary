namespace DiscordBotLibrary
{
    internal static class Helper
    {
        public static string GetDynamicPath(string relativePath)
        {
            string projectBasePath = AppContext.BaseDirectory;
            int binIndex = projectBasePath.IndexOf($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal);

            if (binIndex == -1)
            {
                throw new Exception("Could not determine project base path!");
            }

            projectBasePath = projectBasePath[..binIndex];
            return Path.Combine(projectBasePath, relativePath);
        }
    }
}
