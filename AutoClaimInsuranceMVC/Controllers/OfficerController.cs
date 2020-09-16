using AutoClaimInsuranceMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AutoClaimInsuranceMVC.Controllers
{
    public class OfficerController : Controller
    {
        AutoClaimContext db = new AutoClaimContext();
        // GET: Officer
        public ActionResult OfficerLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OfficerLogin(Officer officer)
        {
            officer.password =encrypt(officer.password);
            var user = db.Officers.Where(a => a.officerId.Equals(officer.officerId) && a.password.Equals(officer.password)).FirstOrDefault();
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(officer.officerId, false);
                Session["OfficerId"] = user.officerId.ToString();
                Session["Officername"] = (user.firstName + " " + user.lastName).ToString();
                Session["Role"] = user.role.ToString();
                if (user.role == "Insurance officer")
                    return RedirectToAction("InsuranceOfficerPage");
                else if (user.role == "Claim officer")
                    return RedirectToAction("ClaimOfficerPage");
                else
                    return RedirectToAction("Assessor", "Claim");

            }
            else
            {
                ModelState.AddModelError("", "Invalid login credentials");
            }

            return View(officer);
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

        [Authorize]
        [HttpPost]
        public ActionResult ClaimOfficerPage()
        {
            var claim = db.Claims.Where(c => c.status.Equals("claimed")).ToList();
            if (claim != null)
            {
                return View(claim);
            }
            else
            {
                ViewBag.Error = "Claim not exists";
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult InsuranceOfficerPage()
        {
            var claim = db.Claims.Where(c => c.status.Equals("not claimed")).ToList();
            if (claim !=null)
            {
                return View(claim);
            }
            else
            {
                ViewBag.Error = "Claim not exists";
            }
            return View();
        }























         




    }
}