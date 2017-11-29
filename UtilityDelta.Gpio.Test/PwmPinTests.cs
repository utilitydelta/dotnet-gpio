using Moq;
using UtilityDelta.Gpio.Enums;
using UtilityDelta.Gpio.Implementation;
using UtilityDelta.Gpio.Interfaces;
using Xunit;

namespace UtilityDelta.Gpio.Test
{
    public class PwmPinTests
    {
        [Fact]
        public void TestLazyExport()
        {
            var fileIo = new Mock<IFileIo>();
            var pinMapper = new Mock<IPinMapper>();
            using (var service = new PinController(fileIo.Object, pinMapper.Object))
            {
                var pin = service.GetPwmPin("20");
                Assert.NotNull(pin);
            }

            pinMapper.Verify(x => x.MapPinToSysfs("20"), Times.Once);
            fileIo.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            fileIo.Verify(x => x.ReadAllText(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void TestPinProps()
        {
            var fileIo = new Mock<IFileIo>();
            var pinMapper = new Mock<IPinMapper>();
            pinMapper.Setup(x => x.MapPinToSysfs("20")).Returns(43);
            pinMapper.Setup(x => x.MapPinToSysfs("23")).Returns(44);
            using (var service = new PinController(fileIo.Object, pinMapper.Object))
            {
                var pin1 = service.GetPwmPin("20");
                var pin2 = service.GetPwmPin("23");

                pin1.DutyCycle = 33;
                pin1.Period = 55;
                pin1.Polarity = PwmPolarity.Inversed;
                pin1.Enabled = true;
                pin1.Enabled = false;

                pin2.DutyCycle = 36;
                pin2.Period = 56;
                pin2.Polarity = PwmPolarity.Normal;
                pin2.Enabled = true;
                pin2.Enabled = false;

                //Test caching pwm
                pin1 = service.GetPwmPin("20");
                pin1.DutyCycle = 393;
            }

            pinMapper.Verify(x => x.MapPinToSysfs("20"), Times.Once);
            pinMapper.Verify(x => x.MapPinToSysfs("23"), Times.Once);

            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/export", "43"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/export", "44"), Times.Once);

            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/pwm43/polarity", "inversed"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/pwm44/polarity", "normal"), Times.Once);

            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/pwm43/duty_cycle", "33"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/pwm43/duty_cycle", "393"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/pwm44/duty_cycle", "36"), Times.Once);

            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/pwm43/period", "55"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/pwm44/period", "56"), Times.Once);

            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/pwm43/enable", "1"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/pwm43/enable", "0"), Times.Exactly(2));
            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/pwm44/enable", "1"), Times.Once);
            fileIo.Verify(x => x.WriteAllText("/sys/class/pwm/pwmchip0/pwm44/enable", "0"), Times.Exactly(2));

            fileIo.Verify(x => x.ReadAllText(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void TestPinReadProps()
        {
            var fileIo = new Mock<IFileIo>();
            fileIo.Setup(x => x.ReadAllText("/sys/class/pwm/pwmchip0/pwm43/polarity")).Returns("normal");
            fileIo.Setup(x => x.ReadAllText("/sys/class/pwm/pwmchip0/pwm43/duty_cycle")).Returns("39");
            fileIo.Setup(x => x.ReadAllText("/sys/class/pwm/pwmchip0/pwm43/period")).Returns("53");
            fileIo.Setup(x => x.ReadAllText("/sys/class/pwm/pwmchip0/pwm43/enable")).Returns("1");

            var pinMapper = new Mock<IPinMapper>();
            pinMapper.Setup(x => x.MapPinToSysfs("20")).Returns(43);
            pinMapper.Setup(x => x.MapPinToSysfs("23")).Returns(44);

            using (var service = new PinController(fileIo.Object, pinMapper.Object))
            {
                var pin1 = service.GetPwmPin("20");
                var pin2 = service.GetPwmPin("23");

                Assert.Equal(pin1.Polarity, PwmPolarity.Normal);
                Assert.Equal(pin1.Polarity, PwmPolarity.Normal);
                Assert.Equal(pin1.DutyCycle, 39);
                Assert.Equal(pin1.Period, 53);
                Assert.True(pin1.Enabled);

                Assert.Equal(pin2.Polarity, PwmPolarity.Normal);
                Assert.Equal(pin2.DutyCycle, 0);
                Assert.Equal(pin2.Period, 0);
                Assert.False(pin2.Enabled);
            }
        }
    }
}