using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.Data.Models
{
    public class OperationUpdate
    {
        public int Id { get; set; }
        public int ProcessNumber { get; set; }

        public int AccountId { get; set; }

        public int AccountDetailId { get; set; }

        public int ProcessTypeId { get; set; }

        public decimal Price { get; set; }

        public decimal ProcessPrice { get; set; }
    }
}
