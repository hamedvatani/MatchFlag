using System;

namespace MatchFlagLibrary
{
    public class FlagOption
    {
        public int FlagIndex { get; set; }
        public int After { get; set; }
        public int ExpectedFlagIndex { get; set; }
        public Action<string, int, int, int> IfNotApear { get; set; }
        public Action<string, int, int, int> IfApear { get; set; }
    }
}