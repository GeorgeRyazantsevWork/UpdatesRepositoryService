using ServiceCloud.UpdatesRepositoryService.Models;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    public class UpdatesRepository : IUpdatesRepository
    {
        public IEnumerable<UpdateModel> Updates => throw new NotImplementedException();

        public int UpdatesCount => throw new NotImplementedException();

        public bool ContainsUpdate(ConfigurationModel? config)
        {
            throw new NotImplementedException();
        }

        public string GetLocation(string? updateId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UpdateModel> GetPotentialUpdates(ConfigurationModel? config)
        {
            throw new NotImplementedException();
        }

        public UpdateModel GetUpdate(ConfigurationModel? config, bool transitive)
        {
            throw new NotImplementedException();
        }
    }
}
