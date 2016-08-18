using System;
using NUnit.Framework;

namespace AgileSwitch.Test
{
    [TestFixture]
    public class SwitchReturnTest
    {
        static string VALUE_FACTORY_EVALUATED = "Value factory shouldn't be evaluated";

        [TestCase]
        public void SwitchShouldReturnWhenCasePassed()
        {
            var result = Switch.On(10)
                .When(100)
                    .Return(1)
                .When(n => n > 5)
                    .Return(2)
                .When(10)
                    .Return(3)
                .Default(4);

            Assert.AreEqual(2, result);
        }

        [TestCase]
        public void ValueFactoryShouldNotBeEvaluatedWhenCaseFailed()
        {
            var result = Switch.On(10)
                .When(100)
                    .Return(n => { Assert.Fail(VALUE_FACTORY_EVALUATED); return 1; })
                .When(n => n < 5)
                    .Return(n => { Assert.Fail(VALUE_FACTORY_EVALUATED); return 2; })
                .When(10)
                    .Return(n => n + 1)
                .Default(4);

            Assert.AreEqual(11, result);
        }

        [TestCase]
        public void DefaultValueShouldBeReturnedIfNoCasePassed()
        {
            var result = Switch.On(10)
                .When(100)
                    .Return(1)
                .When(n => n < 5)
                    .Return(2)
                .When(1)
                    .Return(3)
                .Default(n => n + 1);

            Assert.AreEqual(11, result);
        }
    }
}
