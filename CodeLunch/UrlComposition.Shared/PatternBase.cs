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
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var validStateCode = id.StartsWith("n") || id.StartsWith("m");
            var hasId = id.Contains("-");
            var splitted = id.Split('.', '-', '~');
            var validCodeFormath = splitted.FirstOrDefault()?.Length == 7;
            var isInputValid = validStateCode && hasId && validCodeFormath;
            if (false == isInputValid)
            {
                return null;
            }

            var hasCorrelation = id.Contains("~");
            var hasStep = id.Contains(".");
            var workAndOperation = splitted.FirstOrDefault();
            return new IdComposition
            {
                StateCode = workAndOperation.FirstOrDefault(),
                Work = string.Join(string.Empty, workAndOperation.Take(4)),
                Operation = workAndOperation.Length > 4 ? workAndOperation.Substring(4) : null,
                Step = hasStep ? splitted[1] : null,
                Id = hasStep ? splitted[2] : splitted[1],
                Correlation = hasCorrelation ? splitted.LastOrDefault() : null,
            };
        }

        public abstract bool Match(string id);
    }
}
