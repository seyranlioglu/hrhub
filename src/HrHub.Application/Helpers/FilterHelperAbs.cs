using LinqKit;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Reflection;

namespace HrHub.Application.Helpers
{
    public static class FilterHelper<T> where T : class
    {
        public static List<Attributes> GetAttributeFromRequest(T request)
        {
            List<Attributes> attributeList = new List<Attributes>();
            foreach (PropertyInfo propertyInfo in request.GetType().GetProperties())
            {
                object value = propertyInfo.GetValue(request);
                if (value != null && value != "" && value.ToString() != "0" )
                {
                    attributeList.Add(new Attributes { Name = propertyInfo.Name, Value = value, Type = ExpressionType.Equal });
                }
            }
            return attributeList;
        }
        public static Expression<Func<T, bool>> GeneratePredicate(List<Attributes> liste)
        {

            var predicate = PredicateBuilder.True<T>();
            foreach (var keyword in liste)
            {
                var type = typeof(T);
                var param = Expression.Parameter(type, "p");
                var property = Expression.Property(param, keyword.Name);
                var value = Expression.Constant(keyword.Value);
                Expression body;

                if ((property.Type == typeof(bool) || property.Type == typeof(Nullable<bool>)) && keyword.Type == ExpressionType.Equal)
                {
                    // Nullable boolean özellik için özel durum
                    if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var hasValue = Expression.Property(property, "HasValue");
                        var condition = Expression.Condition(
                            hasValue,
                            Expression.Equal(Expression.Property(property, "Value"), value),
                            Expression.Constant(false)
                        );
                        body = condition;
                    }
                    else
                    {
                        // Nullable değilse doğrudan eşitlik kontrolü yapabiliriz
                        body = Expression.Equal(property, value);
                    }
                }
                else if ((property.Type == typeof(bool) || property.Type == typeof(Nullable<bool>)) && keyword.Type == ExpressionType.NotEqual)
                {
                    // Nullable boolean özellik için özel durum
                    if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var hasValue = Expression.Property(property, "HasValue");
                        var condition = Expression.Condition(
                            hasValue,
                            Expression.NotEqual(Expression.Property(property, "Value"), value),
                            Expression.Constant(true)
                        );
                        body = condition;
                    }
                    else
                    {
                        // Nullable değilse doğrudan eşitlik kontrolü yapabiliriz
                        body = Expression.NotEqual(property, value);
                    }
                }

