using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeidoDbWebApiConsumer.Models
{
    public class Order : IOrder
    {
        #region for EFC CodeFirst
        [Key]
        [Column("OrderID")]
        public Guid OrderID { get; set; }
        
        [ForeignKey(nameof(CustomerID))]
        [Column("CustomerID")]
        public Guid CustomerID { get; set; }

        //public virtual Customer Customer { get; set; }  //removed to avoid recursion in WebApi Json serialization
        #endregion

        public int NrOfArticles { get; set; }

        [Column(TypeName = "money")]
        public decimal Value { get; set; }

        [Column(TypeName = "money")]
        public decimal Freight { get; set; }
        public decimal Total => Value + Freight;
        public decimal VAT => Total * 0.8M;

        public DateTime OrderDate { get; private set; }
        public DateTime? DeliveryDate { get; set; }

        #region Implement IEquatable
        public bool Equals(IOrder other) => OrderID == other.OrderID;

        //Implement due to legacy reasons
        public override bool Equals(object obj) => Equals(obj as IOrder);
        public override int GetHashCode() => OrderID.GetHashCode();
        #endregion

        public override string ToString() => $"{OrderID}: Value: {Value:C2} OrderDate: {OrderDate:d} DeliverDate: {DeliveryDate:d} CustomerID: {CustomerID}";

        #region Class Factory for creating an instance filled with Random data
        public void RandomInit()
        {
            var rnd = new Random();
            bool bAllOK = false;
            while (!bAllOK)
            {
                try
                {
                    this.NrOfArticles = rnd.Next(1, 51);
                    this.Value = (decimal)(rnd.NextDouble() + 0.001D) * 5000;
                    this.Freight = (decimal)(rnd.NextDouble() + 0.001D) * 100;

                    int year = rnd.Next(2020, DateTime.Today.Year);
                    int month = rnd.Next(1, 13);
                    int day = rnd.Next(1, 31);

                    this.OrderDate = new DateTime(year, month, day);
                    this.DeliveryDate = this.OrderDate + new TimeSpan(rnd.Next(1, 31), 0, 0, 0);

                    bAllOK = true;
                }
                catch { }
            }
        }
        public static class Factory
        {
            public static Order CreateRandom(Guid CustomerID)
            {
                var p = new Order();
                p.CustomerID = CustomerID;
                p.OrderID = Guid.NewGuid();

                p.RandomInit();
                return p;
            }
        }
        #endregion

        public Order() {}

        //Copy Constructor
        public Order(IOrder src)
        {
            NrOfArticles = src.NrOfArticles;
            Value = src.Value;
            Freight = src.Freight;

            OrderDate = src.OrderDate;
            DeliveryDate = src.DeliveryDate;

            OrderID = src.OrderID;
            CustomerID = src.CustomerID;
        }
    }
}
