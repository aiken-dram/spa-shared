using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using Shared.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Shared.Application.Extensions;

/// <summary>
/// Static class for working with expressions
/// </summary>
public static partial class FilterExtension
{
    /// <summary>
    /// Building dynamic predicate from list of filters
    /// </summary>
    /// <param name="filters">list of filters</param>
    /// <typeparam name="T">type of entity</typeparam>
    /// <returns>predicate expression</returns>
    public static Expression<Func<T, bool>> BuildPredicates<T>(this IEnumerable<TableFilter> filters)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        Expression? exp = null;

        foreach (var filter in filters)
        {
            Expression body;
            var propertyName = filter.Field;
            var comparison = filter.Operator;
            var value = filter.Value;

            if (comparison.StartsWith("group")) //will implement [not] later //OR NEVAH mwhah
            {
                var propertyNames = propertyName.Split(',');
                var val = value.Split(',');

                //first value
                int id = Convert.ToInt32(val[0]);
                var left = propertyNames[id].Split('.').Aggregate((Expression)parameter, Expression.Property);
                body = MakeComparison(left, "==", "1");

                if (val.Length > 1)
                {
                    foreach (var v in val.Skip(1))
                    {
                        id = Convert.ToInt32(v);
                        left = propertyNames[id].Split('.').Aggregate((Expression)parameter, Expression.Property);
                        //WHAT THE ACTUAL F Expression.Or throws error in db2, while Expression.OrElse works fine
                        body = Expression.OrElse(body, MakeComparison(left, "==", "1"));
                    }
                }
            }
            else
            {
                var left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
                body = MakeComparison(left, comparison, value);
            }

            exp = exp == null ? body : Expression.And(exp, body);
        }

        //2D - replace it with some simple true=true if filters were empty?
        if (exp == null)
            throw new Exception("Filters were empty, could not construct expression");

