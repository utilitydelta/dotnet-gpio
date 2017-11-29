using UtilityDelta.Gpio.Enums;

namespace UtilityDelta.Gpio.Interfaces
{
    /// <summary>
    ///     https://www.kernel.org/doc/Documentation/pwm.txt
    /// </summary>
    public interface IPwmPin
    {
        /// <summary>
        ///     The total period of the PWM signal (read/write).
        ///     Value is in nanoseconds and is the sum of the active and inactive
        ///     time of the PWM.
        /// </summary>
        int Period { get; set; }

        /// <summary>
        ///     Changes the polarity of the PWM signal (read/write).
        ///     Writes to this property only work if the PWM chip supports changing
        ///     the polarity. The polarity can only be changed if the PWM is not
        ///     enabled. Value is the string "normal" or "inversed".
        /// </summary>
        PwmPolarity Polarity { get; set; }

        /// <summary>
        ///     The active time of the PWM signal (read/write).
        ///     Value is in nanoseconds and must be less than the period.
        /// </summary>
        int DutyCycle { get; set; }

        /// <summary>
        ///     Enable/disable the PWM signal (read/write).
        ///     - 0 - disabled
        ///     - 1 - enabled
        /// </summary>
        bool Enabled { get; set; }
    }
}