using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.Data.Models
{
    [Table("operations")]
    public class Operation
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("islem_no")]
        public int IslemNo { get; set; }

        [Column("hesap_adi")]
        public string HesapAdi { get; set; }

        [Column("banka_adi")]
        public string BankaAdi { get; set; }

        [Column("islem_tipi")]
        public string IslemTipi { get; set; }

        [Column("miktar")]
        public decimal Miktar { get; set; }
    }
}
