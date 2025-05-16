using DiscordBotLibrary.Sharding;
using System.Collections.Concurrent;

namespace DiscordBotLibrary
{
    internal sealed class WsGatewayLimiter
    {
        private readonly ConcurrentQueue<string> _messageQueue;
        private byte _sentMessageCount;
        private Shard _shard;

        public WsGatewayLimiter(Shard shard)
        {
            _shard = shard;
            _messageQueue = new ConcurrentQueue<string>();
            _ = ProcessQueueAsync();
        }

        private async Task ProcessQueueAsync()
        {
            using PeriodicTimer timer = new(TimeSpan.FromMinutes(1));
            {
                while (await timer.WaitForNextTickAsync())
                {
                    _sentMessageCount = 0;

                    while (_messageQueue.TryDequeue(out string? payload))
                    {
                        if (_sentMessageCount >= 120)
                        {
                            _messageQueue.Enqueue(payload);
                            break;
                        }

                        _sentMessageCount++;
                        await _shard.SendPayloadWssAsync(payload);
                        await Task.Delay(200);
                    }
                }
            }        
        }

        public bool CheckIfRateLimitExceeded(string payload)
        {
            if (_sentMessageCount >= 120)
            {
                _messageQueue.Enqueue(payload);
                return true;
            }

            _sentMessageCount++;
            return false;
        }
    }
}
