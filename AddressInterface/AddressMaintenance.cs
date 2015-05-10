using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressInterface
{
    public class AddressMaintenance
    {
        public void addAddress ()
        {
            using (var db = new AddressContext())
            {
                AppCore_Address address = new AppCore_Address();
                address.AddressLine1 = "Address Line 1";
                address.AddressLine2 = "";
                address.City = "Seattle";
                address.Company = "Test";
                var query = from st in db.AppCore_State 
                            where st.Abbreviation == "WA"
                            select st;
                var state = query.FirstOrDefault<AppCore_State>();
                address.AppCore_State = state;
                db.AppCore_Address.Add(address);
                db.SaveChanges();
            }
        }
    }
}
