using System;

namespace UtilityDelta.Gpio.Interfaces
{
    public interface IPinController : IDisposable
    {
        IPwmPin GetPwmPin(string pin);
        IGpioPin GetGpioPin(string pin);
    }
}