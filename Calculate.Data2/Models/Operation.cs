﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Calculate.Data.Models
{
    [Table("operations")]
    public class Operation
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("process_number")]
        public int ProcessNumber { get; set; }

        [Column("account_id")]
        public int AccountId { get; set; }
        
        [Column("account_detail_id")]
        public int AccountDetailId { get; set; }

        [Column("process_type_id")]
        public int ProcessTypeId { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("process_price")]
        public decimal ProcessPrice { get; set; }

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

        [Column("case_id")]
        public int CaseId { get; set; }
    }
}