                else if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>) && keyword.Type == ExpressionType.Equal)
                {
                    if (value.Type.IsGenericType && value.Type.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var hasValue = Expression.Property(property, "HasValue");
                        var valueList = value.GetType().GetProperty("Value").GetValue(value, null);

                        var inListExpression = Expression.Condition(
                            hasValue,
                            Expression.Call(
                                typeof(Enumerable),
                                "Contains",
                                new[] { property.Type.GetGenericArguments()[0] }, // Get the underlying type of Nullable
                                Expression.Constant(valueList),
                                Expression.Property(property, "Value")
                            ),
                            Expression.Constant(false)
                        );

                        body = inListExpression;
                    }
                    else
                    {
                        if ((property.Type != typeof(bool) && property.Type != typeof(Nullable<bool>)))
                        {
                            var hasValue = Expression.Property(property, "HasValue");
                            var condition = Expression.Condition(
                                hasValue,
                                Expression.Equal(Expression.Property(property, "Value"), value),
                                Expression.Constant(false)
                            );
                            body = condition;
                        }
                        else
                        {
                            body = Expression.Equal(property, value);
                        }
                    }
                }
                else if ((property.Type != typeof(bool) || property.Type.GetGenericTypeDefinition() == typeof(Nullable<>)) && keyword.Type == ExpressionType.Equal)
                {
                    // Nullable boolean özellik için özel durum
                    if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var hasValue = Expression.Property(property, "HasValue");
                        var condition = Expression.Condition(
                            hasValue,
                            Expression.Equal(Expression.Property(property, "Value"), value),
                            Expression.Constant(true)
                        );
                        body = condition;
                    }
                    else
                    {
                        if (value.Type.IsGenericType && value.Type.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            var containsMethod = typeof(List<>).MakeGenericType(property.Type).GetMethod("Contains");
                            body = Expression.Call(value, containsMethod, property);
                        }
                        // Nullable değilse doğrudan eşitlik kontrolü yapabiliriz
                        else
                            body = Expression.Equal(property, value);
                    }
                }
                else
                {
                    // parametre value isList=true
                    if (value.Type.IsGenericType && value.Type.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var containsMethod = typeof(List<>).MakeGenericType(property.Type).GetMethod("Contains");
                        body = Expression.Call(value, containsMethod, property);
                    }
                    else if (property.Type == typeof(DateTime?))
                    {
                        // Handle nullable DateTime comparison
                        var hasValue = Expression.Property(property, "HasValue");
                        var condition = Expression.Condition(
                            hasValue,
                            Expression.MakeBinary(keyword.Type, Expression.Property(property, "Value"), value),
                            Expression.Constant(false)
                        );
                        body = condition;
                    }
                    else
                        body = Expression.MakeBinary(keyword.Type, property, value);
                }

                var lambda = Expression.Lambda<Func<T, bool>>(body, param);
                predicate = predicate.And(lambda);
            }
            return predicate;
        }

    }
    public abstract class FilterHelperAbs<T>
    {
        public abstract Expression<Func<T, bool>> GeneratePredicate(List<Attributes> attributes);
        public abstract Func<IQueryable<T>, IIncludableQueryable<T, object>> GenerateIncludeFunction<T>(params Expression<Func<T, object>>[] includes);
    }
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }


    }
    public class Attributes
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public ExpressionType Type { get; set; }
    }
    public class ConcreteFilterHelper<T> : FilterHelperAbs<T>
    {
        public override Expression<Func<T, bool>> GeneratePredicate(List<Attributes> liste)
        {
            var predicate = PredicateBuilder.True<T>();
            foreach (var keyword in liste)
            {
                var type = typeof(T);
                var param = Expression.Parameter(type, "p");
                var property = Expression.Property(param, keyword.Name);
                var value = Expression.Constant(keyword.Value);
                Expression body;

                if ((property.Type == typeof(bool) || property.Type == typeof(Nullable<bool>)) && keyword.Type == ExpressionType.Equal)
                {
                    // Nullable boolean özellik için özel durum
                    if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var hasValue = Expression.Property(property, "HasValue");
                        var condition = Expression.Condition(
                            hasValue,
                            Expression.Equal(Expression.Property(property, "Value"), value),
                            Expression.Constant(false)
                        );
                        body = condition;
                    }
                    else
                    {
                        // Nullable değilse doğrudan eşitlik kontrolü yapabiliriz
                        body = Expression.Equal(property, value);
                    }
                }
                else if ((property.Type == typeof(bool) || property.Type == typeof(Nullable<bool>)) && keyword.Type == ExpressionType.NotEqual)
                {
                    // Nullable boolean özellik için özel durum
                    if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var hasValue = Expression.Property(property, "HasValue");
                        var condition = Expression.Condition(
                            hasValue,
                            Expression.NotEqual(Expression.Property(property, "Value"), value),
                            Expression.Constant(true)
                        );
                        body = condition;
                    }
                    else
                    {
                        // Nullable değilse doğrudan eşitlik kontrolü yapabiliriz
                        body = Expression.NotEqual(property, value);
                    }
                }
                else if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>) && keyword.Type == ExpressionType.Equal)
                {
                    if ((property.Type != typeof(bool) && property.Type != typeof(Nullable<bool>)))
                    {
                        var hasValue = Expression.Property(property, "HasValue");
                        var condition = Expression.Condition(
                            hasValue,
                            Expression.Equal(Expression.Property(property, "Value"), value),
                            Expression.Constant(false)
                        );
                        body = condition;
                    }
                    else
                    {
                        body = Expression.Equal(property, value);
                    }
                }

                else
                {
                    // parametre value isList=true
                    if (value.Type.IsGenericType && value.Type.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var containsMethod = typeof(List<>).MakeGenericType(property.Type).GetMethod("Contains");
                        body = Expression.Call(value, containsMethod, property);
                    }
                    // Diğer durumlar
                    else
                        body = Expression.MakeBinary(keyword.Type, property, value);
                }

                var lambda = Expression.Lambda<Func<T, bool>>(body, param);
                predicate = predicate.And(lambda);
            }
            return predicate;
        }
        public override Func<IQueryable<T>, IIncludableQueryable<T, object>> GenerateIncludeFunction<T>(
       params Expression<Func<T, object>>[] includes)
        {
            var parameter = Expression.Parameter(typeof(IQueryable<T>), "query");
            var queryable = includes.Aggregate(
                (Expression)parameter,
                (current, include) =>
                    Expression.Call(
                        typeof(EntityFrameworkQueryableExtensions),
                        "Include",
                        new[] { typeof(T) },
                        current,
                        include
                    )
            );

            var lambda = Expression.Lambda<Func<IQueryable<T>, IIncludableQueryable<T, object>>>(queryable, parameter);
            return lambda.Compile();
        }


    }
}
