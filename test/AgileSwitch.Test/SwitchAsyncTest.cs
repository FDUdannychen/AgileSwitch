#if !NET40
using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AgileSwitch.Test
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SwitchAsyncTest
    {
        [TestCase]
        public async Task SwitchShouldContinueIfNoBreakAsync()
        {
            var case1Executed = false;
            var case2Executed = false;
            var case3Executed = false;
            var case4Executed = false;
            var defaultExecuted = false;

            await Switch.On(10)
                .WhenAsync(async n => await Task.FromResult(n > 5).ConfigureAwait(false))
                    .ThenAsync(async n=> await Task.FromResult(case1Executed = true).ConfigureAwait(false))
                .When(100)
                    .ThenAsync(async n => await Task.FromResult(case2Executed = true).ConfigureAwait(false))
                .When(1)
                    .Then(n => case3Executed = true)
                .WhenAsync(async n => await Task.FromResult(n > 1).ConfigureAwait(false))
                    .Then(n => case4Executed = true)                                
                .DefaultAsync(async n => await Task.FromResult(defaultExecuted = true).ConfigureAwait(false));

            Assert.AreEqual(true, case1Executed);
            Assert.AreEqual(false, case2Executed);
            Assert.AreEqual(false, case3Executed);
            Assert.AreEqual(true, case4Executed);
            Assert.AreEqual(true, defaultExecuted);
        }

        [TestCase]
        public async Task SwitchShouldBreakWhenCasePassedAsync()
        {
            var case1Executed = false;
            var case2Executed = false;
            var case3Executed = false;
            var case4Executed = false;
            var defaultExecuted = false;

            await Switch.On(10)
                .When(100)
                    .ThenAsync(async n => await Task.FromResult(case1Executed = true).ConfigureAwait(false))
                    .Break()
                .WhenAsync(async n => await Task.FromResult(n > 5).ConfigureAwait(false))
                    .ThenAsync(async n => await Task.FromResult(case2Executed = true).ConfigureAwait(false))
                    .Break()
                .When(10)
                    .Then(n => case3Executed = true)
                    .Break()
                .WhenAsync(async n => await Task.FromResult(n > 1).ConfigureAwait(false))
                    .Then(n => case4Executed = true)
                    .Break()
                .Default(n => defaultExecuted = true);

            Assert.AreEqual(false, case1Executed);
            Assert.AreEqual(true, case2Executed);
            Assert.AreEqual(false, case3Executed);
            Assert.AreEqual(false, case4Executed);
            Assert.AreEqual(false, defaultExecuted);
        }

        [TestCase]
        public async Task DefaultShouldBeExecutedIfNoCaseMatchedAsync()
        {
            var case1Executed = false;
            var case2Executed = false;
            var case3Executed = false;
            var case4Executed = false;
            var defaultExecuted = false;

            await Switch.On(10)
                .When(100)
                    .ThenAsync(async n => await Task.FromResult(case1Executed = true).ConfigureAwait(false))
                    .Break()
                .WhenAsync(async n => await Task.FromResult(n < 5).ConfigureAwait(false))
                    .ThenAsync(async n => await Task.FromResult(case2Executed = true).ConfigureAwait(false))
                    .Break()
                .WhenAsync(async n => await Task.FromResult(n < 1).ConfigureAwait(false))
                    .Then(n => case4Executed = true)
                .When(1)
                    .Then(n => case3Executed = true)
                .Default(n => defaultExecuted = true);

            Assert.AreEqual(false, case1Executed);
            Assert.AreEqual(false, case2Executed);
            Assert.AreEqual(false, case3Executed);
            Assert.AreEqual(false, case4Executed);
            Assert.AreEqual(true, defaultExecuted);
        }
    }
}
#endif
