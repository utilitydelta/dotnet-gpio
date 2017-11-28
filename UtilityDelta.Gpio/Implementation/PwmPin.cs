using System;
using System.Globalization;
using UtilityDelta.Gpio.Interfaces;

namespace UtilityDelta.Gpio.Implementation
{
    public class PwmPin : IPwmPin
    {
        private const string UnExportPath = "/sys/class/pwm/pwmchip0/unexport";
        private const string ExportPath = "/sys/class/pwm/pwmchip0/export";
        private const string PolarityPath = "/sys/class/pwm/pwmchip0/pwm{0}/polarity";
        private const string EnablePath = "/sys/class/pwm/pwmchip0/pwm{0}/enable";
        private const string PeriodPath = "/sys/class/pwm/pwmchip0/pwm{0}/period";
        private const string DutyCyclePath = "/sys/class/pwm/pwmchip0/pwm{0}/duty_cycle";
        private const string PinOn = "1";
        private const string PinOff = "0";
        private readonly string _dutyCyclePinPath;
        private readonly string _enablePinPath;
        private readonly IFileIo _fileIo;
        private readonly string _periodPinPath;

        private readonly string _polarityPinPath;

        private readonly string _sysfsPinNumber;
        private bool _exported;

        public PwmPin(int sysfsPinNumber, IFileIo fileIo)
        {
            _fileIo = fileIo;
            _sysfsPinNumber = sysfsPinNumber.ToString(CultureInfo.InvariantCulture);
            _enablePinPath = string.Format(EnablePath, _sysfsPinNumber);
            _dutyCyclePinPath = string.Format(DutyCyclePath, _sysfsPinNumber);
            _periodPinPath = string.Format(PeriodPath, _sysfsPinNumber);
            _polarityPinPath = string.Format(PolarityPath, _sysfsPinNumber);
        }

        public void Dispose()
        {
            if (_exported)
            {
                Enabled = false;
                _fileIo.WriteAllText(UnExportPath, _sysfsPinNumber);
            }
        }

        public int Period
        {
            get => GetInt(_periodPinPath);
            set => SetInt(value, _periodPinPath);
        }

        public int Polarity
        {
            get => GetInt(_polarityPinPath);
            set => SetInt(value, _polarityPinPath);
        }

        public int DutyCycle
        {
            get => GetInt(_dutyCyclePinPath);
            set => SetInt(value, _dutyCyclePinPath);
        }

        public bool Enabled
        {
            get
            {
                ExportPinIfRequired();
                return _fileIo.ReadAllText(_enablePinPath) == PinOn;
            }
            set
            {
                ExportPinIfRequired();
                _fileIo.WriteAllText(_enablePinPath, value ? PinOn : PinOff);
            }
        }

        private int GetInt(string path)
        {
            ExportPinIfRequired();
            return Convert.ToInt32(_fileIo.ReadAllText(path));
        }

        private void SetInt(int value, string path)
        {
            ExportPinIfRequired();
            _fileIo.WriteAllText(path, value.ToString(CultureInfo.InvariantCulture));
        }

        private void ExportPinIfRequired()
        {
            if (_exported) return;

            _fileIo.WriteAllText(ExportPath, _sysfsPinNumber);
            _exported = true;
        }
    }
}