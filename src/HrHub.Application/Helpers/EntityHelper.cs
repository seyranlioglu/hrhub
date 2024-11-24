using HrHub.Abstraction.Contracts.Dtos.CommonDtos;
using HrHub.Abstraction.Enums;
using HrHub.Application.Managers.TypeManagers;
using HrHub.Domain.Contracts.Dtos.CommonDtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceStack.Web;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace HrHub.Application.Helpers
{
    public static class EntityHelper
    {
        public static Type? GetEntityType(string typeEntity)
        {
            string targetNamespace = TypeEntitiesNameSpace;
            Assembly targetAssembly = Assembly.Load("HrHub.Domain");
            var targetType = targetAssembly.GetTypes()
                .Where(type => type.Namespace == TypeEntitiesNameSpace && type.IsClass && type.Name == typeEntity).FirstOrDefault();
            return targetType;
        }
        public static Type? TypeDtoEntity(string typeEntitiesNameSpace, string typeDtoEntity)
        {
            Assembly targetAssembly = Assembly.Load("HrHub.Abstraction");
            var targetType = targetAssembly.GetTypes()
                .Where(type => type.Namespace == typeEntitiesNameSpace && type.IsClass && type.Name == typeDtoEntity).FirstOrDefault();
            return targetType;
        }
        private static string TypeEntitiesNameSpace = "HrHub.Domain.Entities.SqlDbEntities";
        public static dynamic CreateDynamicDtoFromJson(object data)
        {
            if (data is JObject jObject)
            {
                var dynamicDto = new ExpandoObject() as IDictionary<string, object>;

                foreach (var property in jObject.Properties())
                {
                    dynamicDto.Add(property.Name, property.Value);
                }

                return dynamicDto;
            }
            return data;
        }
        public static Dictionary<string, string> GetEntityDictionary(string typeEntity)
        {
            // Use reflection to get all public static fields of the TypeEntity class
            var validType = typeof(TypeEntity)
            .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            .Select(f => f.GetValue(null)?.ToString())
            .FirstOrDefault(s => s.Contains(typeEntity));

            return null;  // Return null if the typeEntity doesn't match any field name
        }
        public static async Task<object> ExecuteAddAsync(string typeEntity, object requestData, IServiceProvider _serviceProvider)
        {

            var validType = typeof(TypeEntity)
         .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
         .Select(f => f.GetValue(null)?.ToString())
         .FirstOrDefault(s => s.Contains(typeEntity));
    
            var entityType = GetEntityType(validType);
            if (entityType == null)
                return null;
            var data = JsonConvert.DeserializeObject(requestData.ToString(), typeof(CommonTypeEntityDto));
            var serviceType = typeof(CommonTypeBaseManager<>).MakeGenericType(entityType);
            var constructor = serviceType.GetConstructors().FirstOrDefault();
            var constructorParameters = constructor.GetParameters()
                                                   .Select(param => _serviceProvider.GetService(param.ParameterType))
                                                   .ToArray();
            var serviceInstance = Activator.CreateInstance(serviceType, constructorParameters);
            var method = serviceType.GetMethods()
                                    .FirstOrDefault(m => m.Name == "AddAsync" && m.IsGenericMethod);
            var genericMethod = method.MakeGenericMethod(typeof(CommonTypeEntityDto), typeof(CommonTypeEntityDto));
            var task = (Task)genericMethod.Invoke(serviceInstance, new[] { data });
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            var result = resultProperty?.GetValue(task);
            return result;
        }

        public static async Task<object> ExecuteGetAsync(string typeEntity, object requestData, IServiceProvider _serviceProvider)
        {
            try
            {

           

            var validType = typeof(TypeEntity)
          .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
          .Select(f => f.GetValue(null)?.ToString())
          .FirstOrDefault(s => s.Contains(typeEntity));

            var entityType = GetEntityType(validType);
            if (entityType == null)
                return null;
            var data = JsonConvert.DeserializeObject<CommonTypeEntityDto>(requestData.ToString());
            var serviceType = typeof(CommonTypeBaseManager<>).MakeGenericType(entityType);
            var constructor = serviceType.GetConstructors().FirstOrDefault();
            var constructorParameters = constructor.GetParameters()
                                                   .Select(param => _serviceProvider.GetService(param.ParameterType))
                                                   .ToArray();
            var serviceInstance = Activator.CreateInstance(serviceType, constructorParameters);
            var method = serviceType.GetMethods()
                                    .FirstOrDefault(m => m.Name == "Get" && m.IsGenericMethod);

            var attributes = FilterHelper<CommonTypeEntityDto>.GetAttributeFromRequest(data);
            Type filterHelperGenericType = typeof(FilterHelperAbs<>);
            Type genericFilterHelperType = filterHelperGenericType.MakeGenericType(entityType!);
            object filterHelperInstance = Activator.CreateInstance(typeof(ConcreteFilterHelper<>).MakeGenericType(entityType!))!;

            MethodInfo generatePredicateMethod = genericFilterHelperType.GetMethod("GeneratePredicate")!;
            object predicate = generatePredicateMethod.Invoke(filterHelperInstance, new object[] { attributes })!;
            var genericMethod = method.MakeGenericMethod(typeof(CommonTypeGetDto));
            var task = (Task)genericMethod.Invoke(serviceInstance, new[] { predicate });
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            var result = resultProperty?.GetValue(task);
            return result;
            }
            catch (Exception )
            {
                return null;
            }
        }

        public static async Task<object> ExecuteGetListAsync(string typeEntity, object requestData, IServiceProvider _serviceProvider)
        {


            var validType = typeof(TypeEntity)
         .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
         .Select(f => f.GetValue(null)?.ToString())
         .FirstOrDefault(s => s.Contains(typeEntity));

            var entityType = GetEntityType(validType);
            if (entityType == null)
                return null;
            var data = JsonConvert.DeserializeObject<CommonTypeEntityDto>(requestData.ToString());
            var serviceType = typeof(CommonTypeBaseManager<>).MakeGenericType(entityType);
            var constructor = serviceType.GetConstructors().FirstOrDefault();
            var constructorParameters = constructor.GetParameters()
                                                   .Select(param => _serviceProvider.GetService(param.ParameterType))
                                                   .ToArray();
            var serviceInstance = Activator.CreateInstance(serviceType, constructorParameters);
            var method = serviceType.GetMethods()
                                    .FirstOrDefault(m => m.Name == "GetList" && m.IsGenericMethod);

            var attributes = FilterHelper<CommonTypeEntityDto>.GetAttributeFromRequest(data);
            Type filterHelperGenericType = typeof(FilterHelperAbs<>);
            Type genericFilterHelperType = filterHelperGenericType.MakeGenericType(entityType!);
            object filterHelperInstance = Activator.CreateInstance(typeof(ConcreteFilterHelper<>).MakeGenericType(entityType!))!;

            MethodInfo generatePredicateMethod = genericFilterHelperType.GetMethod("GeneratePredicate")!;
            object predicate = generatePredicateMethod.Invoke(filterHelperInstance, new object[] { attributes })!;
            var genericMethod = method.MakeGenericMethod(typeof(CommonTypeGetDto));
            var task = (Task)genericMethod.Invoke(serviceInstance, new[] { predicate });
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            var result = resultProperty?.GetValue(task);
            return result;
        }
        private static object MapEntityToJson(object entity, object originalRequestData)
        {
            if (originalRequestData is JObject jObject)
            {
                var mappedObject = new ExpandoObject() as IDictionary<string, object>;
                foreach (var property in jObject.Properties())
                {
                    var propertyValue = entity.GetType().GetProperty(property.Name)?.GetValue(entity);
                    mappedObject[property.Name] = propertyValue ?? property.Value;
                }
                if (!mappedObject.ContainsKey("Id"))
                {
                    var entityId = entity.GetType().GetProperty("Id")?.GetValue(entity);
                    if (entityId != null)
                    {
                        mappedObject["Id"] = entityId;
                    }
                }
                return mappedObject;
            }

            return entity;
        }
    }
}
