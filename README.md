# dotnet-gpio
This is a simple .NET Standard library for working with GPIO on Linux devices. You can read sensor values, turn on leds, control a motor and more. Do you need stepper motor library? Check this out: https://github.com/utilitydelta-io/dotnet-stepper

## Usage
```c#

using System;
using UtilityDelta.Gpio.Implementation;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Let there be light!");
            
            using (var controller = new PinController(new FileIo(), new SysfsPinMapper()))
            {
                var pin136 = controller.GetGpioPin("136");
                pin136.PinValue = true;
                System.Threading.Thread.Sleep(5000);
                pin136.PinValue = false;
            }

            Console.WriteLine("We are finished.");
        }
    }
}

```

Everything is built against an interface. So hook it up to your favourite dependency injection framework and go nuts.

```c#

public class MyGpio
{
    private readonly IPinController m_pinController;

    public MyGpio(IPinController pinController)
    {
        m_pinController = pinController;
    }

    public void TurnOnLed()
    {
        m_pinController.GetGpioPin("130").PinValue = true;
    }
}

```


## Why this library?
- This is a pure .NET library with no external dependencies. 
- It's simple and easy to understand.
- Unit tests. 100% code coverage.
- Build using the IoC pattern. Dependency injection friendly.
- Commercial friendly licence.

## Pin Mapping
Many boards have their own pin numbering system that is different to the raw sysfs pins exposed in Linux. If you want to use a board specific numbering scheme, you can implement the IPinMapper interface. There is already one in this library for the C.H.I.P PRO which you can have a look at here: https://github.com/utilitydelta-io/dotnet-gpio/blob/master/UtilityDelta.Gpio/Implementation/ChipProPinMapper.cs
