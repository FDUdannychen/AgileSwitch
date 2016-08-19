using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AgileSwitch.Test
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SwitchReturnAsyncTest
    {
        static string VALUE_FACTORY_EVALUATED = "Value factory shouldn't be evaluated";

        [TestCase]
        public async Task SwitchShouldReturnWhenCasePassedAsync()
        {
            var result = await Switch.On(10)
                .When(100)
                    .Return(1)
                .WhenAsync(async n => await Task.FromResult(n > 5).ConfigureAwait(false))
                    .Return(2)
                .When(10)
                    .ReturnAsync(async n => await Task.FromResult(3).ConfigureAwait(false))
                .WhenAsync(async n => await Task.FromResult(false))
                    .ReturnAsync(async n => await Task.FromResult(4).ConfigureAwait(false))
                .Default(5);

            Assert.AreEqual(2, result);
        }

        [TestCase]
        public async Task ValueFactoryShouldNotBeEvaluatedWhenCaseFailedAsync()
        {
            var result = await Switch.On(10)
                .When(100)
                    .Return(n => { Assert.Fail(VALUE_FACTORY_EVALUATED); return 1; })
                .WhenAsync(async n => await Task.FromResult(n > 5).ConfigureAwait(false))
                    .Return(n => n - 1)
                .When(1)
                    .ReturnAsync(async n => { Assert.Fail(VALUE_FACTORY_EVALUATED); return await Task.FromResult(3).ConfigureAwait(false); })
                .WhenAsync(async n => await Task.FromResult(false))
                    .ReturnAsync(async n => { Assert.Fail(VALUE_FACTORY_EVALUATED); return await Task.FromResult(4).ConfigureAwait(false); })
                .DefaultAsync(async n => await Task.FromResult(n + 1).ConfigureAwait(false));

            Assert.AreEqual(9, result);
        }

        [TestCase]
        public async Task DefaultValueShouldBeReturnedIfNoCasePassedAsync()
        {
            var result = await Switch.On(10)                
                .WhenAsync(async n => await Task.FromResult(n < 5).ConfigureAwait(false))
                    .Return(n => n - 1)
                .When(1)
                    .ReturnAsync(async n => await Task.FromResult(3).ConfigureAwait(false))
                .When(100)
                    .Return(1)
                .WhenAsync(async n => await Task.FromResult(false))
                    .ReturnAsync(async n => await Task.FromResult(4).ConfigureAwait(false))
                .DefaultAsync(async n => await Task.FromResult(n + 1).ConfigureAwait(false));

            Assert.AreEqual(11, result);
        }
    }
}
