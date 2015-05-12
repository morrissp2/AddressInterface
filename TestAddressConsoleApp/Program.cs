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
            AddressMaintenance.Initialize();
            Console.WriteLine("Finshed Initialize");

            Console.WriteLine("Starting Add Address");
            AddressMaintenance am = new AddressMaintenance();
            Console.WriteLine("Time now: " + DateTime.Now);
//            am.addAddress("Address Line 1", "Address Line 2", "City", "Company", "98403", "Name", "NE" );
//            am.addAddress("", "", "", "", "98403", "Name", "NE");
            try
            {
                AddressRequest request = new AddressRequest();
                request.StateAbbreviation = "NE";
                request.Name = "Stephen";
                request.City = "Seattle";
                request.AddressLine1 = "";
                request.AddressLine2 = "";
                request.Company = "Test";
                request.ZipCode = "98403";

                AddressResponse response = am.addAddress(request);
                if (response.Status != "Success")
                {
                    Console.WriteLine("Exceptions:");
                    foreach (string e in response.exceptions)
                    {
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    Console.WriteLine("Address id = " + response.id.ToString());
                }
                Console.WriteLine("Time now: " + DateTime.Now);
                Console.WriteLine("FInishing create address");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            Console.ReadLine();
//            foreach (AppCore_State st in AddressMaintenance.States)
//            {
//                Console.WriteLine(st.Name + " ==== " + st.Abbreviation);
//            }

            AppCore_Address address = am.findAddress(1);

            Console.WriteLine("Address Details");
            Console.WriteLine(address.Name);
            Console.WriteLine(address.AddressLine1);
            Console.WriteLine(address.AppCore_State.Abbreviation);
            Console.ReadLine();
        }
    }
}
