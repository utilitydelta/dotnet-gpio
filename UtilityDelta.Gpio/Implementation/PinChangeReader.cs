using System;
using System.IO;
using UtilityDelta.Gpio.EventArgs;
using UtilityDelta.Gpio.Interfaces;

namespace UtilityDelta.Gpio.Implementation
{
    public class PinChangeReader : IPinChangeReader, IDisposable
    {
        private FileSystemWatcher _valueChangedFileSystemWatcher;
        private FileSystemWatcher _directionChangedFileSystemWatcher;
        private IGpioPin _pin;

        public event EventHandler<PinChangedEventArgs> GpioChanged;

        public PinChangeReader()
        {
            _valueChangedFileSystemWatcher = new FileSystemWatcher();
            _directionChangedFileSystemWatcher = new FileSystemWatcher();
        }

        public void Start(IGpioPin pin)
        {
            _pin = pin ?? throw new Exception("Change reader needs to be supplied a GPIO pin");

            Watch(_valueChangedFileSystemWatcher, _pin.GetValuePath());
            Watch(_directionChangedFileSystemWatcher, _pin.GetDirectionPath());
        }

        public void Stop()
        {
            Unwatch(_valueChangedFileSystemWatcher);
            Unwatch(_directionChangedFileSystemWatcher);
            _pin = null;
        }

        private void Watch(FileSystemWatcher fileSystemWatcher, string filePath)
        {
            fileSystemWatcher.Path = filePath;
            fileSystemWatcher.Changed += OnGpioValueChanged;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void Unwatch(FileSystemWatcher fileSystemWatcher)
        {
            fileSystemWatcher.EnableRaisingEvents = false;
            fileSystemWatcher.Changed -= OnGpioValueChanged;
            fileSystemWatcher.Path = null;
        }

        private void OnGpioValueChanged(object sender, FileSystemEventArgs eventArgs)
        {
            GpioChanged?.Invoke(this, new PinChangedEventArgs(_pin.PinValue));
        }

        public void Dispose()
        {
            Stop();
        }
    }
}