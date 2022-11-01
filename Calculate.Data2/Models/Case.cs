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
    }
}
