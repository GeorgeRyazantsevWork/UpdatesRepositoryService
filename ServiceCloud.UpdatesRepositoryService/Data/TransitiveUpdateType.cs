namespace ServiceCloud.UpdatesRepositoryService.Data
{
    /// <summary>
    /// Тип переходного обновления конфигурации информационной базы 1С
    /// </summary>
    public enum TransitiveUpdateType
    {
        /// <summary>
        /// Значение не определено
        /// </summary>
        Unknown,

        /// <summary>
        /// Переходное обновление с базовой редакции на профессиональную
        /// </summary>
        BaseTopProf,

        /// <summary>
        /// Переходное обновление с профессиональной редакции на корпоративную
        /// </summary>
        ProfToCorp
    }
}
