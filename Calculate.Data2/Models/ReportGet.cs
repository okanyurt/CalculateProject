using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.Data.Models
{
    public class ReportGet
    {  
        public int Id { get; set; }
        public string Account { get; set; }
        public string AccountDetail { get; set; }
        public string ProcessType { get; set; }
        public decimal Price { get; set; }
    }
}
