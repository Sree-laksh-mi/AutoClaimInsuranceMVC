using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using AutoClaimInsuranceMVC.Models;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web.Security;
using System.Runtime.Remoting.Messaging;
using System.Data.Entity;
using System.Web.UI.WebControls;

namespace AutoClaimInsuranceMVC.Controllers
{
    public class UserController : Controller
    {
        AutoClaimContext db = new AutoClaimContext();

        public ActionResult HomePage()
        {
            return View();
        }

        // GET: User
        public ActionResult Register()
        {

            return View();
        }

        //Post: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "insurerId,userId,password,confirmPassword")] RegisteredUser user)
        {
            user.password = Encrypt(user.password);
            user.confirmPassword = Encrypt(user.confirmPassword);
            var check = db.registeredUsers.Find(user.userId);
            if (check == null)
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                db.registeredUsers.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "User already Exists");
                return View();
            }

        }

        public ActionResult Index()
        {
            return View();
        }

        // Post User Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RegisteredUser user)
        {
            user.password = Encrypt(user.password);
            var userobject = db.registeredUsers.Where(a => a.userId.Equals(user.userId) && a.password.Equals(user.password)).FirstOrDefault();
            if (userobject != null)
            {
                FormsAuthentication.SetAuthCookie(user.userId, false);
                Session["UserId"] = userobject.userId.ToString();
                return RedirectToAction("InsuranceView");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login credentials");
            }

            return View(user);
        }
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
       
        [Authorize]
        [HttpGet]
        public ActionResult InsuranceView()
        {

            string userId = (string)Session["userId"];
            var user = db.registeredUsers.Where(c => c.userId.Equals(userId)).FirstOrDefault();

            var insurances = db.Insurances.Where(c => c.insurerId.Equals(user.insurerId)).ToList();
            if (insurances != null)
            {
                return View(insurances);
            }
            else
            {
                ViewBag.Error = "No isurances";
            }
            return View();
        }
        [Authorize]
        public ActionResult Claim(string policyNumber)
        {
            Session["policyNumber"] = policyNumber;
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Claim([Bind(Include = "dateAndTime,policeCase,reason,licenseCopy,rcCopy")] Claim claim, HttpPostedFileBase fileLicense, HttpPostedFileBase fileRc)
        {

            string policyNumber = Session["policyNumber"].ToString();
            var insure = db.Insurances.Where(c => c.policyNumber.Equals(policyNumber)).FirstOrDefault();
            DateTime lastDate = insure.endDate;
            int value = DateTime.Compare(DateTime.Now, lastDate);
            if (value < 0)
            {
                string userId = (string)Session["userId"];
                var user = db.registeredUsers.Where(c => c.userId.Equals(userId)).FirstOrDefault();
                string insurerId = user.insurerId;
                string pathLicense = "";
                string pathRc = "";
                try
                {
                    if (fileLicense.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(fileLicense.FileName);
                        string FileExtension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                        if (FileExtension == "pdf")
                        {
                            fileName = claim.policyNumber + "Licence";
                            pathLicense = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                            fileLicense.SaveAs(pathLicense);
                            ViewBag.Message = "File Uploaded Successfully!!";
                            claim.licenseCopy = pathLicense;
                        }
                        else
                        {
                            ViewBag.Message = "Select a PDF file";
                            claim.licenseCopy = null;
                        }
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "File Upload Failed");
                }

                if (fileRc.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(fileRc.FileName);
                    string FileExtension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                    if (FileExtension == "pdf")
                    {
                        fileName = claim.policyNumber + "RC";
                        pathRc = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                        fileRc.SaveAs(pathRc);
                        ViewBag.Message_two = "File Uploaded Successfully!!";
                        claim.rcCopy = pathRc;
                    }
                    else
                    {
                        ViewBag.Message_two = "Select a PDF file";
                        claim.rcCopy = null;
                    }
                }

                else
                {
                    ModelState.AddModelError("", "File Upload Failed");
                }
                var check = db.Claims.Where(c => c.policyNumber.Equals(claim.policyNumber)).FirstOrDefault();
                if (check == null)
                {

                    if (claim.licenseCopy != null && claim.rcCopy != null)
                    {
                        var Claim = new Claim()
                        {
                            insurerId = insurerId,
                            MailID = userId,
                            policyNumber = policyNumber,
                            dateAndTime = claim.dateAndTime,
                            policeCase = claim.policeCase,
                            reason = claim.reason,
                            licenseCopy = pathLicense,
                            rcCopy = pathRc,
                            status = "progress",
                            claimDate = DateTime.Now
                        };
                        db.Claims.Add(Claim);
                        db.SaveChanges();
                        return View();
                    }
                   
                    
                }
                
                else
                {
                    ViewBag.check = "Failed";
                    return RedirectToAction("Index");
                }

            }
            else
                ModelState.AddModelError("", "Sorry your insurance date has expired");

            return View();
            
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
 }
