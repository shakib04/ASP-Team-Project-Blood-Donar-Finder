using BloodDonationProject.Models;
using BloodDonationProject.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodDonationProject.Controllers
{
    public class DonorController : Controller
    {
        UserInfoRepository userInfoRepository = new UserInfoRepository();
        BloodDonationDBEntities12 context = new BloodDonationDBEntities12();

        [HttpGet]
        public ActionResult Index()
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            var email = Session["Email"].ToString();
            var data = context.userInfoes.Where(r => r.Email == email).FirstOrDefault<userInfo>();
            var userId = data.userID;
            Session["userid"] = userId;
            return View(context.userInfoes.Find(userId));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(userInfo userInfo, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                bool EmailAlreadyAdded = context.userInfoes.Any(x => x.Email == userInfo.Email);
                if (EmailAlreadyAdded)
                {
                    TempData["EmailExist"] = "This email is Already in use";
                }
                if (file != null)
                {
                    string path = null;
                    if (file.ContentType == "image/png" || file.ContentType == "image/jpeg" || file.ContentType == "image/jpg")
                    {
                        path = Path.Combine(Server.MapPath("~/Content/Images/"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        if (path != null)
                        {
                            file.SaveAs(path);
                            userInfo.ProPic = file.FileName;
                            userInfoRepository.Insert(userInfo);
                            return RedirectToAction("Login", "Home");

                        }
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit()
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            var email = Session["Email"].ToString();
            var data = context.userInfoes.Where(r => r.Email == email).FirstOrDefault<userInfo>();
            return View(context.userInfoes.Find(data.userID));
        }

        [HttpPost]
        public ActionResult Edit(userInfo user)
        {
            var userToEdit = context.userInfoes.Find(Session["userid"]);
            userToEdit.Name = user.Name;
            userToEdit.Password = user.Password;
            userToEdit.Email = user.Email;
            userToEdit.Phone = user.Phone;
            userToEdit.Address = user.Address;
            userToEdit.DOB = user.DOB;
            userToEdit.BloodGroup = user.BloodGroup;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChangeProPic()
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult ChageProPic(HttpPostedFileBase file)
        {
            var userToChange = context.userInfoes.Find(Session["userid"]);
            if (file != null)
            {
                string path = null;
                if (file.ContentType == "image/png" || file.ContentType == "image/jpeg" || file.ContentType == "image/jpg")
                {
                    path = Path.Combine(Server.MapPath("~/Content/Images/"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    if (path != null)
                    {
                        file.SaveAs(path);
                        userToChange.ProPic = file.FileName;
                        context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult ChangeDoc()
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangeDoc(HttpPostedFileBase file)
        {
            var userToChange = context.userInfoes.Find(Session["userid"]);
            if (file != null)
            {
                string path = null;
                if (file.ContentType == "application/pdf")
                {
                    path = Path.Combine(Server.MapPath("~/Content/Doc/"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    if (path != null)
                    {
                        file.SaveAs(path);
                        userToChange.Docoment = file.FileName;
                        context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.Onlypdf = "Only Pdf";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Report()
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            var email = Session["Email"].ToString();
            var data = context.userInfoes.Where(r => r.Email == email).FirstOrDefault<userInfo>();
            var userId = data.userID;
            var rating = context.donorRatings.Where(r => r.userID == userId).FirstOrDefault<donorRating>();
            if (rating != null)
            {
                return View(context.donorRatings.Find(rating.rateId));
            }
            return View();
        }

        [HttpGet]
        public ActionResult Report2()
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            var user = userInfoRepository.GetAll();
            return View(user);
        }

        [HttpGet]
        public ActionResult Post()
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            var email = Session["Email"].ToString();
            var data = context.userInfoes.Where(r => r.Email == email).FirstOrDefault<userInfo>();
            var bloodGroup = data.BloodGroup;
            var list = context.Posts.Where(r => r.WantedBlood == bloodGroup).ToList();
            list = list.OrderByDescending(c => c.Time).ToList();
            return View(list);
        }

        public ActionResult PostDetails(int id)
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            return View(context.Posts.Find(id));
        }

        [HttpGet]
        public ActionResult FlagThisPost(int id)
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            var postId = context.Posts.Find(id);

            var myFlags = context.FlagPosts.ToList().Where(x => x.userID == (int)Session["userid"]);
            var checkAlreadyFlaged = myFlags.Any(x => x.PostID == id);

            if (checkAlreadyFlaged)
            {
                return Content("Post has been already Flagged by You!!");
            }
            return View();
        }

        [HttpPost]
        public ActionResult FlagThisPost(int id, FlagPost fp)
        {
            fp.userID = (int)Session["userid"];
            fp.PostID = id;
            context.FlagPosts.Add(fp);
            context.SaveChanges();
            return RedirectToAction("Post");
        }

        public ActionResult BloodRequest()
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            var userId = Session["userid"];
            var list = context.RequestBloods.Where(i => i.DonarId == (int)userId).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult BloodRequestAccept(int id)
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            DonationReply req = new DonationReply();
            req.willDonate = "Accepted";
            req.RequestId = id;
            context.DonationReplies.Add(req);
            context.SaveChanges();
            return RedirectToAction("BloodRequest");
        }

        public ActionResult BloodRequestDecline(int id)
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            DonationReply req = new DonationReply();
            req.willDonate = "Declined";
            req.RequestId = id;
            context.DonationReplies.Add(req);
            context.SaveChanges();
            return RedirectToAction("BloodRequest");
        }

        [HttpGet]
        public ActionResult Users()
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            var data = context.userInfoes.Where(r => r.Type == "User").ToList();
            return View(data);
        }

        [HttpGet]
        public ActionResult UserPost(int id)
        {
            if (Session["Email"] == null || Session["Type"].ToString() != "Donner")
            {
                return RedirectToAction("Login", "Home");
            }
            var data = context.Posts.Where(r => r.UserId == id).ToList();
            return View(data);
        }
    }
}