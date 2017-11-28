using System;
using System.Collections.Generic;
using System.Linq;
using UtilityDelta.Gpio.Interfaces;

namespace UtilityDelta.Gpio.Implementation
{
    public class SysfsPinMapper : IPinMapper
    {
        public int MapPinToSysfs(string pin) => Convert.ToInt32(pin);
    }
}