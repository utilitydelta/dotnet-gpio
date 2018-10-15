using System;
using UtilityDelta.Gpio.EventArgs;

namespace UtilityDelta.Gpio.Interfaces
{
    internal interface IPinChangeReader
    {
        event EventHandler<PinChangedEventArgs> GpioChanged;
        void Start(IGpioPin pin);
        void Stop();
    }
}