namespace UtilityDelta.Gpio.Interfaces
{
    public interface IPinController
    {
        IPwmPin GetPwmPin(int pin);
        IGpioPin GetGpioPin(int pin);
    }
}