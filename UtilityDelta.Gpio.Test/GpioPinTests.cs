using System;
using Moq;
using UtilityDelta.Gpio.Implementation;
using UtilityDelta.Gpio.Interfaces;
using Xunit;

namespace UtilityDelta.Gpio.Test
{
    public class GpioPinTests
    {
        [Fact]
        public void TestLazyExport()
        {
            var fileIo = new Mock<IFileIo>();
            var pinMapper = new Mock<IPinMapper>();
            var service = new PinController(fileIo.Object, pinMapper.Object);
            var pin = service.GetGpioPin(20);
            
            pinMapper.Verify(x => x.MapPinToSysfs(20), Times.Once);
            fileIo.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fileIo.Verify(x => x.ReadAllText(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void TestSetValue()
        {
            var fileIo = new Mock<IFileIo>();
            var pinMapper = new Mock<IPinMapper>();
            pinMapper.Setup(x => x.MapPinToSysfs(21)).Returns(33);
            var service = new PinController(fileIo.Object, pinMapper.Object);
            var pin = service.GetGpioPin(21);
            pin.PinValue = true;
            pin.PinValue = false;
            pin.PinValue = true;

            pinMapper.Verify(x => x.MapPinToSysfs(21), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/export", "33"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio33/direction", "out"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio33/direction", "in"), Times.Never);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio33/value", "1"), Times.Exactly(2));
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio33/value", "0"), Times.Exactly(1));
            fileIo.Verify(x => x.ReadAllText(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void TestGettingValue()
        {
            var fileIo = new Mock<IFileIo>();
            fileIo.Setup(x => x.ReadAllText("/sys/class/gpio/gpio43/value")).Returns("1");
            var pinMapper = new Mock<IPinMapper>();
            pinMapper.Setup(x => x.MapPinToSysfs(99)).Returns(43);
            var service = new PinController(fileIo.Object, pinMapper.Object);
            var pin = service.GetGpioPin(99);
            var value = pin.PinValue;
            value = pin.PinValue;
            value = pin.PinValue;

            Assert.True(value);

            pinMapper.Verify(x => x.MapPinToSysfs(99), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/export", "43"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio43/direction", "out"), Times.Never);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio43/direction", "in"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio43/value", "1"), Times.Exactly(0));
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio43/value", "0"), Times.Exactly(0));
            fileIo.Verify(x => x.ReadAllText("/sys/class/gpio/gpio43/value"), Times.Exactly(3));
        }

        [Fact]
        public void SetAndGetTogether()
        {
            var fileIo = new Mock<IFileIo>();
            fileIo.Setup(x => x.ReadAllText("/sys/class/gpio/gpio43/value")).Returns("0");
            var pinMapper = new Mock<IPinMapper>();
            pinMapper.Setup(x => x.MapPinToSysfs(99)).Returns(43);
            var service = new PinController(fileIo.Object, pinMapper.Object);
            var pin = service.GetGpioPin(99);
            pin.PinValue = true;
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio43/direction", "out"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio43/direction", "in"), Times.Never);

            var value = pin.PinValue;
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio43/direction", "out"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio43/direction", "in"), Times.Once);

            Assert.False(value);
            pin.PinValue = false;
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio43/direction", "out"), Times.Exactly(2));
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio43/direction", "in"), Times.Once);

            pinMapper.Verify(x => x.MapPinToSysfs(99), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/export", "43"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio43/value", "1"), Times.Exactly(1));
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/gpio43/value", "0"), Times.Exactly(1));
            fileIo.Verify(x => x.ReadAllText("/sys/class/gpio/gpio43/value"), Times.Exactly(1));
        }

        [Fact]
        public void DontNeedToUnExport()
        {
            var fileIo = new Mock<IFileIo>();
            fileIo.Setup(x => x.ReadAllText("/sys/class/gpio/gpio43/value")).Returns("0");
            var pinMapper = new Mock<IPinMapper>();
            pinMapper.Setup(x => x.MapPinToSysfs(99)).Returns(43);
            var service = new PinController(fileIo.Object, pinMapper.Object);
            using (var pin = service.GetGpioPin(99))
            {
                
            }

            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/export", "43"), Times.Never);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/unexport", "43"), Times.Never);
            fileIo.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void NeedToUnExport()
        {
            var fileIo = new Mock<IFileIo>();
            fileIo.Setup(x => x.ReadAllText("/sys/class/gpio/gpio43/value")).Returns("0");
            var pinMapper = new Mock<IPinMapper>();
            pinMapper.Setup(x => x.MapPinToSysfs(99)).Returns(43);
            var service = new PinController(fileIo.Object, pinMapper.Object);
            using (var pin = service.GetGpioPin(99))
            {
                var value = pin.PinValue;
                pin.PinValue = true;
            }

            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/export", "43"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/gpio/unexport", "43"), Times.Once);
        }
    }
}
