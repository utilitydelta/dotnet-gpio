namespace UtilityDelta.Gpio.Interfaces
{
    public interface IGpioPin
    {
        /// <summary>
        /// The first time you get or set this property will determine
        /// the pin direction ('out' - eg. controlling motors; and 'in' - eg. reading a digital sensor value)
        /// </summary>
        bool PinValue { get; set; }

        string GetValuePath();
        string GetDirectionPath();
    }
}