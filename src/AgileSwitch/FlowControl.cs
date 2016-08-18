using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgileSwitch.FlowControl
{
    public interface IStart<T> { }
    public interface IAfterWhen<T> : IVoidAfterWhen<T> { }
    public interface IAfterThen<T> : IVoidStart<T> { }

    public interface IVoidStart<T> { }
    public interface IVoidAfterWhen<T> { }
    public interface IReturnStart<T, TReturn> { }
    public interface IReturnAfterWhen<T, TReturn> { }
}
