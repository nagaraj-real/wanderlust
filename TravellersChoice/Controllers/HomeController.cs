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
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using TravellersChoice.Models;
using TravellersChoice.Helper;
using WebGrease.Css.Extensions;

namespace TravellersChoice.Controllers
{
    public class HomeController : Controller
    {
        
        private TravellerContext db = new TravellerContext();
       private ApplicationDbContext adb=new ApplicationDbContext();
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

       
        public ActionResult Place(string id)
        {
           

           
            if (id!=null && db.Place.ToList().Any(p => p.PlaceName == id.Trim()))
            {
                var place = db.Place.ToList().First(p => p.PlaceName == id.Trim());
                place.Reviews=db.Review.Where(p=>p.PlaceId==place.PlaceId).ToList();
                place.Images=db.Image.Where(p=>p.PlaceId==place.PlaceId).ToList();

                place.Reviews.ForEach(p =>
                {   p.User=new User();
                    p.User.UserProfile = adb.Users.Find(p.UserId);
                });
                place.Images.ForEach(p =>
                {
                    p.User = new User();
                    p.User.UserProfile = adb.Users.Find(p.UserId);
                });
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_PlacedetailsPartial", place);
                }
                else
                {
                    return View("Place", place);
                }
         
            }
            else
            {
                var placedto = db.Place.ToList();
                int topdestinationindex = 3; int noofrandom = 4;
                var item=new Place();
                if (topdestinationindex < placedto.Count)
                {
                    item = placedto[topdestinationindex];
                    placedto.RemoveAt(topdestinationindex);
                }
                placedto.Shuffle();
                placedto = placedto.SkipWhile((p1, p2) => p2 == topdestinationindex).Take(noofrandom).ToList();
                if (item!=null)
                {
                    placedto.Add(item);
                }
                return View("Places",placedto);
            }

        }

        [HttpPost]
        public PartialViewResult Upload()
        {
            var img = new Image();
            try
            {

               Place placemodel =new Place();

                placemodel.PlaceName = Request.Form["placename"];
                if (Request.Form["placeid"] != null)
                {
                    placemodel.PlaceId =Convert.ToInt32(Request.Form["placeid"]);
                }
                if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                var maxcount = db.Image.Count(p => p.PlaceId == placemodel.PlaceId);
               
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = placemodel.PlaceName + "_" + (maxcount + 1) + Path.GetExtension(file.FileName);
                    var serverpath = "~/Images/Places/" + placemodel.PlaceName + "/";
                    var path = Path.Combine(Server.MapPath(serverpath), fileName);
                    System.IO.Directory.CreateDirectory(Server.MapPath(serverpath));
                    file.SaveAs(path);
                    
                    img.ImageUrl = serverpath + fileName;
                    img.ImageUrl = img.ImageUrl.Replace("~", "");
                    img.UserId = User.Identity.GetUserId();
                    img.PlaceId = placemodel.PlaceId;
                    db.Image.Add(img);
                    db.SaveChanges();
                    img.User=new User();
                    img.User.UserProfile = adb.Users.Find(img.UserId);
                }
            }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return PartialView("_ImagePartial", img);
        }


        [HttpPost]
        public PartialViewResult AddReview(Place placemodel)
        {
            Review rev = new Review();
         
           

            try
            {
                var rating = ValueProvider.GetValue("reviewrating").AttemptedValue;
                var comments = ValueProvider.GetValue("reviewtext").AttemptedValue;
                var placeid = placemodel.PlaceId;
                rev.PlaceId =Convert.ToInt32(placeid);
                rev.ReviewRating = Convert.ToInt32(rating);
                rev.ReviewDesc = comments;
                rev.UserId = User.Identity.GetUserId();
                db.Review.Add(rev);             
                db.SaveChanges();
                var placeobj = db.Place.Find(placemodel.PlaceId);
                var ratingsum = db.Review.Where(item => item.PlaceId == placemodel.PlaceId).Sum(item => item.ReviewRating);
                var totalcount = db.Review.Count(item => item.PlaceId == placemodel.PlaceId);
                placeobj.PlaceAvgRating = Convert.ToInt32(ratingsum / totalcount);
                db.SaveChanges();
                rev.User =new User();
                rev.User.UserProfile = adb.Users.Find(rev.UserId);
               
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return PartialView("_ReviewPartial",rev);
        }

        public ActionResult Profile(string id)
        {
            var userid = adb.Users.First(p => p.UserName == id).Id;
            var user = db.Profile.Find(userid);
            user.UserProfile = adb.Users.Find(userid);
            user.Images = db.Image.Where(p => p.UserId == userid).ToList();
            user.Reviews = db.Review.Where(p => p.UserId == userid).ToList();
            user.Images.ForEach(p =>
                p.Place = db.Place.FirstOrDefault(p1 => p1.PlaceId == p.PlaceId)
                );
            user.Reviews.ForEach(p =>
                p.Place = db.Place.FirstOrDefault(p1 => p1.PlaceId == p.PlaceId)
                );
            ViewBag.Title = "Profile";
            return View("Profile",user);
        }

        [HttpPost]
        public JsonResult UploadProfilePic()
        {
            User usermodel = new User();
            try
            {
                usermodel = db.Profile.Find(User.Identity.GetUserId());
                
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = "ProfilePic" + Path.GetExtension(file.FileName);
                        var serverpath = "~/Images/Users/" + User.Identity.GetUserName() + "/";
                        var path = Path.Combine(Server.MapPath(serverpath), fileName);
                        System.IO.Directory.CreateDirectory(Server.MapPath(serverpath));
                        file.SaveAs(path);

                        usermodel.ImgUser = serverpath + fileName;
                        usermodel.ImgUser = usermodel.ImgUser.Replace("~", "");
                        
                        db.SaveChanges();
      
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return Json(usermodel.ImgUser);
        }

        [HttpGet]
        public JsonResult AutoCompletePlaces(string term)
        {
            var result = db.Place.Where(p=>(p.PlaceName+p.PlaceState+p.PlaceCountry).ToLower().Contains(term.ToLower())).ToList();
   

            return Json(new
            {
                Result = (from p in result select new { PlaceText = p.PlaceName +","+p.PlaceState + ","+p.PlaceCountry,PlaceValue=p.PlaceName })
                
            }, JsonRequestBehavior.AllowGet);
        }
    }


       
}