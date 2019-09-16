using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utility
{
    public static class AssemblyTypeLoader
    {
        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <returns>The types.</returns>
        /// <param name="assembly">Assembly.</param>
        public static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            return GetTypes(assembly, out string message);
        }

        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <returns>The types.</returns>
        /// <param name="assembly">Assembly.</param>
        /// <param name="message">Message.</param>
        public static IEnumerable<Type> GetTypes(Assembly assembly, out string message)
        {
            IEnumerable<Type> result = Enumerable.Empty<Type>();
            message = string.Empty;

            try
            {
                result = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder builder = new StringBuilder();
                if (ex.Types.Any())
                {
                    ex.Types.Where(type => type != null).ToList().ForEach(type => builder.AppendFormat("Load type: \"{0}\" fail. ", type.FullName));
                }

                if (ex.LoaderExceptions.Any())
                {
                    ex.LoaderExceptions.Where(x => x != null).ForEach(x => builder.AppendFormat("Load exception: \"{0}\". ", x.Message));
                }

                message = builder.ToString();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Get filted types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <param name="filter">Filter function.</param>
        /// <returns>Type collection.</returns>
        public static IEnumerable<Type> GetTypes(Assembly assembly, Func<Type, bool> filter)
        {
            return GetTypes(assembly, filter, out string message);
        }

        /// <summary>
        /// Get filted types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <param name="filter">Filter function.</param>
        /// <param name="message">Process message.</param>
        /// <returns>Type collection.</returns>
        public static IEnumerable<Type> GetTypes(Assembly assembly, Func<Type, bool> filter, out string message)
        {
            IEnumerable<Type> result = new List<Type>();
            message = string.Empty;

            try
            {
                result = assembly.GetTypes().Where(type => filter(type)).ToArray();
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder builder = new StringBuilder();
                if (ex.Types.Any())
                {
                    ex.Types.Where(type => type != null).ForEach(type => builder.AppendFormat("Load type: \"{0}\" fail. ", type.FullName));
                }

                if (ex.LoaderExceptions.Any())
                {
                    ex.LoaderExceptions.Where(x => x != null).ForEach(x => builder.AppendFormat("Load exception: \"{0}\". ", x.Message));
                }

                message = builder.ToString();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return result;
        }
    }
}
