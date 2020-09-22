using AutoClaimInsuranceMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace AutoClaimInsuranceMVC.Controllers
{
    public class AssessorController : Controller
    {
        AutoClaimContext db = new AutoClaimContext();
        // GET: Assessor
        public ActionResult AssessorDetails(string reportId,string claimId)
        {
            if ((Session["officerId"] != null) && Session["role"].ToString() == "Assessor")
            {
                int reportID = int.Parse(reportId);
                int claimID = int.Parse(claimId);
                ViewBag.reportId = reportId;
                var claim = db.Claims.Where(c => c.claimId == claimID).FirstOrDefault();
                return View(claim);
            }
            else
            {
                return RedirectToAction("OfficerLogin", "Officer");
            }
        }
        [Authorize]
        public ActionResult ReportGenerate(string damage,string claimId, string reportId)
        {
            if ((Session["officerId"] != null) && Session["role"].ToString() == "Assessor")
            {
                int claimID = int.Parse(claimId);
                var claim = db.Claims.Where(c => c.claimId == claimID).FirstOrDefault();
                int reportID = int.Parse(reportId);
                var report = db.Reports.Where(r => r.reportId == reportID).FirstOrDefault();
                ViewBag.claimId = claimId;
                ViewBag.reportId = reportId;
                if (damage == "partial")
                    return View();
                else if (damage == "noDamage")
                {
                    claim.status = "evaluated";
                    db.Entry(claim).State = EntityState.Modified;
                    db.SaveChanges();
                    report.amount = 0;
                    report.content = "No damages found";
                    report.status = "completed";
                    db.Entry(report).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Assessorpage", "Officer");
                }
                else
                {
                    var insurance = db.Insurances.Where(i => i.policyNumber.Equals(claim.policyNumber)).FirstOrDefault();
                    string modelYear = insurance.model.ToString();
                    int position = modelYear.IndexOf("-");
                    modelYear = modelYear.Substring(position + 1);
                    int year = int.Parse(modelYear);
                    int presentYear = int.Parse(DateTime.Now.Year.ToString());
                    int yearDiffernce = presentYear - year;
                    double principalAmount = insurance.valueInsured;
                    for (int i = 1; i < yearDiffernce; i++)
                    {
                        principalAmount = principalAmount - (0.05 * principalAmount);
                    }
                    claim.status = "evaluated";
                    db.Entry(claim).State = EntityState.Modified;
                    db.SaveChanges();
                    report.amount = principalAmount;
                    report.content = "There is a total damage for the vehicle hence the claim is genuine and verified";
                    report.status = "completed";
                    db.Entry(report).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("AssessorPage", "Officer");
                }
            }
            else
            {
                return RedirectToAction("OfficerLogin", "Officer");
            }
           
        }
        public ActionResult CreateReport(string glasspercentage, string glassvalue, string plasticpercentage, string plasticvalue, string interiorpercentage, string interiorvalue, string outerpercentage, string outervalue, string claimId, string reportId)
        {
            if ((Session["officerId"] != null) && Session["role"].ToString() == "Assessor")
            {
                int claimID = int.Parse(claimId);
                var claim = db.Claims.Where(c => c.claimId == claimID).FirstOrDefault();
                var insurance = db.Insurances.Where(i => i.policyNumber.Equals(claim.policyNumber)).FirstOrDefault();
                string modelYear = insurance.model.ToString();
                int position = modelYear.IndexOf("-");
                modelYear = modelYear.Substring(position + 1);
                int year = int.Parse(modelYear);
                int presentYear = int.Parse(DateTime.Now.Year.ToString());
                int yearDiffernce = presentYear - year;
                double glassAmount = 0;
                double plasticAmount = double.Parse(plasticvalue);
                for (int i = 1; i < yearDiffernce; i++)
                {
                    plasticAmount = plasticAmount - (0.05 * plasticAmount);
                }

                double interiorAmount = double.Parse(interiorvalue);
                for (int i = 1; i < yearDiffernce; i++)
                {
                    interiorAmount = interiorAmount - (0.05 * interiorAmount);
                }

                double outerAmount = double.Parse(outervalue);
                for (int i = 1; i < yearDiffernce; i++)
                {
                    outerAmount = outerAmount - (0.05 * outerAmount);
                }

                plasticAmount = plasticAmount - ((int.Parse(plasticpercentage) / 100) * plasticAmount);
                interiorAmount = interiorAmount - ((int.Parse(interiorpercentage) / 100) * interiorAmount);
                outerAmount = outerAmount - ((int.Parse(outerpercentage) / 100) * outerAmount);

                double claimAmount = glassAmount + plasticAmount + interiorAmount + outerAmount;

                claim.status = "evaluated";
                db.Entry(claim).State = EntityState.Modified;
                db.SaveChanges();
                int reportID = int.Parse(reportId);
                var report = db.Reports.Where(r => r.reportId == reportID).FirstOrDefault();
                report.amount = claimAmount;
                report.content = "The damage for vehicle is Glass-" + glasspercentage + " Plastic-" + plasticpercentage + " Outer body-" + outerpercentage + " Interior-" + interiorpercentage + " The claim is genuine and verified";
                report.status = "completed";
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("AssessorPage", "Officer");
            }
            else
            {
                return RedirectToAction("OfficerLogin", "Officer");
            }
        }
        


    }
}