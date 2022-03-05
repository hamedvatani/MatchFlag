using System;
using System.Collections.Generic;

namespace MatchFlagLibrary
{
    public class MatchFlagsOption
    {
        private int flagCount;

        public string RedisServer { get; set; }
        public int RedisPort { get; set; }
        public int DefaultKeyExpiration { get; set; }

        public int FlagCount
        {
            get => flagCount;
            set
            {
                if (flagCount <= 0)
                    flagCount = value;
            }
        }

        public List<FlagOption> FlagOptions { get; set; }

        public static MatchFlagsOption CreateDefault()
        {
            return new MatchFlagsOption
            {
                RedisServer = "127.0.0.1",
                RedisPort = 6379,
                flagCount = 1,
                DefaultKeyExpiration = 18000000,
                FlagOptions = new List<FlagOption>()
            };
        }

        public void AddFlagOption(int flagIndex,
            int after,
            int expectedFlagIndex,
            Action ifNotApear)
        {
            if (FlagOptions == null)
                FlagOptions = new List<FlagOption>();
            FlagOptions.Add(new FlagOption
            {
                FlagIndex = flagIndex,
                After = after,
                ExpectedFlagIndex = expectedFlagIndex,
                IfNotApear = ifNotApear
            });
        }
    }
}