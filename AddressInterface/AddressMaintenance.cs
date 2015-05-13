using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;

namespace AddressInterface
{

    /// <summary>
    /// Address Request is an object that will hold onto the data needed to create an address - just a plain old
    /// CLR Object.  Will be used to send data into the Service interface
    /// </summary>
    public class AddressRequest
    {
        public string AddressLine1 { get; set;}
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string StateAbbreviation { get; set; }
    }

    /// <summary>
    /// Response class used to return the ID of the address created or a list of validation errors or exceptions generated
    /// </summary>
    public class AddressResponse
    {
        public string Status { get; set; }   // "Success", "Failure", "Validation Exception","No Record Found"
        public List<string> exceptions { get; set;}

        // address primary details
        public int id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        
        //state primary details
        public string state_Abbreviation { get; set;  }
        public string state_Name { get; set;  }

        public AddressResponse()
        {
            exceptions = new List<string>();
        }
    }

    
    /// <summary>
    /// The Address Maintenance class is responsible for adding, deleting, updating and searching for adresses.
    /// </summary>
    public class AddressMaintenance
    {
        /// <summary>
        /// States variable is prepopulated once the Initialize method is called to cache the States fron the database
        /// </summary>
        public static List<AppCore_State> States =  null;
        
        /// <summary>
        /// The Initialize Method is used to retrive the list of States form the database and cached for use
        /// </summary>
        public static void Initialize()
        {
            if (States == null)
            {
                using (var db = new AddressContext())
                {
                    States = (from st in db.AppCore_State
                              select st).ToList<AppCore_State>();
                }
            }
        }

        /// <summary>
        /// Reset can be called to refresh the list of states. 
        /// </summary>
        public static void Reset()
        {
            States = null;
            Initialize();
        }

        /// <summary>
        /// This is the default method to add an address.  It takes all the parameters as strings 
        /// and uses them to create an instance of an address to persist to the database
        /// </summary>
        /// <param name="addressLine1">Line 1 of an address</param>
        /// <param name="addressLine2">Line 2 of an address</param>
        /// <param name="city">Associated City</param>
        /// <param name="company">Company of the address</param>
        /// <param name="zipCode">Zip Code for the addres</param>
        /// <param name="name">Name of the Address</param>
        /// <param name="stateAbbreviation">State Abreviation - this is used as the lookup into the States Table</param>
        /// <returns>AddressResponse which contains any errors/validation messages or Id of address added</returns>
        public AddressResponse addAddress(
            string addressLine1, string addressLine2, string city, 
            string company, string zipCode, string name, string stateAbbreviation 
            )
        {
            AddressResponse response = new AddressResponse();
            try
            {
                using (var db = new AddressContext())
                {
                    AppCore_Address address = new AppCore_Address();
                    address.AddressLine1 = addressLine1;
                    address.AddressLine2 = addressLine2;
                    address.City = city;
                    address.Company = company;
                    address.Zip = zipCode;
                    address.Name = name;
                    //var query = from st in States 
                    //            where st.Abbreviation == "WA"
                    //            select st;
                    //var state = query.FirstOrDefault<AppCore_State>();
                    //var state = States.Find(p => p.Abbreviation == "WA");
                    address.AppCore_State = db.AppCore_State.Where(st => st.Abbreviation == stateAbbreviation).First();
                    db.AppCore_Address.Add(address);
                    db.SaveChanges();
                    response.Status = "Success";
                    response.id = address.Id;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var entityValidationErrors in e.EntityValidationErrors)
                {
                    string exceptionString = "Entity of Type " + entityValidationErrors.Entry.Entity.GetType().Name + " in state " + entityValidationErrors.Entry.State + " has the following validation errors:";
                    foreach (var validationErrors in entityValidationErrors.ValidationErrors)
                    {
                        exceptionString = exceptionString + "\n";
                        exceptionString = exceptionString + " - Property: " + validationErrors.PropertyName + " Error: " + validationErrors.ErrorMessage;
                    }
                    response.exceptions.Add(exceptionString);
                    response.Status = "Validation Errors";
                    return response;
                }
            }
            catch (Exception e)
            {
                response.exceptions.Add("Exception occurred: " + e.Message);
                response.Status = "Failure";
                return response;
            }
            return response;
        }

