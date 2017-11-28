using UtilityDelta.Gpio.Interfaces;

namespace UtilityDelta.Gpio.Implementation
{
    public class PinController : IPinController
    {
        private readonly IFileIo _fileIo;
        private readonly IPinMapper _pinMapper;

        public PinController(IFileIo fileIo, IPinMapper pinMapper)
        {
            _fileIo = fileIo;
            _pinMapper = pinMapper;
        }

        public IPwmPin GetPwmPin(int pin)
        {
            return new PwmPin(_pinMapper.MapPinToSysfs(pin), _fileIo);
        }

        public IGpioPin GetGpioPin(int pin)
        {
            return new GpioPin(_pinMapper.MapPinToSysfs(pin), _fileIo);
        }
    }
}