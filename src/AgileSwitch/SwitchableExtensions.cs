using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileSwitch
{
    public static class SwitchableExtensions
    {
        public static Switchable<T> Case<T>(this Switchable<T> source, T comparand, Action<T> then)
        {
            return source.Case(v => v == null ? comparand == null : v.Equals(comparand), then);
        }

        public static Switchable<T> Case<T>(this Switchable<T> source, Func<T, bool> when, Action<T> then)
        {
            if (!source.Broken)
            {
                source.AnyCasePassed = when(source.Value);

                if (source.AnyCasePassed)
                {
                    then(source.Value);
                }
            }
            return source;
        }

#if !NET40
        public static async Task<Switchable<T>> CaseAsync<T>(this Switchable<T> source, T comparand, Func<T, Task> then)
        {
            return await source.CaseAsync(v => v == null ? comparand == null : v.Equals(comparand), then);
        }

        public static async Task<Switchable<T>> CaseAsync<T>(this Switchable<T> source, Func<T, bool> when, Func<T, Task> then)
        {
            if (!source.Broken)
            {
                source.AnyCasePassed = when(source.Value);

                if (source.AnyCasePassed)
                {
                    await then(source.Value);
                }
            }
            return source;
        }

        public static async Task<Switchable<T>> CaseAsync<T>(this Switchable<T> source, Func<T, Task<bool>> when, Action<T> then)
        {
            if (!source.Broken)
            {
                source.AnyCasePassed = await when(source.Value);

                if (source.AnyCasePassed)
                {
                    then(source.Value);
                }
            }
            return source;
        }

        public static async Task<Switchable<T>> CaseAsync<T>(this Switchable<T> source, Func<T, Task<bool>> when, Func<T, Task> then)
        {
            if (!source.Broken)
            {
                source.AnyCasePassed = await when(source.Value);

                if (source.AnyCasePassed)
                {
                    await then(source.Value);
                }
            }
            return source;
        }
#endif
        public static Switchable<T> Break<T>(this Switchable<T> source)
        {
            if (source.AnyCasePassed)
            {
                source.Broken = true;
            }
            return source;
        }

        public static void Default<T>(this Switchable<T> source, Action<T> action)
        {
            if (!source.Broken)
            {
                action(source.Value);
            }
        }
#if !NET40
        public static async Task DefaultAsync<T>(this Switchable<T> source, Func<T, Task> action)
        {
            if (!source.Broken)
            {
                await action(source.Value);
            }
        }
#endif
    }
}
