using DiscordBotLibrary.Sharding;
using System.Collections.Concurrent;

namespace DiscordBotLibrary
{
    internal sealed class WsGatewayLimiter
    {
        private const int MaxMessages = 120;
        private static readonly TimeSpan Window = TimeSpan.FromSeconds(60);

        private readonly ConcurrentQueue<DateTime> _timestamps = new();
        private readonly ConcurrentQueue<string> _messageQueue = new();
        private readonly Shard _shard;

        private readonly SemaphoreSlim _sendLock = new(1, 1);
        private readonly Timer _queueProcessor;

        public WsGatewayLimiter(Shard shard)
        {
            _shard = shard;
            _queueProcessor = new Timer(ProcessQueue, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));
        }

        public async Task<bool> TrySendAsync(string payload)
        {
            await _sendLock.WaitAsync();
            try
            {
                CleanupOldTimestamps();

                if (_timestamps.Count >= MaxMessages)
                {
                    _messageQueue.Enqueue(payload);
                    return false;
                }

                await _shard.SendPayloadWssAsync(payload);
                _timestamps.Enqueue(DateTime.UtcNow);

                return true;
            }
            finally
            {
                _sendLock.Release();
            }
        }

        private async void ProcessQueue(object? state)
        {
            if (_messageQueue.IsEmpty)
                return;

            await _sendLock.WaitAsync();
            try
            {
                CleanupOldTimestamps();

                while (_timestamps.Count < MaxMessages && _messageQueue.TryDequeue(out var payload))
                {
                    await _shard.SendPayloadWssAsync(payload);
                    _timestamps.Enqueue(DateTime.UtcNow);
                }
            }
            finally
            {
                _sendLock.Release();
            }
        }

        private void CleanupOldTimestamps()
        {
            DateTime threshold = DateTime.UtcNow - Window;

            while (_timestamps.TryPeek(out DateTime time) && time < threshold)
                _timestamps.TryDequeue(out _);
        }
    }

}
