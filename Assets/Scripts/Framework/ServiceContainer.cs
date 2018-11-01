using System;
using System.Collections.Generic;
using System.Linq;

namespace TextSpace.Framework.IoC
{
    public static class ServiceContainer
    {
        private static readonly List<object> Dependencies = new List<object>();
        private static readonly List<Type> RegisteredTypes = new List<Type>();
        private static bool _constructed = false;

        public static void AddDependency(object obj)
        {
            if (_constructed)
                throw new Exception("ServiceContainer cannot add a dependency after being constructed");

            if (!Dependencies.Contains(obj))
                Dependencies.Add(obj);
        }

        public static void Construct()
        {
            if (_constructed)
                throw new Exception("ServiceContainer already constructed");

            var registeredTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(IResolvableService).IsAssignableFrom(x) && x.GetConstructors().Any())
                            .Select(x => x);
            RegisteredTypes.AddRange(registeredTypes);

            while (RegisteredTypes.Any())
            {
                var builtDependency = false;
                var typesToBuild = new List<Type>(RegisteredTypes);
                foreach (var regType in typesToBuild)
                {
                    var dependenciesAvailable = true;
                    var ctor = regType.GetConstructors()[0];
                    var ctorArgs = new List<object>();

                    foreach (var param in ctor.GetParameters())
                    {
                        var paramType = param.ParameterType;
                        var dep = Dependencies.FirstOrDefault(d => d.GetType() == paramType || paramType.IsInstanceOfType(d));

                        if (dep == null)    //  couldn't find a dependency, break out of the loop and try the next object
                        {
                            dependenciesAvailable = false;
                            break;
                        }
                        ctorArgs.Add(dep);
                    }

                    if (!dependenciesAvailable)
                        continue;

                    var resolvedObject = ctor.Invoke(ctorArgs.ToArray());
                    Dependencies.Add(resolvedObject);
                    RegisteredTypes.Remove(regType);
                    builtDependency = true;
                }

                if (!builtDependency)
                    throw new Exception("ServiceContainer construction is missing dependencies");
            }
            _constructed = true;
        }

        public static T Resolve<T>() where T : IResolvableService
        {
            return (T)Dependencies.First(d => d is T);
        }
    }
}
