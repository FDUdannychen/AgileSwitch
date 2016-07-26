using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgileSwitch
{
    class Switchable<T> : ISwitchable<T>
    {
        private T _value;
        private bool _break = false;
        private bool _passCase = false;

        public Switchable(T value)
        {
            _value = value;
        }

        public ISwitchable<T> Case(Func<T, bool> when, Action<T> then)
        {
            if (!_break)
            {
                _passCase = when(_value);

                if (_passCase)
                {
                    then(_value);
                }
            }
            return this;
        }

        public ISwitchable<T> Case(T comparand, Action<T> then)
        {
            return this.Case(v => v == null ? comparand ==null : v.Equals(comparand), then);
        }

        public ISwitchable<T> Case<TCase>(Action<TCase> then)
        {
            return this.Case(v => v is TCase, v => then((TCase)(object)v));
        }

        public ISwitchable<T> Break()
        {
            if (_passCase)
            {
                _break = true;
            }
            return this;
        }

        public void Default(Action<T> action)
        {
            if (!_break)
            {
                action(_value);
            }
        }
    }
}
