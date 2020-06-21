using System;

namespace UrlComposition.Shared
{
    public class CorrelationPattern : PatternBase
    {
        public CorrelationPattern(string id) : base(id)
        {
        }

        public override bool Match(string id)
            => throw new NotImplementedException();
    }
}
