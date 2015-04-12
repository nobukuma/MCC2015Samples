using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace NetduinoPlusApplication1
{
    public class Program
    {
        public const int CountMax = 100;

        public static void Main()
        {
            OutputPort LED1 = new OutputPort(Pins.GPIO_PIN_D13, false);

            for (int count = 0; count < CountMax; count++)
            {
                Debug.Print("Hello " + count++);
                LED1.Write(true);
                Thread.Sleep(1500);
                LED1.Write(false);
                Thread.Sleep(1500);
            }
        }

    }
}
