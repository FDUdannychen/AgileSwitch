using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileSwitch.FlowControl;

namespace AgileSwitch
{
    public static class Switch
    {
        public static IStart<T> On<T>(T value)
        {
            return new SwitchObject<T> { Value = value };
        }
    }
}
