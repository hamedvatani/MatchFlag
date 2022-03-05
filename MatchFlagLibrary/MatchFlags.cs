using System;
using System.Linq;
using System.Threading.Tasks;
using KeyExpirationReminderLibrary;
using StackExchange.Redis;

namespace MatchFlagLibrary
{
    public class MatchFlags:IDisposable
    {
        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase db;
        private readonly MatchFlagsOption options;
        private readonly KeyExpirationReminder reminder;

        public MatchFlags(Action<MatchFlagsOption> optionBuilder = null)
        {
            options = MatchFlagsOption.CreateDefault();
            if (optionBuilder != null)
                optionBuilder(options);
            redis = ConnectionMultiplexer.Connect($"{options.RedisServer}:{options.RedisPort}");
            db = redis.GetDatabase();
            reminder = new KeyExpirationReminder();
        }

        public void Dispose()
        {
            redis?.Dispose();
        }

        private void SetRedisFlag(string key, int flagIndex, int keyTimeout)
        {
            var keyExists = db.KeyExists(key);

            var val = (byte[])db.StringGet(key) ?? new byte[options.FlagCount];
            val[flagIndex] = 1;
            if (!db.StringSet(key, val))
                throw new Exception("Can't write to redis");

            if (!keyExists)
            {
                if (keyTimeout == -1)
                    keyTimeout = options.DefaultKeyExpiration;
                if (!db.KeyExpire(key, DateTime.Now.AddMilliseconds(keyTimeout)))
                    throw new Exception("Can't write to redis");
            }
        }

        private bool GetRedisFlag(string key, int flagIndex)
        {
            var val = (byte[])db.StringGet(key) ?? new byte[options.FlagCount];
            return val[flagIndex] == 1;
        }

        public void SetFlag(string key, int flagIndex, int keyTimeout = -1)
        {
            SetRedisFlag(key, flagIndex, keyTimeout);

            int delKeyTime = 0;
            foreach (var flagOption in options.FlagOptions)
            {
                if (flagOption.FlagIndex == flagIndex)
                {
                    delKeyTime = Math.Max(delKeyTime, flagOption.After);
                    reminder.TryAddKey(Guid.NewGuid().ToString(), flagOption.After, (k, t) =>
                    {
                        if (!GetRedisFlag(key, flagOption.ExpectedFlagIndex))
                            flagOption.IfNotApear();
                    });
                }
            }

            delKeyTime += 1000;
            reminder.TryAddKey(Guid.NewGuid().ToString(), delKeyTime, (k, t) =>
            {
                if (db.KeyExists(key))
                    db.KeyDelete(key);
            });
        }
    }
}