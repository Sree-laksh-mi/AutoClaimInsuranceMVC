﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
    public class NonRegisteredUserController : Controller
    {
        AutoClaimContext db = new AutoClaimContext();
        // GET: NonRegisteredUser
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index([Bind(Include = "insurerId,mailID,policyNumber,dateAndTime,policeCase,firNumber,licenseCopy,rcCopy")] Claim claim, HttpPostedFileBase fileLicense, HttpPostedFileBase fileRc)
        {
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

                if (claim.licenseCopy != null && claim.rcCopy != null)
                {
                    var Claim = new Claim()
                    {
                        insurerId = claim.insurerId,
                        MailID = claim.MailID,
                        policyNumber = claim.policyNumber,
                        dateAndTime = claim.dateAndTime,
                        policeCase = claim.policeCase,
                        firNumber = claim.firNumber,
                        licenseCopy = pathLicense,
                        rcCopy = pathRc,
                        status = "not claimed",
                        claimDate = DateTime.Now
                    };
                    db.Claims.Add(Claim);
                    db.SaveChanges();
                    return View();
                }
                else
                    return View();
            }
           
            
        }
   

    }