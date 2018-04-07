namespace UtilityDelta.Gpio.EventArgs
{
    public class PinChangedEventArgs
    {
        public bool Value { get; }

        public PinChangedEventArgs(bool value)
        {
            Value = value;
        }
    }
}