namespace Calculate.Data.Models
{
    public class OperationGet
    {
        public int Id { get; set; }
        public int? ProcessNumber { get; set; }
        public string Account { get; set; }
        public string AccountDetail { get; set; }
        public string ProcessType { get; set; }
        public decimal Price { get; set; }
        public decimal ProcessPrice { get; set; }
        public bool IsEnable { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string CaseName { get; set; }
    }
}
