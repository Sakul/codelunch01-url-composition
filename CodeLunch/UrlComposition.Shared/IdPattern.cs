using System;

namespace UrlComposition.Shared
{
    public class IdPattern : PatternBase
    {
        public IdPattern(string id) : base(id)
        {
        }

        public override bool Match(string id)
            => throw new NotImplementedException();
    }
}