        return Expression.Lambda<Func<T, bool>>(exp, parameter);
    }

    private static Expression MakeComparison(Expression left, string comparison, string value)
    {
        switch (comparison)
        {
            case "==":
                return MakeEqual(left, value);
            case "!=":
                return MakeNotEqual(left, value);
            case ">":
                return MakeBinary(ExpressionType.GreaterThan, left, value);
            case ">=":
                return MakeBinary(ExpressionType.GreaterThanOrEqual, left, value);
            case "<":
                return MakeBinary(ExpressionType.LessThan, left, value);
            case "<=":
                return MakeBinary(ExpressionType.LessThanOrEqual, left, value);
            case "Contains":
            case "StartsWith":
            case "EndsWith":
                return Expression.Call(MakeString(left), comparison, Type.EmptyTypes, Expression.Constant(value, typeof(string)));
            case "like":
                var efLikeMethod = typeof(DbFunctionsExtensions).GetMethod("Like",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new[] { typeof(DbFunctions), typeof(string), typeof(string) },
                    null);
                if (efLikeMethod == null)
                    throw new Exception("Could not find 'Like' method in DbFunctionsExtensions");
                var pattern = Expression.Constant(value, typeof(string));
                return Expression.Call(efLikeMethod,
                Expression.Property(null, typeof(EF), nameof(EF.Functions)), left, pattern);
            case "date":
                if (!string.IsNullOrEmpty(value))
                {
                    CultureInfo ru = new CultureInfo(Messages.CultureInfo);
                    var dates = value.Split(" ~ ");
                    if (dates.Length == 1)
                    {
                        var date_val = DateTime.Parse(dates[0], ru);
                        return MakeBinary(ExpressionType.Equal, left, date_val.ToString());
                    }
                    else if (dates.Length == 2)
                    {
                        var dates_val = dates.Select(p => DateTime.Parse(p, ru));
                        var date_val_from = dates_val.Min();
                        var date_val_to = dates_val.Max();
                        var expLeft = MakeBinary(ExpressionType.GreaterThanOrEqual, left, date_val_from.ToString());
                        var expRight = MakeBinary(ExpressionType.LessThanOrEqual, left, date_val_to.ToString());
                        return Expression.And(expLeft, expRight);
                    }
                }
                return Expression.Constant(true); //wont work i think it woooont
            case "stamp":
                if (!string.IsNullOrEmpty(value))
                {
                    CultureInfo ru = new CultureInfo(Messages.CultureInfo);
                    var dates = value.Split(" ~ ");
                    if (dates.Length == 1)
                    {
                        var date_val_from = DateTime.Parse(dates[0], ru);
                        var date_val_to = date_val_from.AddDays(1);
                        var expLeft = MakeBinary(ExpressionType.GreaterThanOrEqual, left, date_val_from.ToString());
                        var expRight = MakeBinary(ExpressionType.LessThan, left, date_val_to.ToString());
                        return Expression.And(expLeft, expRight);
                    }
                    else if (dates.Length == 2)
                    {
                        var dates_val = dates.Select(p => DateTime.Parse(p, ru));
                        var date_val_from = dates_val.Min();
                        var date_val_to = dates_val.Max().AddDays(1);
                        var expLeft = MakeBinary(ExpressionType.GreaterThanOrEqual, left, date_val_from.ToString());
                        var expRight = MakeBinary(ExpressionType.LessThan, left, date_val_to.ToString());
                        return Expression.And(expLeft, expRight);
                    }
                }
                return Expression.Constant(true); //wont work i think it woooont

            case "in":
                if (value != "null")
                {
                    var values = value.Split(',').ToList();
                    var methodInfo = typeof(List<string>).GetMethod("Contains", new Type[] { typeof(string) });

                    if (methodInfo == null)
                        throw new Exception("Could not find 'Contains' method in List of string");

                    var list = Expression.Constant(values);
                    return Expression.Call(list, methodInfo, left);
                }
                return Expression.Constant(true);
            default:
                throw new NotSupportedException(Messages.InvalidComparisonOperator(comparison));
        }
    }

    private static Expression MakeString(Expression source)
    {
        return source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
    }

    private static Expression MakeEqual(Expression left, string value)
    {
        var type = left.Type;
        var valueType = Nullable.GetUnderlyingType(type) ?? type;
        var c = MakeConstant(value, type);
        var numbers = new string[] { "Int16", "Int32", "Int64" };
        return Expression.Equal(left, c);
    }

    private static Expression MakeNotEqual(Expression left, string value)
    {
        var type = left.Type;
        var valueType = Nullable.GetUnderlyingType(type) ?? type;
        var c = MakeConstant(value, type);
        var numbers = new string[] { "Int16", "Int32", "Int64", "Decimal" };
        return Expression.NotEqual(left, c);
    }

    private static Expression MakeConstant(string value, Type type)
    {
        var valueType = Nullable.GetUnderlyingType(type) ?? type;
        switch (valueType.Name)
        {
            case "Boolean":
                return Expression.Constant(value == "true", type);
            case "Int16":
                return Expression.Constant(Convert.ToInt16(value), type);
            case "Int32":
                return Expression.Constant(Convert.ToInt32(value), type);
            case "Int64":
                return Expression.Constant(Convert.ToInt64(value), type);
            case "Decimal":
                return Expression.Constant(Convert.ToDecimal(value), type);
            default:
                return Expression.Constant(value);
        }
    }

    private static Expression MakeBinary(ExpressionType type, Expression left, string value)
    {
        object? typedValue = value;
        if (left.Type != typeof(string))
        {
            if (string.IsNullOrEmpty(value))
            {
                typedValue = null;
                if (Nullable.GetUnderlyingType(left.Type) == null)
                    left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
            }
            else
            {
                var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
                typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) :
                    valueType == typeof(Guid) ? Guid.Parse(value) :
                    Convert.ChangeType(value, valueType);
            }
        }
        var right = Expression.Constant(typedValue, left.Type);
        return Expression.MakeBinary(type, left, right);
    }
}