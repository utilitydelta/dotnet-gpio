namespace UtilityDelta.Gpio.Interfaces
{
    public interface IPinMapper
    {
        int MapPinToSysfs(int pin);
    }
}