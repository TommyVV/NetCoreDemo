using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DependencyExtension
{
    public static class AssemblyLoader
    {
        private static readonly ConcurrentDictionary<string, Assembly> DicAssemblies =
            new ConcurrentDictionary<string, Assembly>();

        /// <summary>
        /// Initializes the AssemblyLoader.
        /// </summary>
        static AssemblyLoader()
        {
            var currentDomain = AppDomain.CurrentDomain;
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
        /// Loads all.
        /// </summary>
        /// <returns>The all.</returns>
        public static IEnumerable<Assembly> LoadAll()
        {
            return DicAssemblies.Values.ToArray();
        }

        /// <summary>
        /// Get and filter types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <param name="filter">Filter function.</param>
        /// <returns>Type collection.</returns>
        public static IEnumerable<Type> GetTypes(Assembly assembly, Func<Type, bool> filter)
        {
            List<Type> result;

            try
            {
                result = assembly.GetTypes().Where(filter).ToList();
            }
            catch (ReflectionTypeLoadException ex)
            {
                var builder = new StringBuilder();
                if (ex.Types.Any())
                {
                    ex.Types.Where(type => type != null).ForEach(type =>
                        builder.AppendFormat("Load type: \"{0}\" fail. ", type.FullName));
                }

                if (ex.LoaderExceptions.Any())
                {
                    ex.LoaderExceptions.Where(x => x != null)
                        .ForEach(x => builder.AppendFormat("Load exception: \"{0}\". ", x.Message));
                }

                var message = builder.ToString();
                throw new Exception(message);
            }

            return result;
        }

        /// <summary>
        /// Initialize the specified directoryInfo.
        /// </summary>
        /// <returns>The initialize.</returns>
        /// <param name="directoryInfo">Directory info.</param>
        private static void Initialize(DirectoryInfo directoryInfo)
        {
            var assembliesDll = directoryInfo.GetFiles("Domain*.dll");
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
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Tries the add.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        private static void TryAdd(Assembly assembly)
        {
            DicAssemblies.TryAdd(assembly.FullName, assembly);
        }
    }
}