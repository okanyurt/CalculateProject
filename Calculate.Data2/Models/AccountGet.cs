namespace Calculate.Data.Models
{
    public class AccountGet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string? IdentityNumber { get; set; }
        public string? Note { get; set; }
        public bool IsEnable { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CaseId { get; set; }
        public string CaseName { get; set; }      
    }
}
