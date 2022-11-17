namespace Calculate.Data.Models
{
    public class EndDayReport
    {
        public int TotalProcessNumber { get; set; }
        public decimal TotalPayMoney { get; set; }
        public decimal TotalTransfer { get; set; }
        public decimal TotalWithdraw { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal TotalBalance { get; set; }
        public string CaseName { get; set; }
    }
}
