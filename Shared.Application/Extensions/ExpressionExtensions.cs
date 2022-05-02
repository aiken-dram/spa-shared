using System.Linq.Expressions;
using System.Reflection;

namespace Shared.Application.Extensions;

public static class ExpressionExtensions
{
    /// <summary>
    /// Returns PropertyInfo from expression
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="property">Property expression</param>
    /// <returns>PropertyInfo</returns>
    public static PropertyInfo GetPropertyInfo<TEntity, TProperty>(
        this Expression<Func<TEntity, TProperty>> property
    )
    {
        LambdaExpression lambda = property;
        var memberExpression = lambda.Body is UnaryExpression expression
            ? (MemberExpression)expression.Operand
            : (MemberExpression)lambda.Body;

        return (PropertyInfo)memberExpression.Member;
    }
}
