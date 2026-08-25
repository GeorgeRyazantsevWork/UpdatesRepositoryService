namespace ServiceCloud.UpdatesRepository.BuisnessLogic.Properties
{
    /// <summary>
    /// Настройки репозитория обновлений, соответствующие секции конфигурации <c>Repository</c>
    /// </summary>
    public sealed class RepositoryOptions
    {
        /// <summary>
        /// Название сервиса
        /// </summary>
        public const string ServiceName = "Updates Repository Service";

        /// <summary>
        /// Описание сервиса
        /// </summary>
        public const string ServiceDescription = "Сервис репозитория обновлений конфигураций информационных баз 1С";

        /// <summary>
        /// Имя секции конфигурации, из которой заполняются свойства класса
        /// </summary>
        public const string SectionName = "Repository";
        /// <summary>
        /// Интервал обновления индекса репозитория, используемый при отсутствии значения в конфигурации
        /// </summary>
        public static readonly TimeSpan DefaultRefreshInterval = TimeSpan.FromMinutes(30);
        /// <summary>
        /// Полный путь к каталогу с файлами обновлений
        /// </summary>
        public string Path { get; set; } = string.Empty;
        /// <summary>
        /// Интервал между запусками сканирования репозитория
        /// </summary>
        public TimeSpan RefreshInterval { get; set; } = DefaultRefreshInterval;       
    }
}
