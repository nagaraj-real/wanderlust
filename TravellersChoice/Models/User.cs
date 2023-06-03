using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravellersChoice.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser UserProfile { get; set; }

        public string ImgUser { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Image> Images { get; set; }

    }
}