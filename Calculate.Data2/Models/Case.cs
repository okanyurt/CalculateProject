using System.ComponentModel.DataAnnotations.Schema;

namespace Calculate.Data.Models
{
    [Table("cases")]
    public class Case
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("office_id")]
        public int officeId { get; set; }

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

        [NotMapped]
        public string OfficeName { get; set; }
    }
}
