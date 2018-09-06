using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waterful.Core.Models;

namespace Waterful.Back.ViewModels
{
    public class OrderSendVM
    {
        /// <summary>
        /// OrderId
        /// </summary>
        public int Id { get; set; }
        public List<Worker> Workers { get; set; }
    }
}
