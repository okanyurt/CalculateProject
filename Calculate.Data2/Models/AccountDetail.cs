using System.ComponentModel.DataAnnotations.Schema;

namespace Calculate.Data.Models
{
    [Table("account_details")]
    public class AccountDetail
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("account_id")]
        public int AccountId { get; set; }

        [Column("bank_id")]
        public int BankId { get; set; }

        [Column("iban_number")]
        public string IbanNumber { get; set; }


        [Column("bank_account_number")]
        public string BankAccountNumber { get; set; }

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
