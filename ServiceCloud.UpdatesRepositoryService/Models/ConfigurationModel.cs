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

        #region Перегрузка операторов "==" и "!="

        /// <summary>
        /// Оператор сравнения двух объектов класса <see cref="ConfigurationModel"/>  на равенство
        /// </summary>
        /// <param name="left">Сравниваемый объект</param>
        /// <param name="right">Объект, с которым сравниваем</param>
        /// <returns>TRUE - объекты идентичны, FALSE - в противном случае</returns>
        public static bool operator ==(ConfigurationModel? left, ConfigurationModel? right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(left, null))
            {
                return false;
            }
            return left.Equals(right);
        }

        /// <summary>
        /// Оператор сравнения двух объектов класса <see cref="ConfigurationModel"/> на неравенство
        /// </summary>
        /// <param name="left">Сравниваемый объект</param>
        /// <param name="right">Объект, с которым сравниваем</param>
        /// <returns>TRUE - объекты отличаются друг от друга, FALSE - объекты идентичны</returns>
        public static bool operator !=(ConfigurationModel? left, ConfigurationModel? right)
        {
            return !(left == right);
        }

        #endregion

        #region Члены IEquatable<ConfigurationModel>

        /// <summary>
        /// Сравнить текущий объект с заданным
        /// </summary>
        /// <param name="other">Объект, с которым происходит сравнение</param>
        /// <returns>TRUE - объекты равны, FALSE - в противном случае</returns>
        public bool Equals(ConfigurationModel? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return Name == other.Name
                && Publisher == other.Publisher
                && Version == other.Version
                && Edition == other.Edition;
        }

        /// <summary>
        /// Сравнить текущий объект с заданным
        /// </summary>
        /// <param name="other">Объект, с которым происходит сравнение</param>
        /// <returns>TRUE - объекты равны, FALSE - в противном случае</returns>
        public override bool Equals(object? other)
        {
            return Equals(other as ConfigurationModel);
        }

        /// <summary>
        /// Рассчитать хэш-код объекта
        /// </summary>
        /// <returns>Хэш-код объекта</returns>
        public override int GetHashCode()
        {
            int hash = 0;

            if (Name != null)
                hash ^= Name.GetHashCode();

            if (Publisher != null)
                hash ^= Publisher.GetHashCode();

            if (Version != null)
                hash ^= Version.GetHashCode();

            hash ^= Edition.GetHashCode();

            return hash;
        }

        #endregion

        #region Члены IComparable<ConfigurationModel>

        /// <summary>
        /// Провести сравнение текущего объекта с заданным
        /// </summary>
        /// <param name="other">Объект, с которым производим сравнение</param>
        /// <returns> 0 - объекты равны, меньше 0 - текущий объект меньше заданного, больше 0 - текущий объект больше заданного</returns>
        public int CompareTo(ConfigurationModel? other)
        {
            if (other == null)
            {
                return 1;
            }

            int result = 0;
            if (Publisher != null && (result = Publisher.CompareTo(other.Publisher)) != 0)
            {
                return result;
            }
            if (Name != null && (result = Name.CompareTo(other.Name)) != 0)
            {
                return result;
            }
            if (Version != null)
            {
                return Version.CompareTo(other.Version);
            }

            return -1;
        }

        #endregion
    }
}
