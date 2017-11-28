namespace UtilityDelta.Gpio.Interfaces
{
    public interface IPinMapper
    {
        int MapPinToSysfs(string pin);
    }
}