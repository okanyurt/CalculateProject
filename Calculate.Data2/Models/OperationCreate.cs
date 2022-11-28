namespace Calculate.Data.Models
{
    public class OperationCreate
    {
        public int CaseId { get; set; }
        public int? ProcessNumber { get; set; }

        public int AccountId { get; set; }

        public int AccountDetailId { get; set; }

        public int ProcessTypeId { get; set; }

        public decimal Price { get; set; }

        public decimal ProcessPrice { get; set; }
    }
}
