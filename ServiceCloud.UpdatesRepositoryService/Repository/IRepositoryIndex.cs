using ServiceCloud.UpdatesRepositoryService.Models;

namespace ServiceCloud.UpdatesRepositoryService.Repository
{
    public interface IRepositoryIndex
    {
        IReadOnlyCollection<UpdateModel> Updates { get; }

        void Replace(IReadOnlyCollection<UpdateModel> updates);
    }
}
