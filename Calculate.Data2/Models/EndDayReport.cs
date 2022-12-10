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
        public decimal TotalInboundTransfer { get; set; }
        public decimal TotalOutgoingTransfer { get; set; }

        public decimal TotalProcessPrice { get; set; }
        public string CaseName { get; set; }
    }
}
