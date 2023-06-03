using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using TravellersChoice.Models;
using TravellersChoice.Helper;
using WebGrease.Css.Extensions;
using Image = TravellersChoice.Models.Image;
using System.Net.Http.Formatting;

namespace TravellersChoice.Controllers
{
    public class HomeApiController : ApiController
    {
        
        private readonly TravellerContext _db = new TravellerContext();
       private readonly ApplicationDbContext _adb=new ApplicationDbContext();
        readonly int _topdestinationindex;
        readonly int _noofrandom;
        HomeApiController() 
        {
            _db.Configuration.ProxyCreationEnabled = false;
            _topdestinationindex = 3; _noofrandom = 4;
        }




        [System.Web.Http.HttpGet]
        public IHttpActionResult Place(string id = null)
        {
        
            if (id != null && _db.Place.ToList().Any(p => p.PlaceName == id.Trim()))
            {
                var place = _db.Place.ToList().First(p => p.PlaceName == id.Trim());
                place.Reviews = _db.Review.Where(p => p.PlaceId == place.PlaceId).ToList();
                place.Images = _db.Image.Where(p => p.PlaceId == place.PlaceId).ToList();

                place.Reviews.ForEach(p =>
                {
                    p.User = _db.Profile.FirstOrDefault(i=>i.UserId==p.UserId);
                    p.User.UserProfile = _adb.Users.Find(p.UserId);
                });
                place.Images.ForEach(p =>
                {
                    p.User = _db.Profile.FirstOrDefault(i => i.UserId == p.UserId);
                    p.User.UserProfile = _adb.Users.Find(p.UserId);
                });

                return Ok(place);
            }
            else
            {
                return Ok(_db.Place.FirstOrDefault());
            }
            
           
        }

        [System.Web.Http.HttpGet]
        public IEnumerable<Place> GetRandomPlaces()
        {
            var placedto = _db.Place.ToList().SkipWhile((p1, p2) => p2 == _topdestinationindex).Take(_noofrandom).ToList();
            return placedto;
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult GetTopPlace()
        {
            var placedto = _db.Place.ToList().Where(p => p.PlaceId == _topdestinationindex);
            return Ok(placedto);
        }


        


        [System.Web.Http.HttpPost]
        public IHttpActionResult AddReview(Review rev)
        {
          

            try
            {
                _db.Review.Add(rev);             
                _db.SaveChanges();
                var placeobj = _db.Place.Find(rev.PlaceId);
                var ratingsum = _db.Review.Where(item => item.PlaceId == rev.PlaceId).Sum(item => item.ReviewRating);
                var totalcount = _db.Review.Count(item => item.PlaceId == rev.PlaceId);
                placeobj.PlaceAvgRating = Convert.ToInt32(ratingsum / totalcount);
                _db.SaveChanges();
                rev.User = _db.Profile.FirstOrDefault(i => i.UserId == rev.UserId);
                rev.User.UserProfile = _adb.Users.Find(rev.UserId);
                return Ok(rev);
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult AddUser(User usr)
        {


            try
            {
                var userid = "";
                usr.UserProfile.EmailConfirmed = false;
                usr.UserProfile.PhoneNumberConfirmed = false;
                usr.UserProfile.LockoutEnabled = false;
                usr.UserProfile.TwoFactorEnabled = false;
                usr.UserProfile.AccessFailedCount = 0;
                if (_adb.Users.Count(p => p.Email == usr.UserProfile.Email) == 0)
                {
                    _adb.Users.Add(usr.UserProfile);
                    _adb.SaveChanges();
                    userid = usr.UserProfile.Id;
                 
                }
                else
                {
                    userid = _adb.Users.FirstOrDefault(p => p.Email == usr.UserProfile.Email).Id;
                }
                usr.UserId = userid;
                if (_db.Profile.Count(p => p.UserId == usr.UserId) == 0)
                {
                    usr.UserProfile = null;
                    _db.Profile.Add(usr);

                    _db.SaveChanges();
                }
               
                usr = _db.Profile.FirstOrDefault(p => p.UserId == usr.UserId);
                
     
                return Ok(usr);
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult Profile(string id)
        {
            var userid = id;
            var user = _db.Profile.Find(userid);
            user.UserProfile = _adb.Users.Find(userid);
            user.Images = _db.Image.Where(p => p.UserId == userid).ToList();
            user.Reviews = _db.Review.Where(p => p.UserId == userid).ToList();
            user.Images.ForEach(p =>
                p.Place = _db.Place.FirstOrDefault(p1 => p1.PlaceId == p.PlaceId)
                );
            user.Reviews.ForEach(p =>
                p.Place = _db.Place.FirstOrDefault(p1 => p1.PlaceId == p.PlaceId));

            return Ok(user);
        }



      

        [System.Web.Http.HttpGet]
        public IHttpActionResult AutoCompletePlaces(string term)
        {
            var result = _db.Place.Where(p => (p.PlaceName + p.PlaceState + p.PlaceCountry).ToLower().Contains(term.ToLower())).ToList();


            return Json(new
            {
                Result = (from p in result select new { PlaceText = p.PlaceName + "," + p.PlaceState + "," + p.PlaceCountry, PlaceValue = p.PlaceName })

            });
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult Upload()
        {
            var req=HttpContext.Current.Request;
            HttpPostedFile file=null;
            string placename = "";
            string placeid = "";
            string userid = "";

            if(req.Files["recFile"]!=null)
            {
             file = HttpContext.Current.Request.Files["recFile"];
            }

            if (req.Form["PlaceName"] != null && req.Form["PlaceId"] != null && req.Form["UserId"] != null)
            {
                placename = req.Form["PlaceName"].ToString();
                placeid = req.Form["PlaceId"].ToString();
                userid = req.Form["UserId"].ToString();

            }


            if (file == null || string.IsNullOrEmpty(placename) || string.IsNullOrEmpty(placeid))
                return null;
            try
            {
                int plcid = Convert.ToInt32(placeid);
                var maxcount = _db.Image.Count(p => p.PlaceId == plcid);
                if (file != null && file.ContentLength > 0)
                {

                    var fileName = placename + "_" + (maxcount + 1) + Path.GetExtension(file.FileName);
                    var serverpath = "~/Images/Places/" + placename + "/";
                    var path = Path.Combine(HttpContext.Current.Server.MapPath(serverpath), fileName);
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(serverpath));
                    file.SaveAs(path);
                    var img = new Image();
                    img.ImageUrl = serverpath + fileName;
                    img.ImageUrl = img.ImageUrl.Replace("~", "");
                    img.UserId = userid;
                    img.PlaceId = Convert.ToInt32(placeid);
                    _db.Image.Add(img);
                    _db.SaveChanges();
                    img.User = _db.Profile.FirstOrDefault(i => i.UserId == img.UserId);
                    img.User.UserProfile = _adb.Users.Find(img.UserId);
                    return Ok(img);
                }
            }
            catch
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

    }


       
}