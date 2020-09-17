using AutoClaimInsuranceMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoClaimInsuranceMVC.Controllers
{
    public class ClaimOfficerController : Controller
    {
        AutoClaimContext db = new AutoClaimContext();
        // GET: ClaimOfficer
        //[Authorize]
        [HttpGet]
        public ActionResult ClaimOfficerPage()
        {

            return View();
        }

        public ActionResult ClaimProgress()
        {
            var claim = db.Claims.Where(c => c.status.Equals("progress")).ToList();
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
        public ActionResult ClaimDetails(string claimId)
        {
            int claimid = int.Parse(claimId);
            var claimDetails = db.Claims.Where(c => c.claimId == claimid).FirstOrDefault();
            return View(claimDetails);


        }
    }
}