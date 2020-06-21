using System;
using System.Linq;

namespace UrlComposition.Shared
{
    public abstract class PatternBase
    {
        protected UrlComposition UrlComposition;

        public PatternBase(string url)
            => UrlComposition = Parse(url);

        public UrlComposition Parse(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return new UrlComposition();
            }

            var splittedInput = url.Split('.', '-', '~');
            var stateCode = getStateCode();
            var workAndOperation = stateCode.HasValue ? splittedInput.FirstOrDefault().Substring(1) : splittedInput.FirstOrDefault();
            const int WorkSegmentCharacterAmount = 3;
            const int FixedFirstSegmentCharacterAmount = 7;
            var result = new UrlComposition
            {
                StateCode = stateCode,
                Work = workAndOperation.Take(WorkSegmentCharacterAmount).Any() ?
                    string.Join(string.Empty, workAndOperation.Take(WorkSegmentCharacterAmount)) : null,
                Operation = workAndOperation.Length > WorkSegmentCharacterAmount ?
                    workAndOperation.Substring(WorkSegmentCharacterAmount) : null,
                Step = url.Contains(".") ? splittedInput[1] : null,
                Id = getId(),
                Correlation = url.Contains("~") ? splittedInput.LastOrDefault() : null,
                IsValid = isStateCodeValid(url) && url.Contains("-") && splittedInput.FirstOrDefault()?.Length == FixedFirstSegmentCharacterAmount,
            };
            return result;

            char? getStateCode()
            {
                if (false == isStateCodeValid(url, true))
                {
                    return null;
                }
                return splittedInput.Any() ? new char?(splittedInput.FirstOrDefault().ToLower().FirstOrDefault()) : null;
            }
            string getId()
            {
                if (false == url.Contains("-"))
                {
                    return null;
                }
                return url.Contains(".") ? splittedInput[2] : splittedInput[1];
            }
            bool isStateCodeValid(string input, bool ignoreCaseSensitive = false)
            {
                var comparison = ignoreCaseSensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
                return input.StartsWith("n", comparison)
                    || input.StartsWith("m", comparison);
            }
        }

        public abstract bool Match(string id);
    }
}