        /// <summary>
        /// This addAddress method takes in an AddressRequest which contains the details for the address to be added
        /// This is utilizing a Request/Response pattern
        /// </summary>
        /// <param name="request">an instance of AddressRequest that contains address details</param>
        /// <returns>AddressResponse which contains any errors/validation messages or Id of address added</returns>
        public AddressResponse addAddress(AddressRequest request)
        {
            AddressResponse response = new AddressResponse();

            // first check to see if have valid state abbreviation
            if (validateState(request.StateAbbreviation))
            {
                try
                {
                    using (var db = new AddressContext())
                    {
                        AppCore_Address address = new AppCore_Address();
                        address.AddressLine1 = request.AddressLine1;
                        address.AddressLine2 = request.AddressLine2;
                        address.City = request.City;
                        address.Company = request.Company;
                        address.Zip = request.ZipCode;
                        address.Name = request.Name;
                        //var query = from st in States 
                        //            where st.Abbreviation == "WA"
                        //            select st;
                        //var state = query.FirstOrDefault<AppCore_State>();
                        //var state = States.Find(p => p.Abbreviation == "WA");
                        address.AppCore_State = db.AppCore_State.Where(st => st.Abbreviation == request.StateAbbreviation).First();
                        db.AppCore_Address.Add(address);
                        db.SaveChanges();
                        response.id = address.Id;
                        response.Status = "Success";
                    }

                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        string exceptionString = "Entity of Type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                        foreach (var ve in eve.ValidationErrors)
                        {
                            exceptionString = exceptionString + "\n";
                            exceptionString = exceptionString + " - Property: " + ve.PropertyName + " Error: " + ve.ErrorMessage;
                        }
                        response.exceptions.Add(exceptionString);
                        response.Status = "Validation Errors";
                        return response;
                    }
                    //throw;
                }
                catch (Exception e)
                {
                    response.exceptions.Add("Exception occurred: " + e.Message);
                    response.Status = "Failure";
                    return response;
                }
            }
            else
            {
                response.exceptions.Add("Invalid state passed in: " + request.StateAbbreviation);
                response.Status = "Validation Errors";
            }
            return response;
        }

        /// <summary>
        /// This addAdress override is used if you have an instance of an address already.
        /// </summary>
        /// <param name="address">a fully populated addres entity including state relationship</param>
        public void addAddres( AppCore_Address address)
        {
            using (var db = new AddressContext())
            {
                db.AppCore_Address.Add(address);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// findAdress is used to return an instance of an address based on the id that is passed in.  Will return a fully
        /// populated address entity with associated State object
        /// </summary>
        /// <param name="id">integer representing the id of the address record that is to be fetched</param>
        /// <returns>AddressResponse which contains any errors/validation messages or details of address added</returns>
        public AddressResponse findAddress( int id)
        {
            AddressResponse response = new AddressResponse();
            try
            {
                using (var db = new AddressContext())
                {
                    var query = from add in db.AppCore_Address.Include("AppCore_State")
                                where add.Id == id
                                select add;
                    var address = query.FirstOrDefault<AppCore_Address>();
                    if (address != null)
                    {
                        response.Status = "Success";
                        response.Name = address.Name;
                        response.id = address.Id;
                        response.AddressLine1 = address.AddressLine1;
                        response.AddressLine2 = address.AddressLine2;
                        response.City = address.City;
                        response.Company = address.Company;
                        response.ZipCode = address.Zip;
                        response.state_Abbreviation = address.AppCore_State.Abbreviation;
                        response.state_Name = address.AppCore_State.Name;
                    }
                    else
                    {
                        response.Status = "No Record Found";
                        response.exceptions.Add("No record found with id: " + id.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                response.exceptions.Add("Exception occurred: " + e.Message);
                response.Status = "Failure";
                return response;
            }
            return response;
        }

        /// <summary>
        /// validateState takes in a State abreviation and looks in the States class variable to determine if a valid state 
        /// was passed in
        /// </summary>
        /// <param name="stateAbrev"></param>
        /// <returns>true if valid state abbreviation is passed in false if not</returns>
        private bool validateState ( string stateAbrev )
        {
            if (States.Find(p => p.Abbreviation == stateAbrev) == null)
                return false;
            else
                return true;
        }
    }
}
