﻿using AutoClaimInsuranceMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace AutoClaimInsuranceMVC.Controllers
{
    public class ClaimOfficerController : Controller
    {
        AutoClaimContext db = new AutoClaimContext();
        // GET: ClaimOfficer
        //[Authorize]
        [HttpGet]
        [Authorize]
        public ActionResult ClaimOfficerPage()
        {
            var progress = db.Claims.Where(p => p.status.Equals("progress")).Count();
            ViewBag.progress = progress;
            var evaluated = db.Claims.Where(e => e.status.Equals("evaluated")).Count();
            ViewBag.evaluated = evaluated;
            var claimed= db.Claims.Where(e => e.status.Equals("claimed")).Count();
            ViewBag.claimed = claimed;
            return View();
        }
        [Authorize]
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
        [Authorize]
        public ActionResult ClaimDetails(string claimId)
        {
            int claimid = int.Parse(claimId);
            var claimDetails = db.Claims.Where(c => c.claimId == claimid).FirstOrDefault();
            ViewBag.Report = db.Reports.Where(r => r.status.Equals("pending")).ToList();
            ViewBag.Assessor = db.Officers.Where(o => o.role.Equals("Assessor")).ToList();
            return View(claimDetails);


        }
        [HttpPost]
        [Authorize]
        public ActionResult Assign( string officerId,string claimId)
        {
            
                int claimID = int.Parse(claimId);
                Report report = new Report();
                report.officerId = officerId;
                report.claimId = claimID;
                report.status = "pending";
                report.content = "pending";
                report.reportDate = DateTime.Now;
                report.amount = 0.0;
                db.Reports.Add(report);
                db.SaveChanges();
                var claim = db.Claims.Where(c => c.claimId.Equals(report.claimId)).FirstOrDefault();
                claim.status = "assigned";
                db.Entry(claim).State = EntityState.Modified;
                db.SaveChanges();

            return RedirectToAction("ClaimProgress");
        }
        [HttpGet]
        [Authorize]
        public ActionResult CompletedReport()
        {
            var completedreport = db.Reports.Where(c => c.status.Equals("completed")).ToList();
            ViewBag.completedreport = completedreport;
            return View(completedreport);
        }
        
        [HttpGet]
        [Authorize]
        public ActionResult AcceptedClaim()
        {
            var acceptedclaim = db.Claims.Where(c => c.status.Equals("claimed")).ToList();
            ViewBag.acceptedclaim = acceptedclaim;
            return View(acceptedclaim);
        }
        [Authorize]
        public ActionResult AcceptedClaimDetails(string claimId)
        {
            int claimid = int.Parse(claimId);
            var acceptedClaimDetails = db.Claims.Where(c => c.claimId == claimid).FirstOrDefault();
            return View(acceptedClaimDetails);
        }


    }
}