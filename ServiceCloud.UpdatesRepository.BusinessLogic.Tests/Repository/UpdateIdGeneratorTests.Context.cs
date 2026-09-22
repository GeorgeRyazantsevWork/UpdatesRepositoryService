using ServiceCloud.UpdatesRepository.BusinessLogic.Data;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Repository.Tests
{
    public partial class UpdateIdGeneratorTests
    {
        class Context
        {
            public InfoBaseUpdate ValidUpdate
            {
                get => new InfoBaseUpdate()
                {
                    DestinationConfig = new InfobaseConfig()
                    {
                        Publisher = "Фирма \"1С\"",
                        Name = "БухгалтерияПредприятия",
                        VersionString = "3.1.16.94",
                        Edition = InfoBaseConfigEdition.PROF
                    },

                    SourceConfigs = new InfobaseConfig[]
                    {
                        new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1С\"",
                            Name = "БухгалтерияПредприятия",
                            VersionString = "3.1.16.91",
                            Edition = InfoBaseConfigEdition.PROF
                        }
                    },

                    IsTransitive = false
                };
            }

            public static IEnumerable<InfoBaseUpdate> InvalidUpdateCases
            {
                get
                {
                    // "Пустой" объект
                    yield return new InfoBaseUpdate();

                    // Не задана целевая конфигурация
                    yield return new InfoBaseUpdate()
                    {
                        SourceConfigs = new InfobaseConfig[]
                        {
                        new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1С\"",
                            Name = "БухгалтерияПредприятия",
                            VersionString = "3.1.16.91",
                            Edition = InfoBaseConfigEdition.PROF
                        }
                    },

                        IsTransitive = false
                    };

                    // Задана не корректная целевая конфигурация
                    yield return new InfoBaseUpdate()
                    {
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1С\"",
                            Name = string.Empty,
                            VersionString = "3.1.16.94",
                            Edition = InfoBaseConfigEdition.PROF
                        },

                        SourceConfigs = new InfobaseConfig[]
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "Фирма \"1С\"",
                                Name = "БухгалтерияПредприятия",
                                VersionString = "3.1.16.91",
                                Edition = InfoBaseConfigEdition.PROF
                            }
                        },

                        IsTransitive = false
                    };

                    // Не задана коллекция исходных конфигураций
                    yield return new InfoBaseUpdate()
                    {
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1С\"",
                            Name = "БухгалтерияПредприятия",
                            VersionString = "3.1.16.94",
                            Edition = InfoBaseConfigEdition.PROF
                        },

                        IsTransitive = false
                    };

                    // Задана не корректная конфигурация в коллекции исходных
                    yield return new InfoBaseUpdate()
                    {
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1С\"",
                            Name = "БухгалтерияПредприятия",
                            VersionString = "3.1.16.94",
                            Edition = InfoBaseConfigEdition.PROF
                        },

                        SourceConfigs = new InfobaseConfig[]
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "Фирма \"1С\"",
                                Name = string.Empty,
                                VersionString = "3.1.16.91",
                                Edition = InfoBaseConfigEdition.PROF
                            }
                        },

                        IsTransitive = false
                    };
                }
            }

            public static IEnumerable<Tuple<InfoBaseUpdate, string>> IdCases
            {
                get
                {
                    yield return new Tuple<InfoBaseUpdate, string>(
                        new InfoBaseUpdate()
                        {
                            DestinationConfig = new InfobaseConfig
                            {
                                Publisher = "Фирма \"1С\"",
                                Name = "БухгалтерияПредприятия",
                                VersionString = "3.1.16.94",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            SourceConfigs = new InfobaseConfig[]
                            {
                                new InfobaseConfig()
                                {
                                    Publisher = "Фирма \"1С\"",
                                    Name = "БухгалтерияПредприятия",
                                    VersionString = "3.1.16.91",
                                    Edition = InfoBaseConfigEdition.PROF
                                }
                            },

                            IsTransitive = false
                        },
                        "6B6AF8983A79BA2467C4BACCD1C2DC85A509A42608FE1B31CAB4E212D13BE6FD");

                    yield return new Tuple<InfoBaseUpdate, string>(
                        new InfoBaseUpdate()
                        {
                            DestinationConfig = new InfobaseConfig
                            {
                                Publisher = "Фирма 1С",
                                Name = "БухгалтерияПредприятия",
                                VersionString = "3.1.16.94",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            SourceConfigs = new InfobaseConfig[]
                            {
                                new InfobaseConfig()
                                {
                                    Publisher = "Фирма 1С",
                                    Name = "БухгалтерияПредприятия",
                                    VersionString = "3.1.16.91",
                                    Edition = InfoBaseConfigEdition.PROF
                                }
                            },

                            IsTransitive = false
                        },
                        "907C8D1F27B1D78CDAFFF891F1AB9CD3BD0459E39B137A1F50986E0F7011CF31");

                    yield return new Tuple<InfoBaseUpdate, string>(
                        new InfoBaseUpdate()
                        {
                            DestinationConfig = new InfobaseConfig
                            {
                                Publisher = "Фирма \"1С\"",
                                Name = "БухгалтерияПредприятияКорп",
                                VersionString = "3.2.18.123",
                                Edition = InfoBaseConfigEdition.CORP
                            },

                            SourceConfigs = new InfobaseConfig[]
                            {
                                new InfobaseConfig()
                                {
                                    Publisher = "Фирма \"1С\"",
                                    Name = "БухгалтерияПредприятия",
                                    VersionString = "3.2.18.122",
                                    Edition = InfoBaseConfigEdition.CORP
                                }
                            },

                            IsTransitive = false
                        },
                        "D016558015ACCAA8B45823883A2B435AFD61D91F2DD4290ABB71252B3F60253A");
                }
            }
        }
    }
}
