#if !NET40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileSwitch.FlowControl;

namespace AgileSwitch
{
    public static class AsyncExtensions
    {
        public static async Task<IAfterWhen<T>> WhenAsync<T>(this IStart<T> source, Func<T, Task<bool>> predicate)
        {
            return await WhenAsyncImpl<IAfterWhen<T>, T>(source, predicate).ConfigureAwait(false);
        }
        
        public static async Task<IVoidAfterWhen<T>> WhenAsync<T>(this IVoidStart<T> source, Func<T, Task<bool>> predicate)
        {
            return await WhenAsyncImpl<IVoidAfterWhen<T>, T>(source, predicate).ConfigureAwait(false);
        }

        public static async Task<IVoidAfterWhen<T>> When<T>(this Task<IVoidStart<T>> source, T comparand)
        {
            return (await source.ConfigureAwait(false)).When(comparand);
        }

        public static async Task<IVoidAfterWhen<T>> When<T>(this Task<IVoidStart<T>> source, Func<T, bool> predicate)
        {
            return (await source.ConfigureAwait(false)).When(predicate);
        }

        public static async Task<IVoidAfterWhen<T>> WhenAsync<T>(this Task<IVoidStart<T>> source, Func<T, Task<bool>> predicate)
        {
            return await WhenAsyncImpl<IAfterWhen<T>, T>(await source.ConfigureAwait(false), predicate).ConfigureAwait(false);
        }

        public static async Task<IVoidAfterWhen<T>> When<T>(this Task<IAfterThen<T>> source, T comparand)
        {
            return (await source.ConfigureAwait(false)).When(comparand);
        }

        public static async Task<IVoidAfterWhen<T>> When<T>(this Task<IAfterThen<T>> source, Func<T, bool> predicate)
        {
            return (await source.ConfigureAwait(false)).When(predicate);
        }

        public static async Task<IVoidAfterWhen<T>> WhenAsync<T>(this Task<IAfterThen<T>> source, Func<T, Task<bool>> predicate)
        {
            return await WhenAsyncImpl<IVoidAfterWhen<T>, T>(await source.ConfigureAwait(false), predicate).ConfigureAwait(false);
        }

        public static async Task<IReturnAfterWhen<T, TReturn>> When<T, TReturn>(this Task<IReturnStart<T, TReturn>> source, T comparand)
        {
            return (await source.ConfigureAwait(false)).When(comparand);
        }

        public static async Task<IReturnAfterWhen<T, TReturn>> When<T, TReturn>(this Task<IReturnStart<T, TReturn>> source, Func<T, bool> predicate)
        {
            return (await source.ConfigureAwait(false)).When(predicate);
        }

        public static async Task<IReturnAfterWhen<T, TReturn>> WhenAsync<T, TReturn>(this IReturnStart<T, TReturn> source, Func<T, Task<bool>> predicate)
        {
            return await WhenAsyncImpl<IReturnAfterWhen<T, TReturn>, T>(source, predicate).ConfigureAwait(false);
        }

        public static async Task<IReturnAfterWhen<T, TReturn>> WhenAsync<T, TReturn>(this Task<IReturnStart<T, TReturn>> source, Func<T, Task<bool>> predicate)
        {
            return await (await source.ConfigureAwait(false)).WhenAsync(predicate).ConfigureAwait(false);
        }

        static async Task<TInterface> WhenAsyncImpl<TInterface, T>(object s, Func<T, Task<bool>> predicate) where TInterface : class
        {
            var source = s as SwitchObject<T>;
            source.PreviousCasePassed = !source.Broken && await predicate(source.Value).ConfigureAwait(false);
            return source as TInterface;
        }

        public static async Task<IAfterThen<T>> Then<T>(this Task<IAfterWhen<T>> source, Action<T> action)
        {
            var s = await source.ConfigureAwait(false) as SwitchObject<T>;
            s.Then(action);
            return s;
        }

        public static async Task<IAfterThen<T>> Then<T>(this Task<IVoidAfterWhen<T>> source, Action<T> action)
        {
            return (await source.ConfigureAwait(false)).Then(action);
        }

        public static async Task<IAfterThen<T>> ThenAsync<T>(this IVoidAfterWhen<T> source, Func<T, Task> action)
        {
            var s = source as SwitchObject<T>;
            if (!s.Broken && s.PreviousCasePassed)
            {
                await action(s.Value).ConfigureAwait(false);
            }
            return s;
        }

        public static async Task<IAfterThen<T>> ThenAsync<T>(this Task<IAfterWhen<T>> source, Func<T, Task> action)
        {
            return await (await source.ConfigureAwait(false)).ThenAsync(action).ConfigureAwait(false);
        }

        public static async Task<IAfterThen<T>> ThenAsync<T>(this Task<IVoidAfterWhen<T>> source, Func<T, Task> action)
        {
            return await (await source.ConfigureAwait(false)).ThenAsync(action).ConfigureAwait(false);
        }

