#if !NET40
using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AgileSwitch.Test
{
    [TestFixture]
    public class SwitchAsyncTest
    {
        [TestCase]
        public async Task ActionShouldBeExecutedWhenCaseAsyncSucceeds()
        {
            var case1Executed = false;
            var case2Executed = false;
            var case3Executed = false;
            var case4Executed = false;

            await Switch.On(10)
                .Case<string>(s => case1Executed = true)
                .CaseAsync(n => n > 5, async n => { await Task.Delay(2000); case2Executed = true; })
                .Case(10, n => case3Executed = true)
                .CaseAsync(100, async n => { await Task.Delay(1); case4Executed = true; });

            Assert.AreEqual(false, case1Executed);
            Assert.AreEqual(true, case2Executed);
            Assert.AreEqual(true, case3Executed);
            Assert.AreEqual(false, case4Executed);
        }

        [TestCase]
        public async Task SwitchShouldContinueIfNoBreak()
        {
            var caseCount = 0;
            var defaultExecuted = false;

            await Switch.On(10)
                .CaseAsync(n => n < 100, async n => { await Task.Delay(1); caseCount++; })
                .CaseAsync(10, async n => { await Task.Delay(1); caseCount++; })
                .CaseAsync(async n => { await Task.Delay(1); return n == 10; }, async n => { await Task.Delay(1); caseCount++; })
                .DefaultAsync(async n => { await Task.Delay(1); defaultExecuted = true; });

            Assert.AreEqual(3, caseCount);
            Assert.AreEqual(true, defaultExecuted);
        }

        [TestCase]
        public async Task SwitchShouldBreakWhenNecessary()
        {
            var caseId = 0;

            await Switch.On(10)
                .Case(n => n < 100, n => caseId = 1)
                .CaseAsync(100, async n => { await Task.Delay(1); caseId = 2; })
                    .Break()
                .Case(10, n => caseId = 3)
                    .Break()
                .CaseAsync(n => n < 100, async n => { await Task.Delay(1); caseId = 4; })
                .Default(n => Assert.Fail("should break before default"));

            Assert.AreEqual(3, caseId);
        }
    }
}
#endif