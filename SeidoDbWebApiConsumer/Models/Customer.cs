using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeidoDbWebApiConsumer.Models
{
    public class Customer : ICustomer
    {
        #region for EFC CodeFirst
        [Key]
        [Column("CustomerID")]
        public Guid CustomerID { get; set; }

        public virtual List<Order> Orders { get; set; } = new List<Order>();
        #endregion

        [Column(TypeName = "nvarchar (200)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar (200)")]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar (200)")]
        public string Adress { get; set; }

        public int ZipCode { get; set; }

        [Column(TypeName = "nvarchar (200)")]
        public string City { get; set; }

        [Column(TypeName = "nvarchar (200)")]
        public string Country { get; set; }
 
        public DateTime BirthDate { get; set; }

        public decimal OrderValue { get => Orders.Sum(o => o.Value); }
        public override string ToString() => $"{CustomerID}: {FirstName} {LastName}, {Adress}, {ZipCode} {City}, {Country}";

        #region Implement IEquatable
        public bool Equals(ICustomer other) => CustomerID == other?.CustomerID;

        //Implement due to legacy reasons
        public override bool Equals(object obj) => Equals(obj as ICustomer);
        public override int GetHashCode() => CustomerID.GetHashCode();
        #endregion

        #region Class Factory for creating an instance filled with Random data
        public void RandomInit()
        {
            string[] _firstnames = "Fred John Mary Jane Oliver Marie Per Thomas Ann Susanne".Split(' ');
            string[] _lastnames = "Johnsson Pearsson Smith Ewans Andersson Svensson Shultz Perez".Split(' ');
            string[] _adresses = "Backvagen, Ringvagen, Box, Smith street, Graaf strasse, Vasagatan, Odenplan, Birger Jarlsgatan".Split(',');
            string[] _countries = "Sverige Norge Finland Lettland Tyskland Spanien".Split(' ');
            string[][] _cities = new string[_countries.Length][];
            _cities[0] = "Stockholm Malmo Gothenburg Gavle Linkoping Nykoping".Split();
            _cities[1] = "Oslo Tromso Haugesund Bergen".Split();
            _cities[2] = "Helsingfors Vaasa Oulu Tampere".Split();
            _cities[3] = "Riga Madona Daugavpils".Split();
            _cities[4] = "Dusseldorf Berlin Hannover Munich Hamburg".Split();
            _cities[5] = "Madrid, San Sebastian, Cordoba, Sevillia".Split();

            var rnd = new Random();
            bool bAllOK = false;
            while (!bAllOK)
            {
                try
                {
                    this.FirstName = _firstnames[rnd.Next(0, _firstnames.Length)];
                    this.LastName = _lastnames[rnd.Next(0, _lastnames.Length)];
                    this.Adress = _adresses[rnd.Next(0, _adresses.Length)].Trim() + " " + rnd.Next(1, 100);

                    this.ZipCode = rnd.Next(10000, 99999);

                    var _countryIdx = rnd.Next(0, _countries.Length);
                    this.City = _cities[_countryIdx][rnd.Next(0, _cities[_countryIdx].Length)];
                    this.Country = _countries[_countryIdx];

                    int year = rnd.Next(1940, DateTime.Today.Year - 20);
                    int month = rnd.Next(1, 13);
                    int day = rnd.Next(1, 31);

                    this.BirthDate = new DateTime(year, month, day);

                    bAllOK = true;
                }
                catch { }
            }
        }
        public static class Factory
        {
            public static Customer CreateRandom()
            {
                var p = new Customer();
                p.CustomerID = Guid.NewGuid();
                p.RandomInit();
                return p;
            }
        }
        #endregion

        public Customer() {}

        //Copy Constructor
        public Customer(ICustomer src)
        {
            FirstName = src.FirstName;
            LastName = src.LastName;
            Country = src.Country;

            BirthDate = src.BirthDate;
        }
    }
}
