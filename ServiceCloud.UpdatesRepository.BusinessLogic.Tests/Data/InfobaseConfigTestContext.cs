using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using System.Collections;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Tests.Data
{
    static class InfobaseConfigTestContext
    {
        public static InfobaseConfig TestConfig = new InfobaseConfig()
        {
            Publisher = "ООО СервисКлауд",
            Name = "Облако",
            Version = new InfoBaseConfigVersion("1.0.0.1")
        };

        public static IEnumerable CompareCases()
        {
            yield return new object[]
            {
               new InfobaseConfig()
               {
                   Publisher = "B",
                   Name = "A",
                   Version = new InfoBaseConfigVersion("0.0.0.1")
               },

               new InfobaseConfig()
               {
                   Publisher = "A",
                   Name = "A",
                   Version = new InfoBaseConfigVersion("0.0.0.1")
               },

               1
            };

            yield return new object[]
            {
               new InfobaseConfig()
               {
                   Publisher = "A",
                   Name = "B",
                   Version = new InfoBaseConfigVersion("0.0.0.1")
               },

               new InfobaseConfig()
               {
                   Publisher = "A",
                   Name = "B",
                   Version = new InfoBaseConfigVersion("0.0.0.1")
               },

               0
            };

            yield return new object[]
            {
               new InfobaseConfig()
               {
                   Publisher = "A",
                   Name = "A",
                   Version = new InfoBaseConfigVersion("0.0.0.1")
               },

               new InfobaseConfig()
               {
                   Publisher = "A",
                   Name = "B",
                   Version = new InfoBaseConfigVersion("0.0.0.1")
               },

               -1
            };
        }

        public static IEnumerable<InfobaseConfig> InequivalentOperands()
        {
            // Не совпадает Publisher
            yield return new InfobaseConfig
            {
                Publisher = "СервисКлауд",
                Name = "Облако",
                Version = new InfoBaseConfigVersion("1.0.0.1")
            };

            // Не совпадает Name
            yield return new InfobaseConfig
            {
                Publisher = "СервисКлауд",
                Name = "Яблако",
                Version = new InfoBaseConfigVersion("1.0.0.1")
            };

            // Не совпадает Version
            yield return new InfobaseConfig
            {
                Publisher = "СервисКлауд",
                Name = "Яблако",
                Version = new InfoBaseConfigVersion("1.0.0.0")
            };

            // Не совпадает Edition
            yield return new InfobaseConfig
            {
                Publisher = "СервисКлауд",
                Name = "Яблако",
                Version = new InfoBaseConfigVersion("1.0.0.1"),
                Edition = InfoBaseConfigEdition.BASE
            };
        }
    }
}
