using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Utility
{
    public static class AssemblyLoader
    {
        private static readonly ConcurrentDictionary<string, Assembly> dicAssemblies = new ConcurrentDictionary<string, Assembly>();

        /// <summary>
        /// Initializes the AssemblyLoader.
        /// </summary>
        static AssemblyLoader()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.GetAssemblies().ForEach(TryAdd);
            Initialize(new DirectoryInfo(currentDomain.BaseDirectory));

            DependencyContext.Default?.CompileLibraries?.ForEach(lib =>
            {
                var paths = new List<string>();
                try
                {
                    paths = lib.ResolveReferencePaths().ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                if (paths.Count > 0)
                {
                    foreach (var path in paths)
                    {
                        Initialize(new FileInfo(path));
                    }
                }

            });
        }

        /// <summary>
        /// Load the specified assemblyName.
        /// </summary>
        /// <returns>The load.</returns>
        /// <param name="assemblyName">Assembly name.</param>
        public static Assembly Load(AssemblyName assemblyName)
        {
            if (dicAssemblies.TryGetValue(assemblyName.Name, out Assembly result))
            {
                return result;
            }

            return default(Assembly);

        }

        /// <summary>
        /// Loads all.
        /// </summary>
        /// <returns>The all.</returns>
        public static IEnumerable<Assembly> LoadAll()
        {
            return dicAssemblies.Values.ToArray();
        }

        /// <summary>
        /// Initialize the specified directoryInfo.
        /// </summary>
        /// <returns>The initialize.</returns>
        /// <param name="directoryInfo">Directory info.</param>
        private static void Initialize(DirectoryInfo directoryInfo)
        {
            var assembliesDll = directoryInfo.GetFiles("*.dll");
            if (assembliesDll.Any())
            {
                assembliesDll.ForEach(Initialize);
            }
        }

        /// <summary>
        /// Initialize the specified fileInfo.
        /// </summary>
        /// <returns>The initialize.</returns>
        /// <param name="fileInfo">File info.</param>
        private static void Initialize(FileInfo fileInfo)
        {
            try
            {
                TryAdd(Assembly.LoadFrom(fileInfo.FullName));
            }
            catch (Exception e)
            {
                //todo log.debug(e)
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Tries the add.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        private static void TryAdd(Assembly assembly)
        {
            dicAssemblies.TryAdd(assembly.FullName, assembly);
        }
    }
}
