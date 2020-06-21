namespace UrlComposition.Shared
{
    public class IdManager : IIdManager
    {
        public bool IsMatched(string id, string target)
        {
            if (target.StartsWith("~"))
            {
                return id.EndsWith(target);
            }
            else
            {
                return id == target;
            }
        }
    }
}
