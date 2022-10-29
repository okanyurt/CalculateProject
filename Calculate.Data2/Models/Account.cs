using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate.Data.Models
{
    [Table("accounts")]
    public class Account
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Column("identity_number")]
        public string IdentityNumber { get; set; }

        [Column("note")]
        public string Note { get; set; }

        [Column("is_enable")]
        public bool IsEnable { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_by")]
        public int UpdatedBy { get; set; }

        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; }
    }
}
