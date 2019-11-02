using System;
using System.Collections.Concurrent;

namespace Web.Services
{
    public class GroupStore : IGroupStore
    {
        private ConcurrentDictionary<string, string> _groups;

        public GroupStore()
        {
            _groups =  new ConcurrentDictionary<string, string>();
        }

        public void AddGroupHost(string groupName, string hostId)
        {
            _groups.AddOrUpdate(groupName, hostId, (key, old) => hostId);
        }

        public string GetGroupHost(string groupName)
        {
            var groupExists = _groups.TryGetValue(groupName, out var groupHost);

            return groupExists ? groupHost : throw new InvalidOperationException($"Group {groupName} does not exist.");
        }

        public bool GroupExists(string groupName)
        {
            return _groups.ContainsKey(groupName);
        }
    }
}