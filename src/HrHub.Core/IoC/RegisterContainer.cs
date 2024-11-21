using HrHub.Abstraction.Attributes;
using HrHub.Abstraction.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.IoC
{
    public static class RegisterContainer
    {
        public static void RegisterImplementations<TBaseInterface>(this IServiceCollection services, string projectName)
        {
            var assembly = GetAssemblyByName(projectName);
            if (assembly == null)
            {
                throw new ArgumentException($"Could not find assembly with name '{projectName}'.");
            }

            var baseInterfaceType = typeof(TBaseInterface);
            var typesGeneral = assembly.GetTypes();

            var types = typesGeneral.Where(type => type.IsClass && !type.IsAbstract && type.GetInterfaces()
                                                                                           .Any(w => w.Name == baseInterfaceType.Name))
                                                                                           .ToList();
            //|| w.Name.Contains("Manager")

            foreach (var type in types)
            {
                var intName = type.Name;
                var implementedInterface = type.GetInterfaces().FirstOrDefault(i => i.Name.Contains(intName));
                if (implementedInterface != null)
                {
                    var lifeCircleAttribute = type.GetCustomAttribute<LifeCycleAttribute>();
                    LifeCycleTypes lifeCircleType = LifeCycleTypes.Scoped;
                    if (lifeCircleAttribute != null)
                        lifeCircleType = lifeCircleAttribute.LifeCycleTypes;

                    var serviceDescriptor = GetServiceDescriptor(type, implementedInterface, lifeCircleType);
                    if (serviceDescriptor != null)
                        services.Add(serviceDescriptor);
                }
            }
        }

        private static Assembly GetAssemblyByName(string name)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fileName = String.Format("{0}.dll", name);
            var assembly = Directory
                .GetFiles(path, fileName, SearchOption.AllDirectories)
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                .FirstOrDefault();
            return assembly;
            //return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == name);
        }

        private static ServiceDescriptor GetServiceDescriptor(Type implementationType, Type interfaceType, LifeCycleTypes lifeCircleType)
        {
            switch (lifeCircleType)
            {
                case LifeCycleTypes.Singleton:
                    return ServiceDescriptor.Singleton(interfaceType, implementationType);
                case LifeCycleTypes.Transient:
                    return ServiceDescriptor.Transient(interfaceType, implementationType);
                case LifeCycleTypes.Scoped:
                    return ServiceDescriptor.Scoped(interfaceType, implementationType);
                case LifeCycleTypes.NotRegister:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifeCircleType), lifeCircleType, null);
            }
        }
    }
}
