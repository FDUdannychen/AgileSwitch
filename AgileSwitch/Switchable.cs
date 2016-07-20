using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileSwitch
{
    public class Switchable<T>
    {
        internal T Value { get; set; }
        internal bool Broken { get; set; }
        internal bool AnyCasePassed { get; set; }

        internal Switchable(T value)
        {
            this.Value = value;
        }

        public Switchable<T> Case<TCase>(Action<TCase> then)
        {
            return this.Case(v => v is TCase, v => then((TCase)(object)v));
        }
    }
}
