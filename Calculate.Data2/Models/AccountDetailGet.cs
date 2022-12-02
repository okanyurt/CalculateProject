namespace Calculate.Data.Models
{
    public class AccountDetailGet
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int BankId { get; set; }
        public string? IbanNumber { get; set; }
        public string? BankAccountNumber { get; set; }
        public bool IsEnable { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string? AccountName { get; set; }

        public string? BankName { get; set; }
    }
}