        public static async Task<IVoidStart<T>> Break<T>(this Task<IAfterThen<T>> source)
        {
            return (await source.ConfigureAwait(false)).Break();
        }

        public static async Task<IReturnStart<T, TReturn>> Return<T, TReturn>(this Task<IAfterWhen<T>> source, TReturn returnValue)
        {
            return (await source.ConfigureAwait(false)).Return(returnValue);
        }

        public static async Task<IReturnStart<T, TReturn>> Return<T, TReturn>(this Task<IAfterWhen<T>> source, Func<T, TReturn> returnValueFactory)
        {
            return (await source.ConfigureAwait(false)).Return(returnValueFactory);
        }

        public static async Task<IReturnStart<T, TReturn>> ReturnAsync<T, TReturn>(this IAfterWhen<T> source, Func<T, Task<TReturn>> returnValueFactory)
        {
            var s = source as SwitchObject<T>;
            return new ReturnSwitchObject<T, TReturn>
            {
                Value = s.Value,
                Broken = false,
                PreviousCasePassed = s.PreviousCasePassed,
                ReturnValue = s.PreviousCasePassed ? await returnValueFactory(s.Value).ConfigureAwait(false) : default(TReturn)
            };
        }
        
        public static async Task<IReturnStart<T, TReturn>> ReturnAsync<T, TReturn>(this IReturnAfterWhen<T, TReturn> source, Func<T, Task<TReturn>> returnValueFactory)
        {
            var s = source as ReturnSwitchObject<T, TReturn>;
            if (!s.Broken && s.PreviousCasePassed)
            {
                s.ReturnValue = await returnValueFactory(s.Value).ConfigureAwait(false);
                s.Broken = true;
            }
            return s;
        }

        public static async Task<IReturnStart<T, TReturn>> Return<T, TReturn>(this Task<IReturnAfterWhen<T, TReturn>> source, TReturn returnValue)
        {
            return (await source.ConfigureAwait(false)).Return(returnValue);
        }

        public static async Task<IReturnStart<T, TReturn>> Return<T, TReturn>(this Task<IReturnAfterWhen<T, TReturn>> source, Func<T, TReturn> returnValueFactory)
        {
            return (await source.ConfigureAwait(false)).Return(returnValueFactory);
        }

        public static async Task<IReturnStart<T, TReturn>> ReturnAsync<T, TReturn>(this Task<IReturnAfterWhen<T, TReturn>> source, Func<T, Task<TReturn>> returnValueFactory)
        {
            return await (await source.ConfigureAwait(false)).ReturnAsync(returnValueFactory).ConfigureAwait(false);
        }

        public static async Task Default<T>(this Task<IVoidStart<T>> source, Action<T> action)
        {
            (await source.ConfigureAwait(false)).Default(action);
        }

        public static async Task DefaultAsync<T>(this Task<IVoidStart<T>> source, Func<T, Task> action)
        {
            await (await source.ConfigureAwait(false)).DefaultAsync(action).ConfigureAwait(false);
        }

        public static async Task Default<T>(this Task<IAfterThen<T>> source, Action<T> action)
        {
            (await source.ConfigureAwait(false)).Default(action);
        }

        public static async Task DefaultAsync<T>(this Task<IAfterThen<T>> source, Func<T, Task> action)
        {
            await (await source.ConfigureAwait(false)).DefaultAsync(action).ConfigureAwait(false);
        }

        public static async Task DefaultAsync<T>(this IVoidStart<T> source, Func<T, Task> action)
        {
            var s = source as SwitchObject<T>;
            if (!s.Broken)
            {
                await action(s.Value).ConfigureAwait(false);
            }
        }

        public static async Task<TReturn> Default<T, TReturn>(this Task<IReturnStart<T, TReturn>> source, TReturn defaultValue)
        {
            return (await source.ConfigureAwait(false)).Default(defaultValue);
        }

        public static async Task<TReturn> Default<T, TReturn>(this Task<IReturnStart<T, TReturn>> source, Func<T, TReturn> defaultValueFactory)
        {
            return (await source.ConfigureAwait(false)).Default(defaultValueFactory);
        }

        public static async Task<TReturn> DefaultAsync<T, TReturn>(this Task<IReturnStart<T, TReturn>> source, Func<T, Task<TReturn>> defaultValueFactory)
        {
            return await (await source.ConfigureAwait(false)).DefaultAsync(defaultValueFactory).ConfigureAwait(false);
        }

        public static async Task<TReturn> DefaultAsync<T, TReturn>(this IReturnStart<T, TReturn> source, Func<T, Task<TReturn>> defaultValueFactory)
        {
            var s = source as ReturnSwitchObject<T, TReturn>;
            return s.Broken ? s.ReturnValue : await defaultValueFactory(s.Value).ConfigureAwait(false);
        }
    }
}
#endif
