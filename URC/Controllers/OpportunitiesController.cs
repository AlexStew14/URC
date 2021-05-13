/**
 * Author:    Jantzen Allphin
 * Partner:   None
 * Date:      September 23, 2020
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
 *    Contains C# code for the opportunities controller of the web site.
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
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using URC.Areas.Identity.Data;
using URC.Data;
using URC.Models;

namespace URC.Controllers
{
    /// <summary>
    /// This class represents the controller for the Opportunities views.
    /// </summary>
    [Authorize(Roles = "Administrator,Professor,Student")]
    public class OpportunitiesController : Controller
    {
        private readonly URC_Context _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Creates an OpportunitiesController object.
        /// </summary>
        /// <param name="context">The context (database) to be used.</param>
        public OpportunitiesController(URC_Context context, IConfiguration configuration, UserManager<URCUser> userManager)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Returns a list of all Opportunity entities (including Tags entity) to the Index page.
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Opportunities
                .Include(o => o.Tags)
                .AsNoTracking()
                .ToListAsync());
        }

        /// <summary>
        /// Returns a list of all Opportunity entities to the List page.
        /// </summary>
        [Authorize(Roles = "Administrator,Professor")]
        public async Task<IActionResult> List()
        {
            return View(await _context.Opportunities.ToListAsync());
        }

        /// <summary>
        /// Returns the requested Opportunity entity to the Details page.
        /// </summary>
        [Authorize(Roles = "Administrator,Professor,Student")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities
                .Include(opp => opp.Tags)
                .Include(opp => opp.RequiredSkills)
                .Include(opp => opp.Applications)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (opportunity == null)
            {
                return NotFound();
            }

            return View(opportunity);
        }

        /// <summary>
        /// Returns the requested Opportunity entity to the Manage page.
        /// </summary>
        [Authorize(Roles = "Administrator,Professor")]
        public async Task<IActionResult> Manage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities
                .Include(opp => opp.Applications)
                .FirstOrDefaultAsync(m => m.ID == id);

            // only let the owner manage the opportunity -> hack (should use ID)
            if (opportunity.ProfessorName != User.Identity.Name && !User.IsInRole("Administrator"))
                return View("../Shared/Permission_Denied");

            if (opportunity == null)
            {
                return NotFound();
            }

            return View(opportunity);
        }

        /// <summary>
        /// Returns the Create page for an Opportunity.
        /// </summary>
        [Authorize(Roles = "Administrator,Professor")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Adds a new Opportunity entity using the given bindings.
        ///
        /// Note:
        /// To protect from overposting attacks, enable the specific properties you want to bind to, for
        /// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="opportunity">The new Opportunity to be added.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Professor")]
        public async Task<IActionResult> Create([Bind("ID,ProjectName,ProfessorName,ProjectDescription,ProjectImage,ProjectMentor,BeginDate,EndDate,Pay,Filled,Skills")] Opportunity opportunity)
        {
            if (ModelState.IsValid)
            {
                HandleRequiredSkills(opportunity);
                RemovePopularRequiredSkills(opportunity);
                AddPopularRequiredSkills(opportunity);
                UpdateSuitableStudentsRecommendations(opportunity);

                _context.Add(opportunity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(opportunity);
        }

        /// <summary>
        /// Whenever an opportunity is created, students that have the skills should be recommended for it.
        /// This gets suitable students and creates a recommendation for them.
        /// </summary>
        private void UpdateSuitableStudentsRecommendations(Opportunity opportunity)
        {
            var students = _context.Students.Where(s => s.StudentSkills.Count() >= opportunity.RequiredSkills.Count()).Include(s => s.StudentSkills).ToList();

            var suitableStudents = students.Where(s => opportunity.RequiredSkills.All(o => s.StudentSkills.Any(ss => ss.SkillName.ToUpper() == o.SkillName.ToUpper())));

            if (suitableStudents != null)
            {
                foreach (var s in suitableStudents)
                {
                    _context.Recommendations.Add(new Recommendation { opportunity = opportunity, student = s });
                }
            }
        }

        /// <summary>
        /// Handles the addition of opportunity required skills to PopularRequiredSkills.
        /// Used for displaying popular required skills to students.
        /// </summary>
        private void AddPopularRequiredSkills(Opportunity opportunity)
        {
            string[] skills = opportunity.Skills.Split(", ");

            foreach (var skill in skills)
            {
                var popSkill = _context.PopularRequiredSkills.Where(s => s.name == skill.ToUpper());
                if (popSkill != null && popSkill.Count() > 0)
                {
                    var p = popSkill.First();
                    p.count++;
                    _context.PopularRequiredSkills.Update(p);
                }
                else
                {
                    _context.PopularRequiredSkills.Add(new PopularRequiredSkill { count = 1, name = skill.ToUpper() });
                }
            }
        }

        /// <summary>
        /// Handles the removal of required tags from the database.
        /// </summary>
        private void RemovePopularRequiredSkills(Opportunity opportunity)
        {
            var prevSkills = _context.RequiredSkills.Where(o => o.OpportunityID == opportunity.ID);

            foreach (var skill in prevSkills)
            {
                var popSkill = _context.PopularRequiredSkills.Where(s => s.name == skill.SkillName.ToUpper());
                if (popSkill != null && popSkill.Count() > 0)
                {
                    var p = popSkill.First();
                    if (p.count > 1)
                    {
                        p.count -= 1;
                        _context.Update(p);
                    }
                    else
                    {
                        _context.PopularRequiredSkills.Remove(p);
                    }
                }
            }
        }

        /// <summary>
        /// This method handles the removal of old required skills and the adding of new ones.
        /// </summary>
        private void HandleRequiredSkills(Opportunity opportunity)
        {
            var prevSkills = _context.RequiredSkills.Where(s => s.OpportunityID == opportunity.ID);
            _context.RequiredSkills.RemoveRange(prevSkills);
            string[] skills = opportunity.Skills.Split(", ");
            opportunity.RequiredSkills = new List<RequiredSkill>();
            foreach (var skill in skills)
                opportunity.RequiredSkills.Add(new RequiredSkill { SkillName = skill });
        }

        /// <summary>
        /// Returns the Edit page for the requested Opportunity entity.
        /// </summary>
        /// <param name="id">The Opportunity ID to be used.</param>
        [Authorize(Roles = "Administrator,Professor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Opportunity opp = await _context.Opportunities.Where(o => o.ID == id).FirstOrDefaultAsync();

            // only let the owner edit the opportunity -> TODO: hack
            if (opp.ProfessorName != User.Identity.Name && !User.IsInRole("Administrator"))
                return View("../Shared/Permission_Denied");

            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity == null)
            {
                return NotFound();
            }
            return View(opportunity);
        }

        /// <summary>
        /// Edits an existing Opportunity entity using the given bindings.
        ///
        /// Note:
        /// To protect from overposting attacks, enable the specific properties you want to bind to, for
        /// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="opportunity">The new Opportunity to be edited.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Professor")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ProjectName,ProfessorName,ProjectDescription,ProjectImage,ProjectMentor,BeginDate,EndDate,Pay,Filled")] Opportunity opportunity)
        {
            if (id != opportunity.ID)
            {
                return NotFound();
            }

            // only let the owner edit the opportunity -> TODO: hack
            if (opportunity.ProfessorName != User.Identity.Name && !User.IsInRole("Administrator"))
                return View("../Shared/Permission_Denied");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(opportunity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpportunityExists(opportunity.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(List));
            }
            return View(opportunity);
        }

        /// <summary>
        /// Returns the Delete page for the requested Opportunity entity.
        /// </summary>
        /// <param name="id">The Opportunity ID to be used.</param>
        [Authorize(Roles = "Administrator,Professor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities
                .FirstOrDefaultAsync(m => m.ID == id);
            if (opportunity == null)
            {
                return NotFound();
            }

            return View(opportunity);
        }

        /// <summary>
        /// Deletes the Opportunity entity using the given ID.
        ///
        /// Note:
        /// To protect from overposting attacks, enable the specific properties you want to bind to, for
        /// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// </summary>
        /// <param name="id">The Opportunity ID to be used.</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Professor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            var recStudents = _context.Recommendations.Where(o => o.StudentId == opportunity.ID);
            if (recStudents != null)
                _context.Recommendations.RemoveRange(recStudents);
            RemovePopularRequiredSkills(opportunity);

            _context.Opportunities.Remove(opportunity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        /// <summary>
        /// HttpPost method to upload a file from the create opp page.
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
                string[] permittedExtensions = { ".jpeg", ".jpg", ".png" };


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
                        return BadRequest(new { message = "File need to be .jpg, .jpeg, .png." });
                    }

                    Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
                        {
                            { ".jpeg", new List<byte[]>
                                {
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                                }
                            },
                            { ".jpg", new List<byte[]>
                                {
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                                }
                            },
                            { ".png", new List<byte[]>
                                {
                                    new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
                                }
                            },
                        };

                    using (var reader = new BinaryReader(formFile.OpenReadStream()))
                    {
                        var signatures = _fileSignature[ext];
                        var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                        if (!signatures.Any(signature =>
                             headerBytes.Take(signature.Length).SequenceEqual(signature)))
                        {
                            return BadRequest(new { message = "File need to be a image file which is jpeg, jpg, png not a disguise as image file." });
                        }
                    }

                    new_name = string.Format("{0}.{1}", WebUtility.HtmlEncode(formFile.FileName).Replace(".", string.Empty), ext);
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
        /// Returns true if the given Opportunity ID exists, false otherwise.
        /// </summary>
        /// <param name="id">The Opportunity ID to be used.</param>
        private bool OpportunityExists(int id)
        {
            return _context.Opportunities.Any(e => e.ID == id);
        }
    }
}