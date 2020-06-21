namespace UrlComposition.Shared
{
    public class IdPattern : PatternBase
    {
        public IdPattern(string id) : base(id)
        {
        }

        public override bool Match(string id)
            => UrlComposition.Id == Parse(id).Id;
    }
}
