using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Calculate.Data.Models
{

    [Table("users")]
    public class User : IdentityUser<int>
    {
        [Column("id")]
        public override int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("mobile_phone")]
        public string MobilePhone { get; set; }

        [Column("role_id")]
        public int RoleID { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        [Column("token_access")]
        public string AccessToken { get; set; }

        [Column("last_signed_in_at")]
        public DateTime? LastSignedInAt { get; set; }

        [Column("email")]
        public override string Email { get; set; }

        [Column("password_hash")]
        public override string PasswordHash { get; set; }

        [NotMapped]
        public string RoleName { get; set; }

        //[JsonIgnore]
        //public ICollection<UserRight> UserRights { get; set; }
        //public User()
        //{
        //    UserRights = new List<UserRight>();
        //}
    }
}
