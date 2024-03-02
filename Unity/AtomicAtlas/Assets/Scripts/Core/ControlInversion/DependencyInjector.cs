using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine.Assertions;

public static class DependencyInjector
{
    private static Dictionary<Type, Type> dependencyDict;
    private static Dictionary<Type, object> resolvedObjectDict;
    private static bool isInitialized = false;

    public static void Initialize()
    {
        dependencyDict = new Dictionary<Type, Type>();
        resolvedObjectDict = new Dictionary<Type, object>();
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(InjectableAttribute)));

        foreach (Type classType in types)
        {
            var attribute = classType.GetAttribute<InjectableAttribute>();
            var injectionType = attribute.Type;

            dependencyDict.Add(injectionType, classType);
        }
    }

    public static T Resolve<T>()
    {
        if (!isInitialized)
        {
            Initialize();
        }

        Type type = typeof(T);
        object existingObject;
        resolvedObjectDict.TryGetValue(type, out existingObject);

        if (existingObject != null)
        {
            return (T) existingObject;
        }

        Assert.IsTrue(dependencyDict.ContainsKey(type), "Unable to resolve dependency for " + type.Name);

        Type classType;
        dependencyDict.TryGetValue(type, out classType);

        var ctors = classType.GetConstructors();
        T result = (T)ctors[0].Invoke(new object[] { });
        resolvedObjectDict.Add(type, result);

        return result;
    }
}