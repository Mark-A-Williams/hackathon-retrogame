namespace Web.Services
{
    public interface IGroupStore
    {
        void AddGroupHost(string groupName, string hostId);
        string GetGroupHost(string groupName);

        bool GroupExists(string groupName);
    }
}