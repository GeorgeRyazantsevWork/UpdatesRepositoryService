using ServiceCloud.UpdatesRepositoryService.Models;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    public class RepositoryIndex : IRepositoryIndex
    {
        private Dictionary<string, UpdateModel> Updates;

        private Dictionary<string, UpdateModel> StructedUpdates;

        public RepositoryIndex()
        {
            Updates = new();
            StructedUpdates = new();
        }

        public int Count => throw new NotImplementedException();

        public bool Contains(ConfigurationModel targetConfig)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UpdateModel> GetPotentialUpdatesFor(ConfigurationModel? config)
        {
            throw new NotImplementedException();
        }

        public void Replace(IReadOnlyCollection<UpdateModel> updates)
        {
            throw new NotImplementedException();
        }
    }
}
