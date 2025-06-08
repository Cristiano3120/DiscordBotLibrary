namespace DiscordBotLibrary.Json
{
    internal static class JsonElementExtensions
    {
        /// <summary>
        /// Gets the data["d"] property from the JsonElement and deserializes it to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonElement"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T Deserialize<T>(this JsonElement jsonElement)
            => jsonElement.GetProperty("d").Deserialize<T>(DiscordClient.JsonSerializerOptions) 
                ?? throw new Exception("Failed to deserialize[JsonElementExtensions(T Deserialize<T>(this JsonElement jsonElement))]");
    }
}
