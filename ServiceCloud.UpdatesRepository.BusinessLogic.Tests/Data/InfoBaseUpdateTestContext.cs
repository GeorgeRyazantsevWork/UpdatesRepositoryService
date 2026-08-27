using ServiceCloud.UpdatesRepository.BusinessLogic.Data;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Tests.Data
{
    class InfoBaseUpdateTestContext
    {
        public static InfobaseConfig DestinationConfig = new InfobaseConfig()
        {
            Publisher = "ООО СервисКлауд",
            Name = "Облако",
            Version = new InfoBaseConfigVersion("1.0.0.1"),
            Edition = InfoBaseConfigEdition.Unknown
        };

        public static InfobaseConfig[] SourseConfigs = new InfobaseConfig[1]
        {
            new InfobaseConfig()
            {
                Publisher = "ООО СервисКлауд",
                Name = "Облако",
                Version = new InfoBaseConfigVersion("1.0.0.0"),
                Edition = InfoBaseConfigEdition.Unknown
            }
        };

        /// <summary>
        /// Набор объектов InfoBaseUpdate, не совпадающих с тестовым 
        /// </summary>
        public static IEnumerable<InfoBaseUpdate> InequivalentOperand()
        {
            // Не совпадает свойство IsTransitive
            yield return new InfoBaseUpdate()
            {
                DestinationConfig = DestinationConfig,
                SourseConfigs = SourseConfigs,
                IsTransitive = true
            };

            // Не равны целевые конфигурации
            yield return new InfoBaseUpdate()
            {
                DestinationConfig = new InfobaseConfig(),
                SourseConfigs = SourseConfigs,
                IsTransitive = false
            };

            // Не совпадает свойство Id
            yield return new InfoBaseUpdate()
            {
                Id = "1",
                DestinationConfig = DestinationConfig,
                SourseConfigs = SourseConfigs,
                IsTransitive = false
            };
        }
    }
}
