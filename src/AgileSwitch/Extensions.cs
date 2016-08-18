using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileSwitch.FlowControl;

namespace AgileSwitch
{
    public static class Extensions
    {
        public static IAfterWhen<T> When<T>(this IStart<T> source, T comparand)
        {
            return WhenImpl<IAfterWhen<T>, T>(source, v => v == null ? comparand == null : v.Equals(comparand));
        }

        public static IAfterWhen<T> When<T>(this IStart<T> source, Func<T, bool> predicate)
        {
            return WhenImpl<IAfterWhen<T>, T>(source, predicate);
        }

        public static IVoidAfterWhen<T> When<T>(this IVoidStart<T> source, T comparand)
        {
            return WhenImpl<IVoidAfterWhen<T>, T>(source, v => v == null ? comparand == null : v.Equals(comparand));
        }

        public static IVoidAfterWhen<T> When<T>(this IVoidStart<T> source, Func<T, bool> predicate)
        {
            return WhenImpl<IVoidAfterWhen<T>, T>(source, predicate);
        }

        public static IReturnAfterWhen<T, TReturn> When<T, TReturn>(this IReturnStart<T, TReturn> source, T comparand)
        {
            return WhenImpl<IReturnAfterWhen<T, TReturn>, T>(source, v => v == null ? comparand == null : v.Equals(comparand));
        }

        public static IReturnAfterWhen<T, TReturn> When<T, TReturn>(this IReturnStart<T, TReturn> source, Func<T, bool> predicate)
        {
            return WhenImpl<IReturnAfterWhen<T, TReturn>, T>(source, predicate);
        }

        static TInterface WhenImpl<TInterface, T>(object s, Func<T, bool> predicate) where TInterface : class
        {
            var source = s as SwitchObject<T>;
            source.PreviousCasePassed = !source.Broken && predicate(source.Value);
            return source as TInterface;
        }

        public static IAfterThen<T> Then<T>(this IVoidAfterWhen<T> source, Action<T> action)
        {
            var s = source as SwitchObject<T>;
            if (!s.Broken && s.PreviousCasePassed)
            {
                action(s.Value);
            }
            return s;
        }

        public static IReturnStart<T, TReturn> Return<T, TReturn>(this IAfterWhen<T> source, TReturn returnValue)
        {
            var s = source as SwitchObject<T>;
            return new ReturnSwitchObject<T, TReturn>
            {
                Value = s.Value,
                Broken = false,
                PreviousCasePassed = s.PreviousCasePassed,
                ReturnValue = s.PreviousCasePassed ? returnValue : default(TReturn)
            };
        }

        public static IReturnStart<T, TReturn> Return<T, TReturn>(this IAfterWhen<T> source, Func<T, TReturn> returnValueFactory)
        {
            var s = source as SwitchObject<T>;
            return new ReturnSwitchObject<T, TReturn>
            {
                Value = s.Value,
                Broken = false,
                PreviousCasePassed = s.PreviousCasePassed,
                ReturnValue = s.PreviousCasePassed ? returnValueFactory(s.Value) : default(TReturn)
            };
        }

        public static IReturnStart<T, TReturn> Return<T, TReturn>(this IReturnAfterWhen<T, TReturn> source, TReturn returnValue)
        {
            var s = source as ReturnSwitchObject<T, TReturn>;
            if (!s.Broken && s.PreviousCasePassed)
            {
                s.ReturnValue = returnValue;
                s.Broken = true;
            }
            return s;
        }

        public static IReturnStart<T, TReturn> Return<T, TReturn>(this IReturnAfterWhen<T, TReturn> source, Func<T, TReturn> returnValueFactory)
        {
            var s = source as ReturnSwitchObject<T, TReturn>;
            if (!s.Broken && s.PreviousCasePassed)
            {
                s.ReturnValue = returnValueFactory(s.Value);
                s.Broken = true;
            }
            return s;
        }

        public static IVoidStart<T> Break<T>(this IAfterThen<T> source)
        {
            var s = source as SwitchObject<T>;
            if (!s.Broken && s.PreviousCasePassed)
            {
                s.Broken = true;
            }
            return source;
        }

        public static void Default<T>(this IVoidStart<T> source, Action<T> action)
        {
            var s = source as SwitchObject<T>;
            if (!s.Broken)
            {
                action(s.Value);
            }
        }

        public static TReturn Default<T, TReturn>(this IReturnStart<T, TReturn> source, TReturn defaultValue)
        {
            var s = source as ReturnSwitchObject<T, TReturn>;
            return s.Broken ? s.ReturnValue : defaultValue;
        }

        public static TReturn Default<T, TReturn>(this IReturnStart<T, TReturn> source, Func<T, TReturn> defaultValueFactory)
        {
            var s = source as ReturnSwitchObject<T, TReturn>;
            return s.Broken ? s.ReturnValue : defaultValueFactory(s.Value);
        }
    }
}
