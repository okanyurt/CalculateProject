using System.ComponentModel.DataAnnotations.Schema;

namespace Calculate.Data.Models
{
    public class AccountGetName
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
