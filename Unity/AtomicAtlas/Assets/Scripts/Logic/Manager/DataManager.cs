using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Assertions;
using Atlas.Data;
using Atlas.Core;
using Atlas.WorldGen;

namespace Atlas.Logic
{
    public interface IDataManager
    {
        public IEnumerable<IData> AllData { get; }
        public IEnumerable<IStrategyData> AllStrategyData { get; }

        public T GetData<T>() where T : IData;
        public T GetStrategyData<T>() where T : IStrategyData;
    }

    [Injectable(typeof(IDataManager))]
    public class DataManager : IDataManager
    {
        public IEnumerable<IData> AllData => gameData;
        public IEnumerable<IStrategyData> AllStrategyData => strategyData;

        private List<IData> gameData;
        private List<IStrategyData> strategyData;

        public DataManager()
        {
            gameData = new List<IData>();
            strategyData = new List<IStrategyData>();

            LoadAllData();
        }

        public T GetData<T>() where T : IData
        {
            return (T)gameData.FirstOrDefault(data => data.GetType() == typeof(T));
        }

        public T GetStrategyData<T>() where T : IStrategyData
        {
            return (T)strategyData.FirstOrDefault(data => data.GetType() == typeof(T));
        }

        private void LoadData<T>() where T : IData
        {
            var folderName = typeof(T).Name;
            var dataPath = Application.dataPath + "/Data/" + folderName;
            var filePaths = Directory.GetFiles(dataPath, "*.xml");
            var classType = typeof(T);

            Assert.IsNotNull(classType, "Unable to find data: " + folderName);

            T result = default;

            foreach (string filePath in filePaths)
            {
                using (StreamReader r = new StreamReader(filePath))
                {
                    var data = r.ReadToEnd();
                    var serializer = new XmlSerializer(classType);

                    using (TextReader reader = new StringReader(data))
                    {
                        if (result == null)
                        {
                            result = (T)serializer.Deserialize(reader);
                        }
                        else
                        {
                            result.Merge((T)serializer.Deserialize(reader));
                        }
                    }
                }
            }
            gameData.Add(result);
        }

        private void LoadStrategyData()
        {
            var allTypes = AtlasHelpers.GetTypesWithAttribute<StrategyAttribute>();
            var baseDataPath = Application.dataPath + "/Data/Strategies/";

            foreach (Type classType in allTypes)
            {
                var attribute = classType.GetCustomAttribute<StrategyAttribute>();
                var dataType = attribute.DataClassType;
                var dataPath = baseDataPath + classType.Name;
                var filePaths = Directory.GetFiles(dataPath, "*.xml");
                IStrategyData result = null;

                foreach (string filePath in filePaths)
                {
                    using (StreamReader r = new StreamReader(filePath))
                    {
                        var data = r.ReadToEnd();
                        var serializer = new XmlSerializer(dataType);

                        using (TextReader reader = new StringReader(data))
                        {
                            if (result == null)
                            {
                                result = (IStrategyData)serializer.Deserialize(reader);
                            }
                            else
                            {
                                result.Merge((IStrategyData)serializer.Deserialize(reader));
                            }
                        }
                    }
                }
                strategyData.Add(result);
            }
        }

        private void LoadAllData()
        {
            LoadStrategyData();
            LoadData<NationData>();
        }
    } 
}
