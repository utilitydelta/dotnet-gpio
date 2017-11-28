using System;
using System.Collections.Generic;
using System.Text;
using UtilityDelta.Gpio.Implementation;
using Xunit;

namespace UtilityDelta.Gpio.Test
{
    public class ChipProTest
    {
        [Fact]
        public void TestMappings()
        {
            var service = new ChipProPinMapper();
            Assert.Equal(138, service.MapPinToSysfs("PE10"));
            Assert.Equal(138, service.MapPinToSysfs("31"));
            Assert.Equal(39, service.MapPinToSysfs("PB7"));
            Assert.Equal(39, service.MapPinToSysfs("23"));
        }
    }
}
