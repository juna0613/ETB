using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETB.Fnc
{
    public enum MaybeType
    {
        Just,
        Nothing
    };

    public class Maybe<T>
    {
        protected readonly MaybeType _typ;
        public MaybeType Type { get { return _typ; } }
        public Exception Error { get; protected set; }

        internal Maybe(MaybeType typ)
        {
            _typ = typ;
        }

        public bool Take(out T value)
        {
            if(Type == MaybeType.Just)
            {
                value = ((Just<T>)this).Value;
                return true;
            }
            value = default(T);
            return false;
        }
    }

    public sealed class Nothing<T> : Maybe<T>
    {
        internal Nothing(Exception e = null) : base(MaybeType.Nothing)
        {
            Error = e;
        }
    }

    public sealed class Just<T> : Maybe<T>
    {
        private readonly T _val;
        internal Just(T value) : base(MaybeType.Just)
        {
            _val = value;
            Error = null;
        }

        public T Value { get { return _val; } }
    }

    public static class Maybe
    {
        public static Maybe<T> Nothing<T>(Exception e = null)
        {
            return new Nothing<T>(e);
        }
        public static Maybe<T> Just<T>(T value)
        {
            return new Just<T>(value);
        }
        public static Maybe<T> Return<T>(T value)
        {
            return Equals(value, default(T)) ? Nothing<T>() : Just<T>(value);
        }
        public static Maybe<T2> Bind<T1, T2>(Maybe<T1> value, Func<T1, Maybe<T2>> func)
        {
            T1 val;
            return value.Take(out val) ? func(val) : Nothing<T2>();
        }

        public static Maybe<T2> Map<T1, T2>(Maybe<T1> value, Func<T1, T2> func)
        {
            T1 val;
            if(value.Take(out val))
            {
                try
                {
                    return Just(func(val));
                }
                catch(Exception e)
                {
                    return Nothing<T2>(e);
                }
            }
            return Nothing<T2>(value.Error);
        }

        public static Maybe<Tuple<T1, T2>> Merge<T1, T2>(Maybe<T1> first, Maybe<T2> second)
        {
            T1 v1;
            T2 v2;
            if(first.Take(out v1) && second.Take(out v2))
            {
                return Just(Tuple.Create(v1, v2));
            }
            return new Nothing<Tuple<T1, T2>>(new MultiException(first.Error, second.Error));
        }
    }

    public static class MaybeExtensions
    {

        public static bool Take<T1, T2>(this Maybe<Tuple<T1, T2>> value, out T1 val1, out T2 val2)
        {
            Tuple<T1, T2> tup;
            if(value.Take(out tup))
            {
                val1 = tup.Item1;
                val2 = tup.Item2;
                return true;
            }
            val1 = default(T1);
            val2 = default(T2);
            return false;
        }

        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return Maybe.Return(value);
        }

        public static Maybe<T> ToJust<T>(this T value)
        {
            return Maybe.Just(value);
        }

        public static Maybe<T2> Bind<T1, T2>(this Maybe<T1> maybe, Func<T1, Maybe<T2>> func)
        {
            return Maybe.Bind(maybe, func);
        }

        public static Maybe<T2> Map<T1, T2>(this Maybe<T1> maybe, Func<T1, T2> func)
        {
            return Maybe.Map(maybe, func);
        }

        public static Maybe<TResult> Select<TSource, TResult>(
            this Maybe<TSource> maybe,
            Func<TSource, TResult> selector)
        {
            return Maybe.Map(maybe, selector);
        }

        public static Maybe<TResult> SelectMany<TSource, TValue, TResult>(
            this Maybe<TSource> maybe,
            Func<TSource, Maybe<TValue>> valueSelector,
            Func<TSource, TValue, TResult> resultSelector)
        {
            return maybe.Bind(
                source => valueSelector(source).Map(result => resultSelector(source, result)));
        }

        public static void Do<T>(this Maybe<T> maybe, Action<T> action)
        {
            T value;
            if (maybe.Take(out value))
            {
                action(value);
            }
        }

        public static void Do<T1, T2>(this Maybe<Tuple<T1, T2>> maybe, Action<T1, T2> action)
        {
            T1 value1;
            T2 value2;
            if (maybe.Take(out value1, out value2))
            {
                action(value1, value2);
            }
        }

        public static bool IsJust<T>(this Maybe<T> maybe)
        {
            return maybe.Type == MaybeType.Just;
        }

        public static bool IsNothing<T>(this Maybe<T> maybe)
        {
            return maybe.Type == MaybeType.Nothing;
        }

        public static T FromJust<T>(this Maybe<T> maybe)
        {
            T value;
            if (maybe.Take(out value))
            {
                return value;
            }
            return default(T);
        }

        public static T GetValueOrDefault<T>(this Maybe<T> maybe, T noneValue)
        {
            T value;
            return maybe.Take(out value) ? value : noneValue;
        }

        public static IEnumerable<T> ToEnumerable<T>(this Maybe<T> maybe)
        {
            T value;
            if (maybe.Take(out value))
            {
                return Enumerable.Empty<T>().Concat(new[] { value });
            }
            return Enumerable.Empty<T>();
        }

    }
}
