namespace UrlComposition.Shared
{
    public interface IIdManager
    {
        bool IsMatched(string id, string target);
    }
}
