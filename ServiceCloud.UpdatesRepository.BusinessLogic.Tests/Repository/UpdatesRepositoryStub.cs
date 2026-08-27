namespace ServiceCloud.UpdatesRepository.BusinessLogic.Repository.Tests
{
    /// <summary>
    /// Вспомогательный класс для тестирования класса UpdatesRepository
    /// </summary>
    class UpdatesRepositoryStub : UpdatesRepository
    {
        /// <summary>
        /// Конструктор, позволяющий создать репозиторий с заранее известными обновлениями
        /// </summary>
        public UpdatesRepositoryStub(IRepositoryIndex index, UpdatesRepositoryTestContext context)
            : base(index, UpdatesRepositoryTestContext.TestRepositoryPath)
        {
            foreach (var update in context.ExistingUpdates)
            {
                index.Add(update);
            }

            for (int i = 0; i < context.ExistingUpdates.Count; i++)
            {
                var id = context.ExistingUpdates[i].Id;
                if (id != null)
                {
                    LocationMap?[id] = context.ExistingLocations[i];
                }
            }
        }
    }
}
