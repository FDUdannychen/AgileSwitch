#if !NET40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileSwitch
{
    public static class TaskSwitchableExtensions
    {
        public static async Task<Switchable<T>> Case<T>(this Task<Switchable<T>> task, Func<T, bool> when, Action<T> then)
        {
            var source = await task;
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

        public static async Task<Switchable<T>> Case<T>(this Task<Switchable<T>> task, T comparand, Action<T> then)
        {
            return await task.Case(v => v == null ? comparand == null : v.Equals(comparand), then);
        }

        public static async Task<Switchable<T>> CaseAsync<T>(this Task<Switchable<T>> task, Func<T, bool> when, Func<T, Task> then)
        {
            var source = await task;
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

        public static async Task<Switchable<T>> CaseAsync<T>(this Task<Switchable<T>> task, T comparand, Func<T, Task> then)
        {
            return await task.CaseAsync(v => v == null ? comparand == null : v.Equals(comparand), then);
        }

        public static async Task<Switchable<T>> CaseAsync<T>(this Task<Switchable<T>> task, Func<T, Task<bool>> when, Action<T> then)
        {
            var source = await task;
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

        public static async Task<Switchable<T>> CaseAsync<T>(this Task<Switchable<T>> task, Func<T, Task<bool>> when, Func<T, Task> then)
        {
            var source = await task;
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

        public static async Task<Switchable<T>> Break<T>(this Task<Switchable<T>> task)
        {
            var source = await task;
            if (source.AnyCasePassed)
            {
                source.Broken = true;
            }
            return source;
        }

        public static async Task Default<T>(this Task<Switchable<T>> task, Action<T> action)
        {
            var source = await task;
            if (!source.Broken)
            {
                action(source.Value);
            }
        }

        public static async Task DefaultAsync<T>(this Task<Switchable<T>> task, Func<T, Task> action)
        {
            var source = await task;
            if (!source.Broken)
            {
                await action(source.Value);
            }
        }
    }
}
#endif