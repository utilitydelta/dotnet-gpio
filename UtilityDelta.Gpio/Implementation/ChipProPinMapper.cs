using System.Collections.Generic;
using System.Linq;
using UtilityDelta.Gpio.Interfaces;

namespace UtilityDelta.Gpio.Implementation
{
    public class ChipProPinMapper : IPinMapper
    {
        //https://docs.getchip.com/chip_pro.html#gr8-pins-and-multiplexing-on-c-h-i-p-pro
        //https://docs.getchip.com/chip_pro_devkit.html#gpio-sysfs-numbers
        private readonly List<ChipProPin> _chipProPinLookup = new List<ChipProPin>
        {
            new ChipProPin {PinName = "PB2", ChipProNbr = 9, SysfsNbr = 0},
            new ChipProPin {PinName = "PB5", ChipProNbr = 21, SysfsNbr = 37},
            new ChipProPin {PinName = "PB6", ChipProNbr = 22, SysfsNbr = 38},
            new ChipProPin {PinName = "PB7", ChipProNbr = 23, SysfsNbr = 39},
            new ChipProPin {PinName = "PB8", ChipProNbr = 24, SysfsNbr = 40},
            new ChipProPin {PinName = "PB9", ChipProNbr = 25, SysfsNbr = 41},
            new ChipProPin {PinName = "PB15", ChipProNbr = 11, SysfsNbr = 47},
            new ChipProPin {PinName = "PB16", ChipProNbr = 12, SysfsNbr = 48},
            new ChipProPin {PinName = "PD2", ChipProNbr = 13, SysfsNbr = 98},
            new ChipProPin {PinName = "PD3", ChipProNbr = 14, SysfsNbr = 99},
            new ChipProPin {PinName = "PD4", ChipProNbr = 15, SysfsNbr = 100},
            new ChipProPin {PinName = "PD5", ChipProNbr = 16, SysfsNbr = 101},
            new ChipProPin {PinName = "PE0", ChipProNbr = 41, SysfsNbr = 128},
            new ChipProPin {PinName = "PE1", ChipProNbr = 40, SysfsNbr = 129},
            new ChipProPin {PinName = "PE2", ChipProNbr = 39, SysfsNbr = 130},
            new ChipProPin {PinName = "PE3", ChipProNbr = 38, SysfsNbr = 131},
            new ChipProPin {PinName = "PE4", ChipProNbr = 37, SysfsNbr = 132},
            new ChipProPin {PinName = "PE5", ChipProNbr = 36, SysfsNbr = 133},
            new ChipProPin {PinName = "PE6", ChipProNbr = 35, SysfsNbr = 134},
            new ChipProPin {PinName = "PE7", ChipProNbr = 34, SysfsNbr = 135},
            new ChipProPin {PinName = "PE8", ChipProNbr = 33, SysfsNbr = 136},
            new ChipProPin {PinName = "PE9", ChipProNbr = 32, SysfsNbr = 137},
            new ChipProPin {PinName = "PE10", ChipProNbr = 31, SysfsNbr = 138},
            new ChipProPin {PinName = "PE11", ChipProNbr = 30, SysfsNbr = 139},
            new ChipProPin {PinName = "PG13", ChipProNbr = 10, SysfsNbr = 1}
        };

        public int MapPinToSysfs(string pin)
        {
            return int.TryParse(pin, out var pinNumber)
                ? _chipProPinLookup.First(x => x.ChipProNbr == pinNumber).SysfsNbr
                : _chipProPinLookup.First(x => x.PinName == pin).SysfsNbr;
        }

        private struct ChipProPin
        {
            public int ChipProNbr;
            public string PinName;
            public int SysfsNbr;
        }
    }
}