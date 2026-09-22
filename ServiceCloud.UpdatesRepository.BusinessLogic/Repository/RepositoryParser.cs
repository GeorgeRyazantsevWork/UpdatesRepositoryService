using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using System.Diagnostics.CodeAnalysis;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Repository
{
    /// <summary>
    /// Класс-помощник для преобразования найденного файла конфигурации в объект типа <see cref="InfoBaseUpdate"/>
    /// </summary>
    public static class RepositoryParser
    {
        /// <summary>
        /// Проверяет наличие обновления по заданному пути
        /// </summary>
        /// <param name="path">Путь к каталогу</param>
        /// <returns>TRUE - каталог существует и в нем содержатся файлы обновления, FASLE - в противном случае</returns>
        public static bool ValidateUpdateFiles(string path)
        {
            var cfuFilePath = Path.Combine(path, "1cv8.cfu");
            var cucFilePath = Path.Combine(path, "1cv8.cuc");

            bool isOrdinaryUpdateFileSet = File.Exists(cfuFilePath) && File.Exists(cucFilePath);

            var cfupFilePath = Path.Combine(path, "1cv8p.cfu");
            var cucpFilePath = Path.Combine(path, "1cv8p.cuc");

            bool isCORPTransitionFileSet = File.Exists(cfupFilePath) && File.Exists(cucpFilePath);

            var cfubFilePath = Path.Combine(path, "1cv8b.cfu");
            var cucbFilePath = Path.Combine(path, "1cv8b.cuc");

            bool isPROFTransitionFileSet = File.Exists(cfubFilePath) && File.Exists(cucbFilePath);

            return Directory.Exists(path) && (isOrdinaryUpdateFileSet || isCORPTransitionFileSet || isPROFTransitionFileSet);
        }

        /// <summary>
        /// Анализирует содержимое .сuc файла
        /// </summary>
        /// <param name="path">Полный путь к файлу</param>
        /// <param name="update">Объект в который будет помещен результат</param>
        /// <returns>Обновление, описанное в cuc-файле</returns>
        public static bool TryParseСUCFile(string path, [NotNullWhen(true)] out InfoBaseUpdate? update)
        {
            update = null;
            try
            {
                // читаем файл
                string rawData;

                using (var reader = File.OpenText(path))
                {
                    rawData = reader.ReadToEnd();
                }

                update = new InfoBaseUpdate();

                // извлекаем конфигурацию, хранящуюся в обновлении
                update.DestinationConfig = ParseDestinationConfigDescription(rawData);

                // извлекаем список конфигураций, для которых обновление подходит
                update.SourceConfigs = ParseSourceConfigsListDescription(rawData);

                // определяем, является ли обновление переходным
                update.IsTransitive = IsTransitiveUpdate(path);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static bool IsTransitiveUpdate(string cucFilePath)
        {
            var cucFileName = Path.GetFileName(cucFilePath);

            if (cucFileName[4] == 'b' || cucFileName[4] == 'p')
            {
                return true;
            }

            return false;
        }

        private static InfobaseConfig ParseDestinationConfigDescription(string text)
        {
            // вычисляем позицию конца записи
            var configRecordEnd = text.LastIndexOf("\",") + 1;
            // вычисляем позицию начала записи
            var configRecordBegin = text.LastIndexOf(",\"", configRecordEnd);


            for (int fieldEnd, count = 0; count < 2; count++)
            {
                fieldEnd = configRecordBegin;
                configRecordBegin = text.LastIndexOf(",\"", fieldEnd);
            }

            configRecordBegin += 1;

            string configRecord = text.Substring(configRecordBegin, configRecordEnd - configRecordBegin);
            char[] separator = new char[] { ',' };

            return ParseConfigurationDescription(configRecord, separator);
        }

        private static InfoBaseConfigEdition GetEdition(string configName, string publisher)
        {
            if (publisher != "Фирма \"1С\"")
            {
                return InfoBaseConfigEdition.Unknown;
            }
            if (configName.EndsWith("Базовая"))
            {
                return InfoBaseConfigEdition.BASE;
            }
            if (configName.EndsWith("КОРП"))
            {
                return InfoBaseConfigEdition.CORP;
            }

            return InfoBaseConfigEdition.PROF;
        }

        private static InfobaseConfig ParseConfigurationDescription(string text, char[] separator)
        {
            InfobaseConfig config = new InfobaseConfig();
            string[] fields = text.Split(separator, 3, StringSplitOptions.None);

            config.Publisher = TrimQuots(fields[1]);
            config.Name = TrimQuots(fields[0]);
            config.Version = new InfoBaseConfigVersion(
                RemoveSecondaryVersion(
                    TrimQuots(fields[2])));
            config.Edition = GetEdition(config.Name, config.Publisher);

            return config;
        }

        private static string TrimQuots(string text)
        {
            if (text == null)
            {
                throw new ArgumentException();
            }

            if (text.StartsWith("\"") && text.EndsWith("\""))
            {
                text = text.Substring(1, text.Length - 2);
            }

            return text.Replace("\"\"", "\"");
        }

        private static string RemoveSecondaryVersion(string versionText)
        {
            if (versionText == null)
            {
                throw new ArgumentException();
            }

            var delimeterPosition = versionText.IndexOf('/');
            if (delimeterPosition > 0)
            {
                versionText = versionText.Substring(0, delimeterPosition);
            }

            return versionText;
        }

        private static LinkedList<InfobaseConfig> ParseSourceConfigsListDescription(string text)
        {
            var result = new LinkedList<InfobaseConfig>();

            // извлекаем количество записей в списке
            var counterFieldEnd = text.IndexOf(",\"");
            var counterFieldBegin = text.LastIndexOf(",", counterFieldEnd - 1) + 1;
            var counterFieldText = text.Substring(counterFieldBegin, counterFieldEnd - counterFieldBegin);

            int counterFieldValue = int.Parse(counterFieldText);

            var configRecordBegin = counterFieldEnd + 1;
            char[] separator = new char[] { ',' };
            // Для каждой записи проводим разбор
            for (int recordsCount = 0; recordsCount < counterFieldValue; recordsCount++)
            {
                // вычисляем индекс последнего символа в записи
                var configRecordEnd = FindIndexOf(text, "\",", configRecordBegin, 2) + 1;
                // получаем текст всей записи
                var configRecordText = text.Substring(configRecordBegin, configRecordEnd - configRecordBegin);

                // разбираем запись и добавляем ее в список
                var config = ParseConfigurationDescription(configRecordText, separator);
                result.AddLast(config);

                // вычисляем начало следующей записи
                configRecordBegin = configRecordEnd + 36 + 2;
            }

            return result;
        }

        /// <summary>
        /// Вспомогательный метод. Находит заданную подстроку
        /// </summary>
        /// <param name="text">исходная строка</param>
        /// <param name="value">искомая подстрока</param>
        /// <param name="start">позиция с которой начинается поиск</param>
        /// <param name="time">номер вхождения</param>
        /// <returns>Индекс символа, с которого начинается N-е вхождение заданной подстроки в исходную</returns>
        private static int FindIndexOf(string text, string value, int start, int time)
        {
            int result = 0;
            for (int count = 0, last = start; count <= time; count++)
            {
                result = text.IndexOf(value, last);
                last = result + 1;
            }
            return result;
        }
    }
}