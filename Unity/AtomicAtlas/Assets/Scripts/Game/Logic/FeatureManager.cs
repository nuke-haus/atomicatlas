using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

public interface IFeatureManager
{
    public FeatureType GetFeatureDefinition<FeatureType>() where FeatureType : FeatureDefinition;
    public DefinitionType GetDefinitionFromFeatureDefinition<FeatureType, DefinitionType, DataType>(string definitionId) where FeatureType : FeatureDefinition where DefinitionType : Definition<DataType>;
}

[Injectable(typeof(IFeatureManager))]
public class FeatureManager : IFeatureManager
{
    private List<FeatureDefinition> gameFeatureDefinitions;

    public FeatureManager()
    {
        LoadFromJson();
    }

    public FeatureType GetFeatureDefinition<FeatureType>() where FeatureType : FeatureDefinition
    {
        return gameFeatureDefinitions.FirstOrDefault<FeatureDefinition>(def => def is FeatureType) as FeatureType;
    }

    public DefinitionType GetDefinitionFromFeatureDefinition<FeatureType, DefinitionType, DataType>(string definitionId) where FeatureType : FeatureDefinition where DefinitionType : Definition<DataType>
    {
        var feature = GetFeatureDefinition<FeatureType>();
        Assert.IsNotNull(feature, "Unable to find feature definition " + typeof(FeatureType).Name);
    
        foreach (var fieldInfo in feature.GetType().GetFields())
        {
            if (fieldInfo.FieldType == typeof(List<DefinitionType>))
            {
                var definitionList = (List<DefinitionType>) fieldInfo.GetValue(feature);
                return definitionList.FirstOrDefault(definition => definition.ID == definitionId);
            }
        }

        throw new Exception(String.Format("Unable to find definition list for {0} in {1}", typeof(DefinitionType).Name, typeof(FeatureType).Name));
    }

    private void LoadFromJson()
    {
        gameFeatureDefinitions = new List<FeatureDefinition>();

        string dataPath = Application.dataPath + "/Json";
        string[] filePaths = Directory.GetFiles(dataPath, "*.json");
        for (int i = 0; i < filePaths.Length; i++)
        {
            filePaths[i] = filePaths[i].Replace("\\", "/");
        }

        foreach (string filePath in filePaths)
        {
            string[] splitPath = filePath.Split("/");
            string typeName = splitPath[splitPath.Length - 1].Replace(".json", string.Empty);
            Type classType = Type.GetType(typeName);

            Assert.IsNotNull(classType, "Unable to find feature definition: " + typeName);

            using (StreamReader r = new StreamReader(filePath))
            {
                string data = r.ReadToEnd();
                var feature = Activator.CreateInstance(classType);
                JsonUtility.FromJsonOverwrite(data, feature);

                gameFeatureDefinitions.Add(feature as FeatureDefinition);
            }
        }
    }

    // Old stuff, don't use any of this
    private void SerializeOld()
    {
        foreach (FeatureDefinition feature in gameFeatureDefinitions)
        {
            Type classType = feature.GetType();
            string serialized = feature.Serialize();
            Debug.Log(serialized);

            PlayerPrefs.SetString(classType.Name, serialized);
        }
    }

    private void DeserializeOld()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(FeatureDefinition)));
        gameFeatureDefinitions = new List<FeatureDefinition>();

        foreach (Type classType in types)
        {
            string data = PlayerPrefs.GetString(classType.Name);
            var ctors = classType.GetConstructors(BindingFlags.Public);
            FeatureDefinition feature = (FeatureDefinition)ctors[0].Invoke(new object[] { });
            JsonUtility.FromJsonOverwrite(data, feature);

            gameFeatureDefinitions.Add(feature);
        }
    }

    private void LoadOld()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(FeatureDefinition)));
        gameFeatureDefinitions = new List<FeatureDefinition>();

        foreach (Type classType in types)
        {
            string data = PlayerPrefs.GetString(classType.Name);
            var ctors = classType.GetConstructors(BindingFlags.Public);
            FeatureDefinition feature = (FeatureDefinition)ctors[0].Invoke(new object[] { });
            JsonUtility.FromJsonOverwrite(data, feature);

            gameFeatureDefinitions.Add(feature);
        }
    }
}
