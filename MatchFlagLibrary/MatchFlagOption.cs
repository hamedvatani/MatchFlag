using System;
using System.Collections.Generic;

namespace MatchFlagLibrary
{
    public class MatchFlagOption
    {
        private int flagCount;

        public string RedisServer { get; set; }
        public int RedisPort { get; set; }

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

        public bool IsValid()
        {
            foreach (var flagOption in FlagOptions)
                if (flagOption.FlagIndex >= FlagCount || flagOption.ExpectedFlagIndex >= FlagCount)
                    return false;
            return true;
        }

        public static MatchFlagOption CreateDefault()
        {
            return new MatchFlagOption
            {
                RedisServer = "127.0.0.1",
                RedisPort = 6379,
                flagCount = 1,
                FlagOptions = new List<FlagOption>()
            };
        }

        public void AddFlagOption(int flagIndex,
            int after,
            int expectedFlagIndex,
            Action<string, int, int, int> ifNotApear,
            Action<string, int, int, int> ifApear)
        {
            if (FlagOptions == null)
                FlagOptions = new List<FlagOption>();
            FlagOptions.Add(new FlagOption
            {
                FlagIndex = flagIndex,
                After = after,
                ExpectedFlagIndex = expectedFlagIndex,
                IfNotApear = ifNotApear,
                IfApear = ifApear
            });
        }

        public void AddFlagOption(int flagIndex,
            int after,
            int expectedFlagIndex,
            Action<string, int, int, int> ifNotApear)
        {
            AddFlagOption(flagIndex, after, expectedFlagIndex, ifNotApear, null);
        }
    }
}