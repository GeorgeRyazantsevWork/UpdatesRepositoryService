using ServiceCloud.UpdatesRepository.BusinessLogic.Helpers;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Models
{
    /// <summary>
    /// Данные об обновлении конфигурации информационной базы 1С
    /// </summary>
    [Serializable]
    public class InfoBaseUpdate : IEquatable<InfoBaseUpdate>
    {
        private InfobaseConfig destinationConfig = InfobaseConfig.Empty;
        private IEnumerable<InfobaseConfig> sourseConfigs = new InfobaseConfig[0];

        /// <summary>
        /// Конструктор объектов класса, используемый для отложенной инициализации свойств объекта
        /// </summary>
        public InfoBaseUpdate() { }

        /// <summary>
        /// Конструктор объектов класса
        /// </summary>
        /// <param name="destination">Конфигурация информационной базы, содержащаяся в обновлении</param>
        /// <param name="sources">Коллекция конфигураций, к которым применимо обновление</param>
        /// <param name="isTransitive">Признак, определяющий является ли обновление "переходным"</param>
        /// <exception cref="ArgumentNullException">Не задан один из обязательных параметров метода</exception>
        public InfoBaseUpdate(InfobaseConfig? destination, IEnumerable<InfobaseConfig>? sources, bool isTransitive = false)
        {
            DestinationConfig = destination;
            SourseConfigs = sources;
            IsTransitive = isTransitive;
        }

        /// <summary>
        /// Идентификатор обновления в репозитории
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Конфигурация базы 1С, которая содержится в обновлении
        /// </summary>
        /// <remarks>Конфигурация, которая будет у базы 1С после применения к ней данного обновления</remarks>
        /// <exception cref="System.ArgumentNullException">Не задана конфигурация</exception>
        public InfobaseConfig? DestinationConfig
        {
            get => destinationConfig;
            set => destinationConfig = value ?? throw new ArgumentNullException(nameof(DestinationConfig));
        }

        /// <summary>
        /// Список конфигураций информационных баз 1С, для которых предназначено это обновление
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Не задана коллекция конфигураций</exception>
        public IEnumerable<InfobaseConfig>? SourseConfigs
        {
            get => sourseConfigs;
            set => sourseConfigs = value ?? throw new ArgumentNullException(nameof(SourseConfigs));
        }

        /// <summary>
        /// Признак, определяющий является ли обновление "переходом" между редакциями конфигурации
        /// </summary>
        /// <remarks>Переходы между редакциями актуальны, только для конфигураций поставляемых фирмой 1С</remarks>
        public bool IsTransitive
        {
            get;
            set;
        }

        /// <summary>
        /// Проверяет возможность применения данного обновления к  заданной конфигурации информационной базы 1С
        /// </summary>
        /// <param name="config">Проверяемая конфигурация базы 1С</param>
        /// <returns>TRUE - обновление можно применить для указанной конфигурации базы, FALSE - в противном случае</returns>
        public bool IsApplicableTo(InfobaseConfig? config)
        {
            return sourseConfigs.Contains(config);
        }

        #region Члены IEquatable<UpdateModel>

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return Equals(obj as InfoBaseUpdate);
        }

        /// <summary>
        /// Сравнить текущий объект с заданным
        /// </summary>
        /// <param name="other">Объект, с которым происходит сравнение</param>
        /// <returns>TRUE - объекты равны, FALSE - в противном случае</returns>
        public bool Equals(InfoBaseUpdate? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return IsTransitive == other.IsTransitive
                   && Id == other.Id
                   && DestinationConfig == other.DestinationConfig;
        }

        /// <summary>
        /// Рассчитать хэш-код объекта
        /// </summary>
        /// <returns>Хэш-код объекта</returns>
        public override int GetHashCode()
        {
            int hash = IsTransitive.GetHashCode();

            if (Id != null)
                hash ^= Id.GetHashCode();

            if (DestinationConfig != null)
                hash ^= DestinationConfig.GetHashCode();

            if (SourseConfigs != null)
            {
                hash ^= HashCalculator.CalculateHash(SourseConfigs);
            }

            return hash;
        }

        #endregion

        #region Перегрузка операторов "==" и "!="

        /// <summary>
        /// Оператор сравнения двух объектов класса UpdateModel на равенство
        /// </summary>
        /// <param name="left">Сравниваемый объект</param>
        /// <param name="right">Объект, с которым сравниваем</param>
        /// <returns>TRUE - объекты идентичны, FALSE - в противном случае</returns>
        public static bool operator ==(InfoBaseUpdate? left, InfoBaseUpdate? right)
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
        /// Оператор сравнения двух объектов класса UpdateModel на неравенство
        /// </summary>
        /// <param name="left">Сравниваемый объект</param>
        /// <param name="right">Объект, с которым сравниваем</param>
        /// <returns>TRUE - объекты отличаются друг от друга, FALSE - объекты идентичны</returns>
        public static bool operator !=(InfoBaseUpdate? left, InfoBaseUpdate? right)
        {
            return !(left == right);
        }

        #endregion
    }
}
