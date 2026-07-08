namespace ServiceCloud.UpdatesRepositoryService.Models
{
    public class ConfigurationModel 
    {
        private static ConfigurationModel empty = new ConfigurationModel() { Name = "Конфигурация" };

        /// <summary>
        /// "Пустая" конфигурация информационной базы 1С (конфигурация не установлена)
        /// </summary>
        public static ConfigurationModel Empty => empty;

        /// <summary>
        /// Наименование конфигурации
        /// </summary>
        public string? Name { get; set; }
    }
}
