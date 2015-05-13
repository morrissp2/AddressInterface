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
            // Start the Test Suite
            Console.WriteLine("Starting Test Suite");

            // Initialize the States Cache
            Console.WriteLine("Initializing States Cache");
            AddressMaintenance.Initialize();
            Console.WriteLine("Finshed Initialize");

            // Start the Find Address Test Case 
            Console.WriteLine("Start Run Test Case - Find Address at: " + DateTime.Now);
            testCaseFindAddress(50000);
            Console.WriteLine("End Run Test Case - Find Address at: "  + DateTime.Now);

            // Start the Add Address Test Case 
            Console.WriteLine("Start Run Test Case - Add Address at: " + DateTime.Now);
            testCaseAddAddress();
            Console.WriteLine("End Run Test Case - Add Address at: " + DateTime.Now);

            // Start the Ad Multiple Addresses Test Case
            Console.WriteLine("Start Run Test Case - Add Multiple Addresses at: " + DateTime.Now);
            testCaseAddMultipleAddreses(1000);
            Console.WriteLine("End Run Test Case - Add Multiple Addresses at: " + DateTime.Now);

            // End Test Suite
            Console.WriteLine("Finished Test Suite.  Press any key to Exit");
            Console.ReadLine();

        } // main

        /// <summary>
        /// This test case is used to test creating a single address
        /// </summary>
        static void testCaseAddAddress()
        {
            AddressMaintenance am = new AddressMaintenance();
            try
            {
                AddressRequest request = new AddressRequest();
                request.StateAbbreviation = "NE";
                request.Name = "Stephen";
                request.City = "Seattle";
                request.AddressLine1 = "Addres LIne 4";
                request.AddressLine2 = "Address LIne 56";
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
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
 
        }

        /// <summary>
        /// testCaseFindAddress is used to test the findAddres API
        /// </summary>
        /// <param name="addressId">holds the id of the addreses to find</param>
        static void testCaseFindAddress(int addressId)
        {
            AddressMaintenance am = new AddressMaintenance();
            AppCore_Address address = am.findAddress(addressId);
            if (address != null)
            {
                Console.WriteLine("Address Details");
                Console.WriteLine(address.Name);
                Console.WriteLine(address.AddressLine1);
                Console.WriteLine(address.AppCore_State.Abbreviation);
            }
        }

        /// <summary>
        /// This test case is used to create multiple addresses and report time
        /// </summary>
        /// <param name="count">holds the number of addreses to insert</param>
        static void testCaseAddMultipleAddreses(int count)
        {
            AddressMaintenance am = new AddressMaintenance();
            for ( int i = 0; i < count; i++)
            {
                AddressRequest a = new AddressRequest();
                a.AddressLine1 = "Address Line 1 " + i.ToString();
                a.AddressLine2 = "Address Line 2 " + i.ToString();
                a.City = "City " + i.ToString();
                a.Company = "Company " + i.ToString();
                a.Name = "Name " + i.ToString();
                a.ZipCode = "98001 " + i.ToString();
                a.StateAbbreviation = "WA";
                AddressResponse resp = am.addAddress(a);
                if (resp.Status != "Success")
                { 
                    Console.WriteLine("Exceptions:");
                    foreach (string e in resp.exceptions)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
 
        } // end testCaseAddMultipleAddresses

    }
}
