using ServiceCloud.UpdatesRepository.BusinessLogic.Helpers;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Data
{
    /// <summary>
    /// Описывает версию конфигурации информационной базы
    /// </summary>
    public sealed class InfoBaseConfigVersion : IComparable, IComparable<InfoBaseConfigVersion>, IEquatable<InfoBaseConfigVersion>
    {
        private readonly string value = string.Empty;

        /// <summary>
        /// Конструктор объектов класса
        /// </summary>
        public InfoBaseConfigVersion()
        { }

        /// <summary>
        /// Конструктор объектов класса
        /// </summary>
        /// <param name="value">Строковое представление версии конфигурации ИБ</param>
        /// <exception cref="ArgumentException">Не задан параметр или в качестве значения указана пустая строка</exception>
        public InfoBaseConfigVersion(string? value)
        {
            this.value = value ?? string.Empty;
        }

        /// <summary>
        /// Конструктор объектов класса
        /// </summary>
        /// <param name="major">Основной номер версии</param>
        /// <param name="minor">Дополнительный номер версии</param>
        public InfoBaseConfigVersion(int major, int minor)
        {
            value = $"{major}.{minor}";
        }

        /// <summary>
        /// Конструктор объектов класса
        /// </summary>
        /// <param name="major">Основной номер версии</param>
        /// <param name="minor">Дополнительный номер версии</param>
        /// <param name="build">Номер сборки</param>
        public InfoBaseConfigVersion(int major, int minor, int build)
        {
            value = $"{major}.{minor}.{build}";
        }

        /// <summary>
        /// Конструктор объектов класса
        /// </summary>
        /// <param name="major">Основной номер версии</param>
        /// <param name="minor">Дополнительный номер версии</param>
        /// <param name="build">Номер сборки</param>
        /// <param name="revision">Номер редакции</param>
        public InfoBaseConfigVersion(int major, int minor, int build, int revision)
        {
            value = $"{major}.{minor}.{build}.{revision}";
        }

        /// <inheritdoc/>
        public int CompareTo(InfoBaseConfigVersion? other)
        {
            return NaturalStringComparer.Instance.Compare(value, other?.value);
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj)
        {
            return CompareTo(obj as InfoBaseConfigVersion);
        }

        /// <inheritdoc/>
        public bool Equals(InfoBaseConfigVersion? other)
        {
            return NaturalStringComparer.Instance.Compare(value, other?.value) == 0;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as InfoBaseConfigVersion);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return value;
        }

        #region Операторы сравнения

        /// <summary>
        /// Сравнение двух объектов типа <see cref="InfoBaseConfigVersion"/> на равенство
        /// </summary>
        /// <param name="left">Первый сравниваемый объект</param>
        /// <param name="right">Второй сравниваемый объект</param>
        /// <returns>TRUE - объекты равны, FALSE - не вравны</returns>
        public static bool operator ==(InfoBaseConfigVersion? left, InfoBaseConfigVersion? right)
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
        /// Сравнение двух объектов типа <see cref="InfoBaseConfigVersion"/> на неравенство
        /// </summary>
        /// <param name="left">Первый сравниваемый объект</param>
        /// <param name="right">Второй сравниваемый объект</param>
        /// <returns>TRUE - объекты НЕ равны, FALSE - вравны</returns>
        public static bool operator !=(InfoBaseConfigVersion? left, InfoBaseConfigVersion? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Cравнение двух объектов типа <see cref="InfoBaseConfigVersion"/> на предмет: первый объект меньше второго
        /// </summary>
        /// <param name="left">Первый сравниваемый объект</param>
        /// <param name="right">Второй сравниваемый объект</param>
        /// <returns>TRUE - первый объект меньше второго, FALSE - в противном случае</returns>
        public static bool operator <(InfoBaseConfigVersion? left, InfoBaseConfigVersion? right)
        {
            if (left != null)
            {
                return left.CompareTo(right) < 0;
            }
            return false;
        }

        /// <summary>
        /// Cравнение двух объектов типа <see cref="InfoBaseConfigVersion"/> на предмет: первый объект больше второго
        /// </summary>
        /// <param name="left">Первый сравниваемый объект</param>
        /// <param name="right">Второй сравниваемый объект</param>
        /// <returns>TRUE - первый объект больше второго, FALSE - в противном случае</returns>
        public static bool operator >(InfoBaseConfigVersion? left, InfoBaseConfigVersion? right)
        {
            if (left != null)
            {
                return left.CompareTo(right) > 0;
            }
            return false;
        }

        /// <summary>
        /// Cравнение двух объектов типа <see cref="InfoBaseConfigVersion"/> на предмет: первый объект меньше или равен второму
        /// </summary>
        /// <param name="left">Первый сравниваемый объект</param>
        /// <param name="right">Второй сравниваемый объект</param>
        /// <returns>TRUE - первый объект меньше или равен второму, FALSE - в противном случае</returns>
        public static bool operator <=(InfoBaseConfigVersion? left, InfoBaseConfigVersion? right)
        {
            if (left != null)
            {
                return left.CompareTo(right) <= 0;
            }
            return true;
        }

        /// <summary>
        /// Cравнение двух объектов типа <see cref="InfoBaseConfigVersion"/> на предмет: первый объект больше или равен второму
        /// </summary>
        /// <param name="left">Первый сравниваемый объект</param>
        /// <param name="right">Второй сравниваемый объект</param>
        /// <returns>TRUE - первый объект больше или равен второму, FALSE - в противном случае</returns>
        public static bool operator >=(InfoBaseConfigVersion? left, InfoBaseConfigVersion? right)
        {
            if (left != null)
            {
                return left.CompareTo(right) >= 0;
            }
            return right == null;
        }

        #endregion
    }
}
