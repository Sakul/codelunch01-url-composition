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
            if (string.IsNullOrWhiteSpace(id))
            {
                return new IdComposition();
            }

            var splitted = id.Split('.', '-', '~');
            var hasStep = id.Contains(".");

            var stateCode = new char?(splitted.FirstOrDefault().ToLower().FirstOrDefault());
            stateCode = isStateCodeValid(id, true) ? stateCode : null;
            var workAndOperation = stateCode.HasValue ? splitted.FirstOrDefault().Substring(1) : splitted.FirstOrDefault();

            var work = workAndOperation.Take(3).Any() ? string.Join(string.Empty, workAndOperation.Take(3)) : null;

            string step = null;
            if (hasStep)
            {
                if (splitted.Length > 1)
                {
                    step = splitted[1];
                }
                else
                {
                    step = splitted[0];
                }
            }

            string theId = null;
            if (id.Contains("-"))
            {
                if (hasStep)
                {
                    if (splitted.Length > 3)
                    {
                        theId = splitted[2];
                    }
                    else if (splitted.Length > 2)
                    {
                        theId = splitted[2];
                    }
                    else if (splitted.Length > 1)
                    {
                        theId = splitted[1];
                    }
                    else
                    {
                        theId = splitted[0];
                    }
                }
                else
                {
                    if (splitted.Length > 1)
                    {
                        theId = splitted[1];
                    }
                    else
                    {
                        theId = splitted[0];
                    }
                }
            }

            var result = new IdComposition
            {
                StateCode = stateCode,
                Work = work,
                Operation = workAndOperation.Length > 3 ? workAndOperation.Substring(3) : null,
                Step = step,
                Id = theId,
                Correlation = id.Contains("~") ? splitted.LastOrDefault() : null,
            };
            result.IsValid = isStateCodeValid(id) && id.Contains("-") && splitted.FirstOrDefault()?.Length == 7;
            return result;
        }
        private bool isStateCodeValid(string input, bool ignoreCaseSensitive = false)
        {
            var comparison = ignoreCaseSensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
            return input.StartsWith("n", comparison)
                || input.StartsWith("m", comparison);
        }

        public abstract bool Match(string id);
    }
}
