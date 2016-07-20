using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileSwitch
{
    public static class Switch
    {
        public static Switchable<T> On<T>(T value)
        {
            return new Switchable<T>(value);
        }
    }
}
