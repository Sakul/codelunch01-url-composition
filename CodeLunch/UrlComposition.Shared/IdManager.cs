namespace UrlComposition.Shared
{
    public class IdManager : IIdManager
    {
        public bool IsMatched(string id, string target)
        {
            return id == target;
        }
    }
}
