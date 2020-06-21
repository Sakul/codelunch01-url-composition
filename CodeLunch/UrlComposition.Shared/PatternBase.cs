using System;
using System.Linq;

namespace UrlComposition.Shared
{
    public abstract class PatternBase
    {
        protected IdComposition IdComposition;

        public PatternBase(string id)
        {
            IdComposition = Parse(id);
        }

        public IdComposition Parse(string id)
        {
            return new IdComposition
            {
                StateCode = id.FirstOrDefault(),
                Work = string.Join(string.Empty, id.Take(4)),
            };
        }

        public abstract bool Match(string id);
    }
}
