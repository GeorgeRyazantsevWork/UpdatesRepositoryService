using ServiceCloud.UpdatesRepositoryService.Models;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    public class RepositoryIndex : IRepositoryIndex
    {
        public IReadOnlyCollection<UpdateModel> Updates => throw new NotImplementedException();

        public void Replace(IReadOnlyCollection<UpdateModel> updates)
        {
            throw new NotImplementedException();
        }
    }
}
