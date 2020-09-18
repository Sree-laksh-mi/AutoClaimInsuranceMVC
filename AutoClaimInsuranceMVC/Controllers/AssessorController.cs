using AutoClaimInsuranceMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoClaimInsuranceMVC.Controllers
{
    public class AssessorController : Controller
    {
        AutoClaimContext db = new AutoClaimContext();
        // GET: Assessor
        public ActionResult AssessorDetails(string reportId,string claimId)
        {
            int reportID = int.Parse(reportId);
            int claimID = int.Parse(claimId);
            ViewBag.reportId = reportId;
            var claim = db.Claims.Where(c => c.claimId == claimID).FirstOrDefault();
            return View(claim);
        }
        [Authorize]
        public ActionResult GenerteReport(string claimId, string reportID)
        {

        }
    }
}