using System;

namespace UtilityDelta.Gpio.Interfaces
{
    public interface IGpioPin : IDisposable
    {
        bool PinValue { get; set; }
    }
}