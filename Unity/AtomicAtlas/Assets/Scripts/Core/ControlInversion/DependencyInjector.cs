using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.Assertions;

namespace Atlas.Core
{
    public static class DependencyInjector
    {
        private static Dictionary<Type, Type> dependencyDict;
        private static Dictionary<Type, object> resolvedObjectDict;
        private static bool isInitialized = false;

        public static void Initialize()
        {
            dependencyDict = new Dictionary<Type, Type>();
            resolvedObjectDict = new Dictionary<Type, object>();
            List<Type> allTypes = new List<Type>();

            foreach (var assembly in AtlasHelpers.GetAtlasAssemblies())
            {
                var types = assembly.GetTypes().Where(t => t.IsDefined(typeof(InjectableAttribute)));
                allTypes.AddRange(types);
            }

            foreach (Type classType in allTypes)
            {
                var attribute = classType.GetCustomAttribute<InjectableAttribute>(); //classType.GetAttribute<InjectableAttribute>();
                var injectionType = attribute.Type;

                dependencyDict.Add(injectionType, classType);
            }

            isInitialized = true;
        }

        public static T Resolve<T>()
        {
            if (!isInitialized)
            {
                Initialize();
            }

            Type type = typeof(T);
            Assert.IsTrue(type.IsInterface, "Cannot resolve type: " + type.Name);

            object existingObject;
            resolvedObjectDict.TryGetValue(type, out existingObject);

            if (existingObject != null)
            {
                return (T)existingObject;
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
}
