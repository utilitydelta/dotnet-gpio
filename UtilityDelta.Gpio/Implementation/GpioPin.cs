using System;
using System.Globalization;
using UtilityDelta.Gpio.Enums;
using UtilityDelta.Gpio.EventArgs;
using UtilityDelta.Gpio.Interfaces;

namespace UtilityDelta.Gpio.Implementation
{
    public class GpioPin : IGpioPin, IDisposable
    {
        private const string UnExportPath = "/sys/class/gpio/unexport";
        private const string ExportPath = "/sys/class/gpio/export";
        private const string DirectionPath = "/sys/class/gpio/gpio{0}/direction";
        private const string ValuePath = "/sys/class/gpio/gpio{0}/value";

        private const char PinOnChar = '1';
        private const string PinOn = "1";
        private const string PinOff = "0";

        private readonly string _directionPinPath;
        private readonly IFileIo _fileIo;

        private readonly string _sysfsPinNumber;
        private readonly string _valuePinPath;
        private GpioDirection? _direction;
        private bool _exported;

        public GpioPin(int sysfsPinNumber, IFileIo fileIo)
        {
            _fileIo = fileIo;
            _sysfsPinNumber = sysfsPinNumber.ToString(CultureInfo.InvariantCulture);
            _directionPinPath = string.Format(DirectionPath, _sysfsPinNumber);
            _valuePinPath = string.Format(ValuePath, _sysfsPinNumber);
        }

        public void Dispose()
        {
            if (_exported)
                _fileIo.WriteAllText(UnExportPath, _sysfsPinNumber);
        }

        public bool PinValue
        {
            get
            {
                if (!_direction.HasValue)
                {
                    SetDirection(GpioDirection.In);
                }
                return _fileIo.ReadAllText(_valuePinPath)[0] == PinOnChar;
            }
            set
            {
                SetDirection(GpioDirection.Out);
                _fileIo.WriteAllText(_valuePinPath, value ? PinOn : PinOff);
            }
        }

        public string GetValuePath() => _valuePinPath;

        public string GetDirectionPath() => _directionPinPath;

        private void SetDirection(GpioDirection gpioDirection)
        {
            ExportPinIfRequired();

            if (_direction == gpioDirection) return;

            _fileIo.WriteAllText(_directionPinPath, gpioDirection.ToString().ToLowerInvariant());
            _direction = gpioDirection;
        }

        private void ExportPinIfRequired()
        {
            if (_exported) return;

            _fileIo.WriteAllText(ExportPath, _sysfsPinNumber);
            _exported = true;
        }
    }
}