using System;
using StackExchange.Redis;

namespace MatchFlagLibrary
{
    public class MatchFlag:IDisposable
    {
        private ConnectionMultiplexer redis;
        private IDatabase db;
        private MatchFlagOption options;

        public MatchFlag(Action<MatchFlagOption> optionBuilder)
        {
            options = MatchFlagOption.CreateDefault();
            optionBuilder(options);
            redis = ConnectionMultiplexer.Connect($"{options.RedisServer}:{options.RedisPort}");
            db = redis.GetDatabase();
        }

        public void Dispose()
        {
            redis?.Dispose();
        }

        public void SetFlag(string key, int flagIndex, int keyTimeout,int after,)
        {
            byte[] val = db.StringGet(key);
            if (val == null)
                val = new byte[options.FlagCount];
            val[flagIndex] = 1;
            db.StringSet(key, val);
        }
    }
}