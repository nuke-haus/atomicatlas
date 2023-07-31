using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

public interface IData
{
    public void Merge(IData data);
}

public interface IDataManager
{
    public T GetData<T>() where T : IData;
}

[Injectable(typeof(IDataManager))]
public class DataManager : IDataManager
{
    private List<IData> gameData;

    public DataManager()
    {
        LoadAllData();
    }

    public T GetData<T>() where T : IData
    {
        return (T) gameData.FirstOrDefault(data => data.GetType() == typeof(T));
    }

    private void LoadData<T>() where T : IData
    {
        var folderName = typeof(T).Name;
        var dataPath = Application.dataPath + "/Data/" + folderName;
        var filePaths = Directory.GetFiles(dataPath, "*.xml");
        var classType = typeof(T);

        for (int i = 0; i < filePaths.Length; i++)
        {
            //filePaths[i] = filePaths[i].Replace("\\", "/");
        }

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
                        result = (T) serializer.Deserialize(reader);
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

    private void LoadAllData()
    {
        gameData = new List<IData>();

        LoadData<NationData>();
        LoadData<WorldGenerationData>();
    }
}
