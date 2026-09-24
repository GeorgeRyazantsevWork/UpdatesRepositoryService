using ServiceCloud.UpdatesRepository.BusinessLogic.Data;
using System.Collections;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Tests.Repository
{
    /// <summary>
    /// Вспомогательный класс для тестирования класса UpdatesRepository
    /// </summary>
    public class UpdatesRepositoryTestContext
    {
        private List<InfobaseConfig> existingConfigs = new List<InfobaseConfig>();

        private List<InfoBaseUpdate> existingUpdates = new List<InfoBaseUpdate>();

        private List<string> existingLocation = new List<string>();

        public UpdatesRepositoryTestContext()
        {
            foreach (var entry in TestRepositoryEntries)
            {
                existingConfigs.Add(entry.Update.DestinationConfig!);
                existingUpdates.Add(entry.Update);
                existingLocation.Add(entry.Location);
            }
            UnknownConfiguration = new InfobaseConfig()
            {
                Name = "Микрофининсы",
                Publisher = "Рарус",
                VersionString = "1.0.5.186",
                Edition = InfoBaseConfigEdition.PROF
            };
        }

        /// <summary>
        /// Вспомогательная структура, описывающая элемент репозитория
        /// </summary>
        public struct RepositoryEntry
        {
            public InfoBaseUpdate Update;
            public string Location;
        }

        /// <summary>
        /// Содержимое тестового репозитория. Набор обновлений и их расположений
        /// </summary>
        public static IEnumerable<RepositoryEntry> TestRepositoryEntries
        {
            get
            {
                // Бухгалтерия предприятия, Фирма 1С, 3.0.173.32/3.3.3.315
                yield return new RepositoryEntry()
                {
                    Update = new InfoBaseUpdate()
                    {
                        Id = "9B7D1893A6BDF4D581537ACBF80940B518B441604D073ED66EA6B1D692970502",

                        DestinationConfig = new InfobaseConfig()
                        {
                            Name = "БухгалтерияПредприятия",
                            Publisher = "Фирма \"1С\"",
                            VersionString = "3.0.173.32",
                            Edition = InfoBaseConfigEdition.PROF
                        },

                        SourceConfigs = new List<InfobaseConfig>
                        {
                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.172.32",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.171.32",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.170.32",
                                Edition = InfoBaseConfigEdition.PROF
                            },
                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.169.32",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.168.32",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.167.32",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.166.32",
                                Edition = InfoBaseConfigEdition.PROF
                            }
                        }
                    },
                    Location = Path.Combine(TestRepositoryPath, "1c", "DoubleVersion", "3.0.173.32")
                };

                // Бухгалтерия предприятия, Фирма 1С, 3.0.58.26
                yield return new RepositoryEntry()
                {
                    Update = new InfoBaseUpdate()
                    {
                        Id = "FA93FD51F05E33EC3DA84E5A785F932FA04D5F1DED777B925F5B6F728C662A3F",

                        DestinationConfig = new InfobaseConfig()
                        {
                            Name = "БухгалтерияПредприятия",
                            Publisher = "Фирма \"1С\"",
                            VersionString = "3.0.58.26",
                            Edition = InfoBaseConfigEdition.PROF
                        },

                        SourceConfigs = new List<InfobaseConfig>
                        {
                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.55.14",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.55.16",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.56.22",
                                Edition = InfoBaseConfigEdition.PROF
                            },
                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.57.10",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.57.15",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.57.17",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.58.20",
                                Edition = InfoBaseConfigEdition.PROF
                            }
                        }
                    },
                    Location = Path.Combine(TestRepositoryPath, "1c", "Accounting", "3.0.58.26")
                };

                // Бухгалтерия предприятия, Фирма 1С, 3.0.58.41
                yield return new RepositoryEntry()
                {
                    Update = new InfoBaseUpdate()
                    {
                        Id = "103EB5F83D0EE3AAB321E667FE3641771874AD6A6AB3FDAF871639EFFA6A6FC7",

                        DestinationConfig = new InfobaseConfig()
                        {
                            Name = "БухгалтерияПредприятия",
                            Publisher = "Фирма \"1С\"",
                            VersionString = "3.0.58.41",
                            Edition = InfoBaseConfigEdition.PROF
                        },

                        SourceConfigs = new List<InfobaseConfig>
                        {
                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.55.16",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.56.22",
                                Edition = InfoBaseConfigEdition.PROF
                            },
                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.57.10",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.57.15",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.57.17",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.58.20",
                                Edition = InfoBaseConfigEdition.PROF
                            },

                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "3.0.58.26",
                                Edition = InfoBaseConfigEdition.PROF
                            }
                        }
                    },
                    Location = Path.Combine(TestRepositoryPath, "1c", "Accounting", "3.0.58.41")
                };

                // Салон красоты, ООО "Лаборатория программного обеспечения", 2.0.9.2
                yield return new RepositoryEntry()
                {
                    Update = new InfoBaseUpdate()
                    {
                        Id = "4E77BFC56C40679DAA891EEC6A0F82C622CEBFC56E4FC07B94292CD53998B5A4",

                        DestinationConfig = new InfobaseConfig()
                        {
                            Name = "СалонКрасоты",
                            Publisher = "ООО \"Лаборатория программного обеспечения\"",
                            VersionString = "2.0.9.2",
                            Edition = InfoBaseConfigEdition.Unknown
                        },

                        SourceConfigs = new List<InfobaseConfig>
                        {
                            new InfobaseConfig()
                            {
                                Name = "СалонКрасоты",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "2.0.8.1",
                                Edition = InfoBaseConfigEdition.PROF

                            },

                            new InfobaseConfig()
                            {
                                Name = "СалонКрасоты",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "2.0.9.1",
                                Edition = InfoBaseConfigEdition.PROF
                            }
                        }
                    },
                    Location = Path.Combine(TestRepositoryPath, "helix", "salon", "2.0.9.2")
                };

                // Бухгалтерия предприятия, Фирма 1С, 2.0.66.135 (переход на ПРОФ)
                yield return new RepositoryEntry()
                {
                    Update = new InfoBaseUpdate()
                    {
                        Id = "FBF3DDE3DC6E957F0C1AAFEC4ACFF615662A5129C03AAD6DF8ACC0DD627AEB9C",

                        DestinationConfig = new InfobaseConfig()
                        {
                            Name = "БухгалтерияПредприятия",
                            Publisher = "Фирма \"1С\"",
                            VersionString = "2.0.66.135",
                            Edition = InfoBaseConfigEdition.PROF
                        },

                        SourceConfigs = new InfobaseConfig[1]
                        {
                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятияБазовая",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "2.0.66.135",
                                Edition = InfoBaseConfigEdition.BASE
                            }
                        },

                        IsTransitive = true
                    },
                    Location = Path.Combine(TestRepositoryPath, "1c", "BaseToProf", "2.0.66.135")
                };

                // Бухгалтерия предприятия, Фирма 1С, 2.0.66.135 (переход на КОРП)
                yield return new RepositoryEntry()
                {
                    Update = new InfoBaseUpdate()
                    {
                        Id = "779E1403475CB22BD42F94EDE3ED4FD32C95E0FA389C5C5B512ABB032AEEDD9A",

                        DestinationConfig = new InfobaseConfig()
                        {
                            Name = "БухгалтерияПредприятияКОРП",
                            Publisher = "Фирма \"1С\"",
                            VersionString = "2.0.66.135",
                            Edition = InfoBaseConfigEdition.CORP
                        },

                        SourceConfigs = new InfobaseConfig[1]
                        {
                            new InfobaseConfig()
                            {
                                Name = "БухгалтерияПредприятия",
                                Publisher = "Фирма \"1С\"",
                                VersionString = "2.0.66.135",
                                Edition = InfoBaseConfigEdition.PROF
                            }
                        },

                        IsTransitive = true
                    },
                    Location = Path.Combine(TestRepositoryPath, "1c", "ProfToCorp", "2.0.66.135")
                };
            }
        }

        /// <summary>
        /// Полный путь к каталогу с файлами репозитория
        /// </summary>
        public static string TestRepositoryPath
        {
            get
            {
                return Path.Combine(TestContext.CurrentContext.TestDirectory, "DataSet", "TestRepo");
            }
        }

        /// <summary>
        /// Набор конфигураций, которые можно получить, установив обновления из репозитория
        /// </summary>
        public List<InfobaseConfig> ExistingConfigs
        {
            get => existingConfigs;
            set => existingConfigs = value ?? throw new ArgumentNullException();
        }

        public InfobaseConfig UnknownConfiguration { get; set; }

        /// <summary>
        /// Набор обновлений содержащихся в репозитории
        /// </summary>
        public List<InfoBaseUpdate> ExistingUpdates
        {
            get => existingUpdates;
            set => existingUpdates = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Набор расположений обновлений, содержащихся в репозитории
        /// </summary>
        public List<string> ExistingLocations
        {
            get => existingLocation;
            set => existingLocation = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Набор данных для тестирования метода GetPotentialUpdate
        /// </summary>
        /// <returns>параметры теста</returns>
        public static IEnumerable PotentialUpdatesCorrectCases()
        {
            // Вариант с пустым результирующим списком
            yield return new object[]
            {
                new List<InfoBaseUpdate>()
                {
                    new InfoBaseUpdate()
                    {
                        Id = "100",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "publisher",
                            Name = "target",
                            VersionString = "0.0.0.2"
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "publisher",
                                Name = "target",
                                VersionString = "0.0.0.1"
                            }
                        }
                    },

                    new InfoBaseUpdate()
                    {
                        Id = "101",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "publisher",
                            Name = "target",
                            VersionString = "0.0.0.3"
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "publisher",
                                Name = "target",
                                VersionString = "0.0.0.2"
                            },
                        }
                    }
                },

                new InfobaseConfig()
                {
                    Publisher = "another_publisher",
                    Name = "target",
                    VersionString = "0.0.0.2"
                },

                new List<InfoBaseUpdate>()
            };

            // В исходном списке присутствуют неподходящие обновления
            yield return new object[]
            {
                new List<InfoBaseUpdate>()
                {
                    new InfoBaseUpdate()
                    {
                        Id = "101",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "publisher",
                            Name = "target",
                            VersionString = "0.0.0.2"
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "publisher",
                                Name = "target",
                                VersionString = "0.0.0.1"
                            }
                        }
                    },

                    new InfoBaseUpdate()
                    {
                        Id = "102",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "publisher",
                            Name = "target",
                            VersionString = "0.0.0.3"
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "publisher",
                                Name = "target",
                                VersionString = "0.0.0.2"
                            },
                        }
                    },

                    new InfoBaseUpdate()
                    {
                        Id = "103",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "another_publisher",
                            Name = "target",
                            VersionString = "0.0.0.3"
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "another_publisher",
                                Name = "target",
                                VersionString = "0.0.0.2"
                            },
                        }
                    },

                    new InfoBaseUpdate()
                    {
                        Id = "104",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "publisher",
                            Name = "another_name",
                            VersionString = "0.0.0.3"
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "publisher",
                                Name = "another_name",
                                VersionString = "0.0.0.2"
                            },
                        }
                    }

                },

                new InfobaseConfig()
                {
                    Publisher = "publisher",
                    Name = "target",
                    VersionString = "0.0.0.2"
                },

                new List<InfoBaseUpdate>()
                {
                    new InfoBaseUpdate()
                    {
                        Id = "102",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "publisher",
                            Name = "target",
                            VersionString = "0.0.0.3"
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "publisher",
                                Name = "target",
                                VersionString = "0.0.0.2"
                            },
                        }
                    }
                }
            };

            // вариант с переходным обновлением ПРОФ => КОРП
            yield return new object[]
            {
                new List<InfoBaseUpdate>()
                {
                    // Переход с ПРОФ на КОРП
                    new InfoBaseUpdate()
                    {
                        Id = "107",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1C\"",
                            Name = "БухгалтерияПредприятияКОРП",
                            VersionString = "0.0.0.1",
                            Edition = InfoBaseConfigEdition.CORP
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "Фирма \"1C\"",
                                Name = "БухгалтерияПредприятия",
                                VersionString = "0.0.0.1",
                                Edition = InfoBaseConfigEdition.PROF
                            }
                        },

                        IsTransitive = true
                    },

                    // Обновление, которое можно установить после переходного
                    new InfoBaseUpdate()
                    {
                        Id = "108",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1C\"",
                            Name = "БухгалтерияПредприятияКОРП",
                            VersionString = "0.0.0.2",
                            Edition = InfoBaseConfigEdition.CORP
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "Фирма \"1C\"",
                                Name = "БухгалтерияПредприятияКОРП",
                                VersionString = "0.0.0.1",
                                Edition = InfoBaseConfigEdition.CORP
                            }
                        },
                    },

                    // Обновление, которое можно установить вместо переходного
                    new InfoBaseUpdate()
                    {
                        Id = "109",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1C\"",
                            Name = "БухгалтерияПредприятия",
                            VersionString = "0.0.0.2",
                            Edition = InfoBaseConfigEdition.PROF
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "Фирма \"1C\"",
                                Name = "БухгалтерияПредприятия",
                                VersionString = "0.0.0.1",
                                Edition = InfoBaseConfigEdition.PROF
                            }
                        },
                    }
                },

                new InfobaseConfig()
                {
                    Publisher = "Фирма \"1C\"",
                    Name = "БухгалтерияПредприятия",
                    VersionString = "0.0.0.1",
                    Edition = InfoBaseConfigEdition.PROF
                },

                new List<InfoBaseUpdate>()
                {
                    // Переход с ПРОФ на КОРП
                    new InfoBaseUpdate()
                    {
                        Id = "107",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1C\"",
                            Name = "БухгалтерияПредприятияКОРП",
                            VersionString = "0.0.0.1",
                            Edition = InfoBaseConfigEdition.CORP
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "Фирма \"1C\"",
                                Name = "БухгалтерияПредприятия",
                                VersionString = "0.0.0.1",
                                Edition = InfoBaseConfigEdition.PROF
                            }
                        },

                        IsTransitive = true
                    },

                    // Обновление, которое можно установить вместо переходного
                    new InfoBaseUpdate()
                    {
                        Id = "109",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1C\"",
                            Name = "БухгалтерияПредприятия",
                            VersionString = "0.0.0.2",
                            Edition = InfoBaseConfigEdition.PROF
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "Фирма \"1C\"",
                                Name = "БухгалтерияПредприятия",
                                VersionString = "0.0.0.1",
                                Edition = InfoBaseConfigEdition.PROF
                            }
                        },
                    }
                }
            };

            // вариант с переходным обновлением БАЗОВАЯ => ПРОФ
            yield return new object[]
            {
                new List<InfoBaseUpdate>()
                {
                    // Переход с БАЗОВОЙ на ПРОФ
                    new InfoBaseUpdate()
                    {
                        Id = "112",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1C\"",
                            Name = "БухгалтерияПредприятия",
                            VersionString = "0.0.0.1",
                            Edition = InfoBaseConfigEdition.PROF
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "Фирма \"1C\"",
                                Name = "БухгалтерияПредприятияБазовая",
                                VersionString = "0.0.0.1",
                                Edition = InfoBaseConfigEdition.BASE
                            }
                        },

                        IsTransitive = true
                    },

                    // Обновление, которое можно установить после переходного
                    new InfoBaseUpdate()
                    {
                        Id = "113",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1C\"",
                            Name = "БухгалтерияПредприятия",
                            VersionString = "0.0.0.2",
                            Edition = InfoBaseConfigEdition.PROF
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "Фирма \"1C\"",
                                Name = "БухгалтерияПредприятия",
                                VersionString = "0.0.0.1",
                                Edition = InfoBaseConfigEdition.PROF
                            }
                        },
                    },
                },

                new InfobaseConfig()
                {
                    Publisher = "Фирма \"1C\"",
                    Name = "БухгалтерияПредприятияБазовая",
                    VersionString = "0.0.0.1",
                    Edition = InfoBaseConfigEdition.BASE
                },

                new List<InfoBaseUpdate>()
                {
                    // Переход с БАЗОВОЙ на ПРОФ
                    new InfoBaseUpdate()
                    {
                        Id = "112",
                        DestinationConfig = new InfobaseConfig()
                        {
                            Publisher = "Фирма \"1C\"",
                            Name = "БухгалтерияПредприятия",
                            VersionString = "0.0.0.1",
                            Edition = InfoBaseConfigEdition.PROF
                        },

                        SourceConfigs = new List<InfobaseConfig>()
                        {
                            new InfobaseConfig()
                            {
                                Publisher = "Фирма \"1C\"",
                                Name = "БухгалтерияПредприятияБазовая",
                                VersionString = "0.0.0.1",
                                Edition = InfoBaseConfigEdition.BASE
                            }
                        },

                        IsTransitive = true
                    }
                }
            };
        }

        public static IEnumerable<InfobaseConfig> InsufficientConfigs
        {
            get
            {
                yield return new InfobaseConfig();

                yield return new InfobaseConfig()
                {
                    Name = string.Empty,
                    Publisher = string.Empty,
                    VersionString = ""
                };

                yield return new InfobaseConfig()
                {
                    Name = null,
                    Publisher = "Фирма \"1С\"",
                    VersionString = "3.0.58.26",
                    Edition = InfoBaseConfigEdition.PROF
                };

                yield return new InfobaseConfig()
                {
                    Name = string.Empty,
                    Publisher = "Фирма \"1С\"",
                    VersionString = "3.0.58.26",
                    Edition = InfoBaseConfigEdition.PROF
                };

                yield return new InfobaseConfig()
                {
                    Name = "БухгалтерияПредприятия",
                    Publisher = null,
                    VersionString = "3.0.58.26",
                    Edition = InfoBaseConfigEdition.PROF
                };

                yield return new InfobaseConfig()
                {
                    Name = "БухгалтерияПредприятия",
                    Publisher = string.Empty,
                    VersionString = "3.0.58.26",
                    Edition = InfoBaseConfigEdition.PROF
                };

                yield return new InfobaseConfig()
                {
                    Name = "БухгалтерияПредприятия",
                    Publisher = "Фирма \"1С\"",
                    VersionString = null,
                    Edition = InfoBaseConfigEdition.PROF
                };

                yield return new InfobaseConfig()
                {
                    Name = "БухгалтерияПредприятия",
                    Publisher = "Фирма \"1С\"",
                    VersionString = "",
                    Edition = InfoBaseConfigEdition.PROF
                };
            }
        }
    }
}
