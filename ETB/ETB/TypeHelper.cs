using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
namespace ETB
{
    public static class TypeHelper
    {
        public static Func<T, IDictionary<string, string>> CreateMapFunc<T>(Func<object, string> stringifier)
        {
            var typ = typeof(T);
            var props = typ.GetProperties();
            var paramExp = Expression.Parameter(typ, "target");
            var getters = props.Where(p => p.PropertyType.IsPrimitive).ToDictionary(prop => prop.Name, prop =>
                {
                    var getter = prop.GetGetMethod();
                    var getterCallExp = Expression.Call(paramExp, getter);
                    return Expression.Lambda<Func<T, string>>(getterCallExp, paramExp).Compile();
                });

            return t =>
                {
                    var dic = new Dictionary<string, string>();
                    foreach (var g in getters)
                    {
                        dic[g.Key] = g.Value(t);
                    }
                    return dic;
                };
        }


    }
}
