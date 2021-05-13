/**
 * Author:    Jantzen Allphin
 * Partner:   None
 * Date:      December 6, 2020
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Jantzen Allphin - This work may not be copied for use in Academic Coursework.
 *
 * I, Jantzen Allphin, certify that I wrote this code (below) from scratch and did
 * not copy it in part or whole from another source.  Any references used
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *
 *    Contains C# code for the Application controller.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using URC.Areas.Identity.Data;
using URC.Data;
using URC.Models;

namespace URC.Controllers
{
    /// <summary>
    /// This class represents the controller for the Application views.
    /// </summary>
    public class ApplicationsController : Controller
    {
        private readonly URC_Context _context;
        private readonly UserManager<URCUser> _userManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Creates an Appplication object.
        /// </summary>
        public ApplicationsController(URC_Context context, UserManager<URCUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Returns a list of all Application entities for the given User to the Index page.
        /// </summary>
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            // only show applications by this user
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var uRC_Context = _context.Application
                .Include(a => a.Opportunity)
                .Where(a => a.StudentID == user.Id);
            return View(await uRC_Context.ToListAsync());
        }

        /// <summary>
        /// Returns the requested Application entity to the Details page.
        /// </summary>
        [Authorize(Roles = "Professor,Student,Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Application
                .Include(a => a.Opportunity)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (application == null)
            {
                return NotFound();
            }

            // include the student model
            var student = await _context.Students
                .Include(s => s.StudentSkills)
                .Include(s => s.CompletedCourses)
                .Include(s => s.Interests)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Sid == application.StudentID);
            ViewData["Student"] = student;

            return View(application);
        }

        /// <summary>
        /// Returns the Create page for an Application.
        /// </summary>
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create(int oppId, string oppName)
        {
            // validate that this student has a profile
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var profile = await _context.Students.FirstOrDefaultAsync(s => s.Sid == user.Id);
            if (profile == null)
                return View("No_Profile");

            ViewData["OppId"] = oppId;
            ViewData["OppName"] = WebUtility.HtmlEncode(oppName);
            return View();
        }

        /// <summary>
        /// Adds a new Application entity using the given bindings.
        ///
        /// Note:
        /// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        /// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Statement,Resume,OpportunityID,Status")] Application application)
        {
            if (ModelState.IsValid)
            {
                // set user
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Sid == user.Id);
                application.StudentID = student.Sid;

                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // restore viewdata
            var opp = _context.Opportunities.Find(application.OpportunityID);
            ViewData["OppId"] = opp.ID;
            ViewData["OppName"] = opp.ProjectName;
            return View(application);
        }

        /// <summary>
        /// Returns the Edit page for the requested Application entity.
        /// </summary>
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Application
                .Include(a => a.Opportunity)
                .FirstOrDefaultAsync(a => a.ID == id);

            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        /// <summary>
        /// Edits an existing Application entity using the given bindings.
        ///
        /// Note:
        /// To protect from overposting attacks, enable the specific properties you want to bind to, for
        /// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Statement,Resume,OpportunityID,StudentID,Status")] Application application)
        {
            if (id != application.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(application);
        }

        /// <summary>
        /// Returns the Delete page for the requested Application entity.
        /// </summary>
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Application
                .Include(a => a.Opportunity)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        /// <summary>
        /// Deletes the Application entity using the given ID.
        ///
        /// Note:
        /// To protect from overposting attacks, enable the specific properties you want to bind to, for
        /// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.Application.FindAsync(id);
            _context.Application.Remove(application);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Returns true if an Application entity with the given id exists.
        /// </summary>
        private bool ApplicationExists(int id)
        {
            return _context.Application.Any(e => e.ID == id);
        }

        /// <summary>
        /// Downloads the given filename from the system.
        /// </summary>
        [HttpGet]
        public ActionResult Download(string filename)
        {
            string fullFileName = Path.Combine(_configuration["StoreFilePath"], WebUtility.HtmlEncode(filename));
            byte[] fileBytes = GetFile(fullFileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        /// <summary>
        /// Prepares the file for download.
        /// </summary>
        // Thanks for the awesome advice on downloading files in MVC: https://www.c-sharpcorner.com/article/file-upload-and-download-using-asp-net-mvc-5-for-beginners/
        byte[] GetFile(string s)
        {
            FileStream fs = System.IO.File.OpenRead(WebUtility.HtmlEncode(s));
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new IOException(s);
            return data;
        }

        /// <summary>
        /// HttpPost method to upload a file from the create application page.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> FileUpload(List<IFormFile> files)
        {
            try
            {
                if (files.Count != 1)
                {
                    return BadRequest(new { message = "Must Submit One File" });
                }

                long size = files.Sum(f => f.Length);
                var new_name = "";
                var filePaths = new List<string>();
                string[] permittedExtensions = {".pdf" };


                foreach (var formFile in files)
                {
                    if (formFile.Length >= 1000000)
                    {
                        return BadRequest(new { message = "File too large. Expect less than 10M" });
                    }

                    if (formFile.Length <= 0)
                    {
                        return BadRequest(new { message = "File empty." });
                    }
                    
                    var ext = Path.GetExtension(WebUtility.HtmlEncode(formFile.FileName)).ToLowerInvariant();

                    if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                    {
                        return BadRequest(new { message = "File need to be .pdf ." });
                    }

                    Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
                        {
                            { ".pdf", new List<byte[]>
                                {
                                    new byte[] { 0x25, 0x50, 0x44, 0x46 },
                                }
                            },
                        };

                    using (var reader = new BinaryReader(formFile.OpenReadStream()))
                    {
                        var signatures = _fileSignature[ext];
                        var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                        if(!signatures.Any(signature =>
                            headerBytes.Take(signature.Length).SequenceEqual(signature)))
                        {
                            return BadRequest(new { message = "File need to be a pdf file not a disguise as pdf file." });
                        }
                    }

                    new_name = string.Format("{0}.{1}", WebUtility.HtmlEncode(formFile.FileName).Replace(".", string.Empty),ext);
                    var filePath = Path.Combine(_configuration["StoreFilePath"], new_name);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                }

                return Ok(new { message = new_name, count = files.Count, size, filePaths });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Failed to upload file: " + e.Message });
            }
        }

        /// <summary>
        /// Changes the role for the given user_id. Based on the value of add_remove,
        ///     it will either remove or add the given role to the user.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AcceptDeny(int app_id, string accept_deny)
        {
            var application = await _context.Application.FindAsync(app_id);
            if (application == null)
                return BadRequest(new { success = false, message = "bad request" });

            try
            {
                if (WebUtility.HtmlEncode(accept_deny) == "accept")
                {
                    application.Status = "accepted";
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = "accepted!" });
                }
                else
                {
                    application.Status = "denied";
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = "denied!" });
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(application.ID))
                {
                    return BadRequest(new { success = false, message = "bad request" });
                }
                else
                {
                    throw;
                }
            }
        }



    }
}
