using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeidoDbWebApiConsumer.Models
{
    public interface IRandomInit
    {
        /// <summary>
        /// Initilize the instance to random values
        /// </summary>
        void RandomInit();
    }
}
