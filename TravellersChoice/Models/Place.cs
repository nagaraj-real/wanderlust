using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using AutoMapper;
using System.Runtime.Serialization;

namespace TravellersChoice.Models
{
    
    public class Place
    {
        [Key]
        public int PlaceId { get; set; }

        public string PlaceName { get; set; }

        public string PlaceState { get; set;}

        public string PlaceCountry { get; set; }

        public string PlaceCity { get; set; }

        public string PlaceDescription { get; set; }

        public int PlaceAvgRating { get; set; }

        public string PlaceLatitude { get; set; }

        public string PlaceLongitude { get; set; }

        public string PlaceImageUrl { get; set; }

        public virtual ICollection<Review> Reviews { get; set;}

        public virtual ICollection<Image> Images { get; set;} 
    }

 
}