using System;

namespace DevilDesireDevLib.Implementation.Events
{
    public class ValueChangedEventArgs : EventArgs
    {
        public readonly int LastValue;
        public readonly int NewValue;

        public ValueChangedEventArgs(int lastValue, int newValue)
        {
            LastValue = lastValue;
            NewValue = newValue;
        }
    }
}