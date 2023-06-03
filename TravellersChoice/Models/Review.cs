using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TravellersChoice.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public string ReviewDesc { get; set; }

        public int ReviewRating { get; set; }
     
        public int PlaceId { get; set; }

        public string UserId { get; set; }

        [ForeignKey("PlaceId")]
        public Place Place { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}