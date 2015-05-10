using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressInterface;

namespace TestAddressConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            AddressMaintenance am = new AddressMaintenance();
            am.addAddress();
            Console.WriteLine("FInishing");

            Console.ReadLine();

        }
    }
}
