using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileSwitch.FlowControl;

namespace AgileSwitch
{
    class SwitchObject<T> : IStart<T>, IVoidStart<T>, IAfterWhen<T>, IAfterThen<T>, IVoidAfterWhen<T>
    {
        internal T Value { get; set; }
        internal bool Broken { get; set; }
        internal bool PreviousCasePassed { get; set; }
    }

    class ReturnSwitchObject<T, TReturn> : SwitchObject<T>, IReturnStart<T, TReturn>, IReturnAfterWhen<T, TReturn>
    {
        internal TReturn ReturnValue { get; set; }
    }
}