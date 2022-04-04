﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeidoDbWebApiConsumer.Models
{
    public interface ICustomer : IEquatable<ICustomer>, IRandomInit
    {
        public Guid CustomerID { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
