namespace ServiceCloud.UpdatesRepository.BusinessLogic.Models
{
    /// <summary>
    /// Возможные типы (редакции) конфигураций информационных баз 1С
    /// </summary>
    /// <remarks> На данный момент редакции имеются только у конфигураций, поставляемых фирмой 1С.
    /// Редакция зависит от типа лицензии приобретенной пользователем и указывается в наименовании конфигурации.</remarks>
    public enum InfoBaseConfigEdition
    {
        /// <summary>
        /// Редакция информационной базы не определена
        /// </summary>
        Unknown,

        /// <summary>
        /// Базовая редакция
        /// </summary>
        BASE,

        /// <summary>
        /// Редакция ПРОФ
        /// </summary>
        PROF,

        /// <summary>
        /// Редакция КОРП
        /// </summary>
        CORP
    }
}
