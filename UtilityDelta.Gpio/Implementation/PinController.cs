using System.Collections.Generic;
using UtilityDelta.Gpio.Interfaces;

namespace UtilityDelta.Gpio.Implementation
{
    public class PinController : IPinController
    {
        private readonly IFileIo _fileIo;
        private readonly Dictionary<string, GpioPin> _gpioDict = new Dictionary<string, GpioPin>();
        private readonly IPinMapper _pinMapper;
        private readonly Dictionary<string, PwmPin> _pwmDict = new Dictionary<string, PwmPin>();

        public PinController(IFileIo fileIo, IPinMapper pinMapper)
        {
            _fileIo = fileIo;
            _pinMapper = pinMapper;
        }

        public IPwmPin GetPwmPin(string pin)
        {
            if (_pwmDict.TryGetValue(pin, out var result)) return result;
            result = new PwmPin(_pinMapper.MapPinToSysfs(pin), _fileIo);
            _pwmDict.Add(pin, result);
            return result;
        }

        public IGpioPin GetGpioPin(string pin)
        {
            if (_gpioDict.TryGetValue(pin, out var result)) return result;
            result = new GpioPin(_pinMapper.MapPinToSysfs(pin), _fileIo);
            _gpioDict.Add(pin, result);
            return result;
        }

        public void Dispose()
        {
            foreach (var gpioPin in _gpioDict)
                gpioPin.Value.Dispose();
            foreach (var pwmPin in _pwmDict)
                pwmPin.Value.Dispose();
        }
    }
}