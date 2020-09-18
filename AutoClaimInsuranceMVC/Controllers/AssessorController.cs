using AutoClaimInsuranceMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

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
        public ActionResult ReportGenerate(string claimId, string reportId)
        {
            ViewBag.claimId = claimId;
            ViewBag.reportId = reportId;
            return View();
        }
        public ActionResult CreateReport(string glasspercentage,string plasticpercentage,string interiorpercentage,string outerpercentage,string AgeofVehicle,string claimId,string reportId)
        {
            int claimID = int.Parse(claimId);
            var claim = db.Claims.Where(c => c.claimId == claimID).FirstOrDefault();
            var insurance = db.Insurances.Where(i => i.policyNumber.Equals(claim.policyNumber)).FirstOrDefault();
            string modelYear = insurance.model.ToString();
            int position = modelYear.IndexOf("-");
            modelYear = modelYear.Substring(position+1);
            int year = int.Parse(modelYear);
            int presentYear = int.Parse(DateTime.Now.Year.ToString());
            int yearDiffernce = presentYear - year;
            double principalAmount = insurance.valueInsured;
            for(int i=1;i<yearDiffernce;i++)
            {
                principalAmount =principalAmount- (0.05 * principalAmount);
            }
            double claimAmount=0;



            
            claim.status = "evaluated";
            db.Entry(claim).State = EntityState.Modified;
            db.SaveChanges();
            int reportID = int.Parse(reportId);
            var report = db.Reports.Where(r => r.reportId == reportID).FirstOrDefault();
            if (claimAmount > 0)
                report.content = "Claim verified and genuine";
            else
                report.content = "Claim can be rejected ";
            report.status = "completed";

            return RedirectToAction("AssessorPage","Officer");
        }
    }
}