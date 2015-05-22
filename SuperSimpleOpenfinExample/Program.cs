using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Openfin.Desktop;

namespace SuperSimpleOpenfinExample
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenFinDesktopApiExample ofExample = new OpenFinDesktopApiExample();

            ofExample.Start();

            Console.ReadLine();
        }
    }
}
