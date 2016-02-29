using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace ETB
{
    public static class TypeHelper
    {
        public static Func<T, IDictionary<string, string>> CreateMapFunc<T>()
        {
            return CreateMapFunc<T>((_, __) => __.ToString());
        }
        public static Func<T, IDictionary<string, string>> CreateMapFunc<T>(Func<string, object, string> stringifier)
        {
            var mapper = CreateObjMapFunc<T>();
            return t =>
            {
                return mapper(t).ToDictionary(k => k.Key, v => stringifier(v.Key, v.Value));
            };
        }

        public static Func<T, IDictionary<string, object>> CreateObjMapFunc<T>()
        {
            var typ = typeof(T);
            var props = typ.GetProperties();
            var paramExp = Expression.Parameter(typ, "target");
            var getters = props.Where(p => p.PropertyType.IsPrimitive || p.PropertyType == typeof(string) || p.PropertyType == typeof(DateTime)).ToDictionary(prop => prop.Name, prop =>
            {
                var getter = prop.GetGetMethod();
                var getterCallExp = Expression.Call(paramExp, getter);
                var castExp = Expression.Convert(getterCallExp, typeof(object));
                return Expression.Lambda<Func<T, object>>(castExp, paramExp).Compile();
            }).ToArray();

            return t =>
            {
                var dic = new Dictionary<string, object>();
                foreach (var g in getters)
                {
                    dic[g.Key] = g.Value(t);
                }
                return dic;
            };
        }

    }
}
