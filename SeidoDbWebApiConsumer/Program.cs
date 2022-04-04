using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SeidoDbWebApiConsumer.Models;
using SeidoDbWebApiConsumer.Services;

using NecklaceModels;

namespace SeidoDbWebApiConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new SeidoDbHttpService(new Uri("https://localhost:44340"));
            //var service = new SeidoDbHttpService(new Uri("http://localhost:5000"));
            //var service = new SeidoDbHttpService(new Uri("https://ws6.seido.se"));
       
            QueryDatabaseAsync(service).Wait();
        }

        private static async Task QueryDatabaseAsync(SeidoDbHttpService service)
        {
            Console.WriteLine("Query Database");
            Console.WriteLine("--------------");

            Console.WriteLine("Testing GetNecklacesAsync()");
            var necklaces = await service.GetNecklacesAsync();
            Console.WriteLine($"Nr of Necklaces: {necklaces.Count()}");
            Console.WriteLine($"First 10 Necklaces:");
            necklaces.Take(10).ToList().ForEach(c => Console.WriteLine(c));

            Console.WriteLine("\nTesting GetNecklaceAsync()");
            Console.WriteLine("Read the first necklace using Int");
            var neck1 = await service.GetNecklaceAsync(necklaces.First().NecklaceID);
            Console.WriteLine(neck1);

            Console.WriteLine("\nTesting CreateNecklaceAsync()");
            //Customer NewCust1 = Customer.Factory.CreateRandom();
            Necklace NewNeck1 = Necklace.Factory.CreateRandomNecklace(25);
            //var NewCust2 = await service.CreateCustomerAsync(NewCust1);
            var NewNeck2 = await service.CreateNecklaceAsync(NewNeck1);
            Console.WriteLine("Created Necklace:");
            Console.WriteLine(NewNeck2);

            var NewNeck3 = await service.GetNecklaceAsync(NewNeck2.NecklaceID);
            // NewNeck1 doesn't have an ID yet as it hasn't been inserted and given an ID
            // Our models library also doesn't implement IEquatable so a simple check will do for now rather than recompile
            if (NewNeck2.NecklaceID == NewNeck3.NecklaceID)
                Console.WriteLine("Readback necklace Equal");
             else
                Console.WriteLine("ERROR: Readback necklace not equal");


            Console.WriteLine($"\nTesting UpdateNecklaceAsync() by setting Pearl 0 (currently {NewNeck1.Pearls[0].Size}) to 25.");
            int tmpSize = NewNeck2.Pearls[0].Size;
            NewNeck2.Pearls[0].Size = 25;
            var UpdatedNecklace1 = await service.UpdateNecklaceAsync(NewNeck2);
            Console.WriteLine($"Updated necklace with new size for pearl 0.\nSize: {UpdatedNecklace1.Pearls[0].Size}.");
            Console.WriteLine($"Restoring pearl 0 to {tmpSize}..");
            NewNeck2.Pearls[0].Size = tmpSize;
            var UpdatedNecklace2 = await service.UpdateNecklaceAsync(NewNeck2);

            if (NewNeck2.Pearls[0].Size == UpdatedNecklace2.Pearls[0].Size)
                Console.WriteLine("Success!");
            else
                Console.WriteLine("ERROR: Failed to restore pearl to original state.");

            Console.WriteLine("\nTesting DeleteCustomerAsync()");
            var DelNeck1 = await service.DeleteNecklaceAsync(NewNeck2.NecklaceID);

            Console.WriteLine($"Necklace to delete.\n{NewNeck2}");
            Console.WriteLine($"Deleted necklace.\n{DelNeck1}");

            if (DelNeck1 != null && DelNeck1.NecklaceID == NewNeck2.NecklaceID)
                Console.WriteLine("Necklace Equal");
            else
                Console.WriteLine("ERROR: Necklaces not equal");

            var DelNeck2 = await service.GetNecklaceAsync(DelNeck1.NecklaceID);
            if (DelNeck2 != null)
                Console.WriteLine("ERROR: Necklace not removed");
            else
                Console.WriteLine("Necklace confirmed removed from Db");

            Console.WriteLine();
        }
    }
}
