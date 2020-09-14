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

namespace AutoClaimInsuranceMVC.Controllers
{
    public class UserController : Controller
    {
        AutoClaimContext db = new AutoClaimContext();
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
            user.password = encrypt(user.password);
            user.confirmPassword = encrypt(user.confirmPassword);
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
            user.password = encrypt(user.password);
            var userobject = db.registeredUsers.Where(a => a.userId.Equals(user.userId) && a.password.Equals(user.password)).FirstOrDefault();
            if (userobject != null)
            {
                FormsAuthentication.SetAuthCookie(user.userId, false);
                Session["UserId"] = userobject.userId.ToString();
            }
            else
            {
                ModelState.AddModelError("", "Invalid login credentials");
            }

            return View(user);
        }
        public static string encrypt(string clearText)
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
    }
}