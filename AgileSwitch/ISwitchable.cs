using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileSwitch
{
    public interface ISwitchable<T>
    {
        ISwitchable<T> Case(Func<T, bool> when, Action<T> then);
        ISwitchable<T> Case<TCase>(Action<TCase> then);
        ISwitchable<T> Break();
        void Default(Action<T> action);
    }
}
