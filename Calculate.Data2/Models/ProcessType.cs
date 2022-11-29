using System.ComponentModel.DataAnnotations.Schema;

namespace Calculate.Data.Models
{
    [Table("process_types")]
    public class ProcessType
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
