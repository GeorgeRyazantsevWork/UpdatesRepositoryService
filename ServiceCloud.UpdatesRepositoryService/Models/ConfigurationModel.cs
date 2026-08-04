using System.Xml.Serialization;

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

        /// <summary>
        /// Наименование издателя конфигурации
        /// </summary>
        public string? Publisher { get; set; }

        /// <summary>
        /// Версия конфигурации
        /// </summary>
        [XmlIgnore]
        public InfoBaseConfigVersion? Version
        {
            get;
            set;
        }

        /// <summary>
        /// Строковое представление версии конфигурации
        /// </summary>
        [XmlElement("Version")]
        public string? VersionString
        {
            get => Version?.ToString();
            set => Version = value != null ? new InfoBaseConfigVersion(value) : new InfoBaseConfigVersion();
        }

        /// <summary>
        /// Редакция конфигурации
        /// </summary>
        public InfoBaseConfigEdition Edition { get; set; }
        // TODO Дописать операторы сравнения
    }
}
