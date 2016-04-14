using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgileSwitch.Test
{
    [TestClass]
    public class SwitchTest
    {
        [TestMethod]
        public void ActionShouldBeExecutedWhenCaseSucceeds()
        {
            var case1Executed = false;
            var case2Executed = false;
            var case3Executed = false;
            var case4Executed = false;

            Switch.On(10)
                .Case<string>(s => case1Executed = true)
                .Case(n => n > 5, n => case2Executed = true)
                .Case<int>(n => case3Executed = true)
                .Case(n => n > 100, n => case4Executed = true);

            Assert.AreEqual(false, case1Executed);
            Assert.AreEqual(true, case2Executed);
            Assert.AreEqual(true, case3Executed);
            Assert.AreEqual(false, case4Executed);
        }

        [TestMethod]
        public void SwitchShouldContinueIfNoBreak()
        {
            var caseCount = 0;
            var defaultExecuted = false;

            Switch.On(10)
                .Case(n => n < 100, n => caseCount++)
                .Case(n => n < 101, n => caseCount++)
                .Case(n => n < 102, n => caseCount++)
                .Default(n => defaultExecuted = true);

            Assert.AreEqual(3, caseCount);
            Assert.AreEqual(true, defaultExecuted);
        }

        [TestMethod]
        public void SwitchShouldBreakWhenNecessary()
        {
            var caseId = 0;

            Switch.On(10)
                .Case(n => n < 100, n => caseId = 1)
                .Case(n => n > 10, n => caseId = 2).Break()
                .Case<int>(n => caseId = 3).Break()
                .Case(n => n < 100, n => caseId = 4)
                .Default(n => Assert.Fail("should break before default"));

            Assert.AreEqual(3, caseId);
        }
    }
}
