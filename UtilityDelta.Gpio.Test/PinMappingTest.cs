using UtilityDelta.Gpio.Implementation;
using Xunit;

namespace UtilityDelta.Gpio.Test
{
    public class PinMappingTest
    {
        [Fact]
        public void TestChipProMappings()
        {
            var service = new ChipProPinMapper();
            Assert.Equal(138, service.MapPinToSysfs("PE10"));
            Assert.Equal(138, service.MapPinToSysfs("31"));
            Assert.Equal(39, service.MapPinToSysfs("PB7"));
            Assert.Equal(39, service.MapPinToSysfs("23"));
        }

        [Fact]
        public void TestSysfsPinMappings()
        {
            var service = new SysfsPinMapper();
            Assert.Equal(55, service.MapPinToSysfs("55"));
            Assert.Equal(77, service.MapPinToSysfs("77"));
        }
    }
}