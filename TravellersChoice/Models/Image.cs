using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TravellersChoice.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }

        public string ImageUrl { get; set; }

        public int PlaceId { get; set; }

        public string UserId { get; set; }

        [ForeignKey("PlaceId")]
        public Place Place { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}