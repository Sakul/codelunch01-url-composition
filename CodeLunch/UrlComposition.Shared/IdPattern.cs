namespace UrlComposition.Shared
{
    public class IdPattern : PatternBase
    {
        public IdPattern(string id) : base(id)
        {
        }

        public override bool Match(string id)
        {
            var other = Parse(id);
            var self = UrlComposition;
            var isIdMatch = self.Id == other.Id;
            var isWorkMatch = $"{self.StateCode}{self.Work}" == $"{other.StateCode}{other.Work}";
            var isStepMatch = string.IsNullOrWhiteSpace(self.Step) || self.Step == other.Step;
            return isIdMatch && isWorkMatch && isStepMatch;
        }
    }
}
