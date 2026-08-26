using ServiceCloud.UpdatesRepository.BusinessLogic.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Repository
{
    /// <summary>
    /// Генератор идентификатора для обновления информационной базы внутри репозитория
    /// </summary>
    public sealed class UpdateIdGenerator : IDisposable
    {
        SHA256 hashCalculator;
        StringBuilder builder;
        const char DELIMITER = ';';
        bool disposed;

        /// <summary>
        /// Конструктор объектов класса
        /// </summary>
        public UpdateIdGenerator()
        {
            hashCalculator = SHA256.Create();
            builder = new StringBuilder();
        }

        /// <summary>
        /// Сгенерировать новый идентификатор для обновления заданной конфигурации базы 1С
        /// </summary>
        /// <param name="update">Данные обновления конфигурации ИБ 1С</param>
        /// <exception cref="ArgumentNullException">Не задана ссылка на объект</exception>
        /// <exception cref="ArgumentException">Объект содержит недостаточно данных для генерации идентификатора</exception>
        /// <exception cref="ObjectDisposedException">Ресурсы объекта освобождены. Дальнейшее использование объекта невозможно</exception>
        public string NewId(UpdateModel? update)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(typeof(UpdateIdGenerator).ToString());
            }

            Validate(update);

            string id = BuildId(update);
            byte[] hashValue = hashCalculator.ComputeHash(Encoding.UTF8.GetBytes(id));

            return HexString(hashValue);
        }

        private string BuildId(UpdateModel update)
        {
            builder.Clear();

            AddToBuilder(update.DestinationConfig!);

            builder.Append(DELIMITER);
            var orderedSorceConfigs = update.SourseConfigs!.OrderBy(item => item);

            foreach (var config in orderedSorceConfigs)
            {
                AddToBuilder(config);
                builder.Append(DELIMITER);
            }

            builder.Append(update.IsTransitive);

            return builder.ToString();
        }

        private void AddToBuilder(ConfigurationModel config)
        {
            builder.Append(config.Publisher);
            builder.Append(DELIMITER);
            builder.Append(config.Name);
            builder.Append(DELIMITER);
            builder.Append(config.Version?.ToString());
            builder.Append(DELIMITER);
            builder.Append(config.Edition.ToString());
        }

        private void Validate([NotNull] UpdateModel? update)
        {
            if (update == null)
            {
                throw new ArgumentNullException("Не задано обновление");
            }

            if (update.DestinationConfig == null
                || update.DestinationConfig == ConfigurationModel.Empty)
            {
                throw new ArgumentException("У объекта, описывающего обновление, не задана целевая конфигурация");
            }

            if (!CheckInfoBaseConfig(update.DestinationConfig))
            {
                throw new ArgumentException("У объекта, описывающего обновление, не корректно заданы свойства целевой конфигурации");
            }

            if (update.SourseConfigs == null
               || update.SourseConfigs.Any() == false)
            {
                throw new ArgumentException("У объекта, описывающего обновление, не задана коллекция исходных конфигураций");
            }

            foreach (var config in update.SourseConfigs)
            {
                if (!CheckInfoBaseConfig(config))
                {
                    throw new ArgumentException("У объекта, описывающего обновление, не корректно заданы свойства в коллекции исходных конфигураций");
                }
            }
        }

        private bool CheckInfoBaseConfig(ConfigurationModel config)
        {
            return !(string.IsNullOrEmpty(config.Publisher)
                    || string.IsNullOrEmpty(config.Name)
                    || string.IsNullOrEmpty(config.VersionString));
        }

        private string HexString(byte[] hashValue)
        {
            builder.Clear();
            foreach (var item in hashValue)
            {
                builder.Append(item.ToString("X2"));
            }
            return builder.ToString();
        }

        #region Члены IDisposable

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            hashCalculator.Dispose();
            hashCalculator = null!;

            if (disposing)
            {
                builder = null!;
            }

            disposed = true;
        }

        ~UpdateIdGenerator()
        {
            Dispose(disposing: false);
        }

        /// <inheritdoc cref="IDisposable" />
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}