namespace UrlComposition.Shared
{
    public class IdManager : IIdManager
    {
        public bool IsMatched(string id, string target)
            => GetPattern(target).Match(id);

        private PatternBase GetPattern(string target)
        {
            if (target.Contains("~"))
            {
                return new CorrelationPattern(target);
            }
            else
            {
                return new IdPattern(target);
            }
        }
    }
}
