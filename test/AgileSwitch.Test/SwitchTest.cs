using System;
using NUnit.Framework;

namespace AgileSwitch.Test
{
    [TestFixture]
    public class SwitchTest
    {
        [TestCase]
        public void SwitchShouldContinueIfNoBreak()
        {
            var case1Executed = false;
            var case2Executed = false;
            var case3Executed = false;
            var case4Executed = false;

            Switch.On(10)
                .When(100)
                    .Then(n => case1Executed = true)
                .When(n => n > 5)
                    .Then(n => case2Executed = true)
                .When(1)
                    .Then(n => case3Executed = true)
                .Default(n => case4Executed = true);

            Assert.AreEqual(false, case1Executed);
            Assert.AreEqual(true, case2Executed);
            Assert.AreEqual(false, case3Executed);
            Assert.AreEqual(true, case4Executed);
        }

        [TestCase]
        public void SwitchShouldBreakWhenCasePassed()
        {
            var case1Executed = false;
            var case2Executed = false;
            var case3Executed = false;
            var case4Executed = false;

            Switch.On(10)
                .When(n => n < 5)
                    .Then(n => case1Executed = true)
                    .Break()
                .When(10)
                    .Then(n => case2Executed = true)
                    .Break()
                .When(n => n > 1)
                    .Then(n => case3Executed = true)
                    .Break()
                .Default(n => case4Executed = true);

            Assert.AreEqual(false, case1Executed);
            Assert.AreEqual(true, case2Executed);
            Assert.AreEqual(false, case3Executed);
            Assert.AreEqual(false, case4Executed);
        }

        [TestCase]
        public void DefaultShouldBeExecutedIfNoCaseMatched()
        {
            var case1Executed = false;
            var case2Executed = false;
            var case3Executed = false;
            var case4Executed = false;

            Switch.On(10)
                .When(100)
                    .Then(n => case1Executed = true)
                    .Break()
                .When(n => n < 5)
                    .Then(n => case2Executed = true)
                    .Break()
                .When(1)
                    .Then(n => case3Executed = true)
                    .Break()
                .Default(n => case4Executed = true);

            Assert.AreEqual(false, case1Executed);
            Assert.AreEqual(false, case2Executed);
            Assert.AreEqual(false, case3Executed);
            Assert.AreEqual(true, case4Executed);
        }
    }
}
