using System;
using UtilityDelta.Gpio.Interfaces;

namespace UtilityDelta.Gpio.Implementation
{
    public class SysfsPinMapper : IPinMapper
    {
        public int MapPinToSysfs(string pin)
        {
            return Convert.ToInt32(pin);
        }
    }
}