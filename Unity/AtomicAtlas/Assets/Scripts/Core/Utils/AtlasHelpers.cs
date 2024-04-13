
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

namespace Atlas.Core
{
    public static class AtlasHelpers
    {
        private static readonly string ATLAS_PREFIX = "Atlas";

        public static IEnumerable<Assembly> GetAtlasAssemblies()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.IsDynamic)
                {
                    continue;
                }

                string assemblyName = assembly.GetName().Name;

                if (!assemblyName.Contains(ATLAS_PREFIX))
                {
                    continue;
                }

                yield return assembly;
            }
        }

        public static IEnumerable<Type> GetTypesWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            var assemblies = GetAtlasAssemblies();
            List<Type> allTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t => t.IsDefined(typeof(TAttribute)));
                allTypes.AddRange(types);
            }

            return allTypes;
        }
    }
}