/**
 * Author:    Jantzen Allphin
 * Partner:   None
 * Date:      October 23, 2020
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
 *    Contains C# code for the students controller of the web site representing the student applications.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using URC.Areas.Identity.Data;
using URC.Data;
using URC.Models;

namespace URC.Controllers
{
    /// <summary>
    /// This class represents the controller for the Students views.
    /// </summary>
    [Authorize(Roles = "Administrator,Professor,Student")]
    public class StudentsController : Controller
    {
        private readonly URC_Context _context;
        private readonly UserManager<URCUser> _userManager;

        /// <summary>
        /// Creates an StudentsController object.
        /// </summary>
        public StudentsController(URC_Context context, UserManager<URCUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns a list of all Student entities (including StudentSkills entity) to the Index page.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students
                .Include(o => o.StudentSkills)
                .AsNoTracking()
                .ToListAsync());
        }

        /// <summary>
        /// Returns the Search page view. 
        /// </summary> 
        [Authorize(Roles = "Administrator,Professor")]
        public IActionResult Search()
        {
            return View();
        }

        /// <summary>
        /// Returns the requested Student entity (including StudentSkills/CompletedCourses/Interests entities) to the Details page for the given Student id.
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(o => o.StudentSkills)
                .Include(o => o.CompletedCourses)
                .Include(o => o.Interests)
                .Include(o => o.RecommendedOpportunities)
                .ThenInclude(o => o.opportunity)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            var popStudentSkills = _context.PopularStudentSkills.OrderByDescending(o => o.count).Take(3);
            var popRequiredSkills = _context.PopularRequiredSkills.OrderByDescending(o => o.count).Take(3);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!User.IsInRole("Professor") && !User.IsInRole("Administrator") && student.Sid != user.Id)
                return View("../Shared/Permission_Denied");

            if (student == null)
            {
                return NotFound();
            }

            return View(new DetailsModel { student = student, popStudentSkills = popStudentSkills, popRequiredSkills = popRequiredSkills });
        }

        /// <summary>
        /// Model for displaying details on student profile, contains student and list of popular skills.
        /// </summary>
        public class DetailsModel
        {
            /// <summary>
            /// Holds the student model for display purposes.
            /// </summary>
            public Student student { get; set; }

            /// <summary>
            /// Holds the popular student skills.
            /// </summary>
            public IQueryable<PopularStudentSkill> popStudentSkills { get; set; }

            /// <summary>
            /// Holds the popular opportunity required skills.
            /// </summary>
            public IQueryable<PopularRequiredSkill> popRequiredSkills { get; set; }
        }

        /// <summary>
        /// Returns the Create page for a Student.
        /// </summary>
        [Authorize(Roles = "Student")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Creates a new Student entity using the giving bindings.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create([Bind("ID,Uid,StudentName,AvailableHoursPerWeek,DegreePlan,PersonalStatement,Resume,ExpectedGraduationDate,ProfileCreationDate,GPA,Active,Skills,FinishedCourses,StudentInterests")] Student student)
        {
            if (ModelState.IsValid)
            {
                // setting Sid for connection
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                student.Sid = user.Id;

                storeSkillsCoursesInterestsAsync(student);
                _context.Add(student);
                await _context.SaveChangesAsync();

                UpdateRecommendedOpportunities(student);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        /// <summary>
        /// Stores the StudentSkills/CompletedCourses/Interest for the given student in their respective tables.
        /// </summary>
        private void storeSkillsCoursesInterestsAsync(Student student)
        {
            // remove old properties
            var prevSkills = _context.StudentSkills.Where(s => s.StudentID == student.ID);
            var prevCompletedCourses = _context.CompletedCourses.Where(s => s.StudentID == student.ID);
            var prevInterests = _context.Interests.Where(s => s.StudentID == student.ID);
            _context.StudentSkills.RemoveRange(prevSkills);
            _context.CompletedCourses.RemoveRange(prevCompletedCourses);
            _context.Interests.RemoveRange(prevInterests);
            RemovePopularStudentSkills(prevSkills);

            // set new skills
            string[] skills = student.Skills.Split(", ");
            student.StudentSkills = new List<StudentSkill>();
            foreach (var skill in skills)
                student.StudentSkills.Add(new StudentSkill { SkillName = WebUtility.HtmlEncode(skill) });

            AddPopularStudentSkills(skills);

            // set new completedCourses
            string[] finishedCourses = student.FinishedCourses.Split(", ");
            student.CompletedCourses = new List<CompletedCourse>();
            foreach (var c in finishedCourses)
                student.CompletedCourses.Add(new CompletedCourse { CompletedCourseName = WebUtility.HtmlEncode(c) });

            // set new interests
            string[] studentInterests = student.StudentInterests.Split(", ");
            student.Interests = new List<Interest>();
            foreach (var si in studentInterests)
                student.Interests.Add(new Interest { InterestName = WebUtility.HtmlEncode(si) });
        }

        /// <summary>
        /// This method adds students skills to the PopularStudentSkill database table.
        /// </summary>
        /// <param name="skills"></param>
        private void AddPopularStudentSkills(string[] skills)
        {
            foreach (var skill in skills)
            {
                var popSkill = _context.PopularStudentSkills.Where(s => s.name == WebUtility.HtmlEncode(skill.ToUpper()));
                if (popSkill != null && popSkill.Count() > 0)
                {
                    var p = popSkill.First();
                    p.count++;
                    _context.PopularStudentSkills.Update(p);
                }
                else
                {
                    _context.PopularStudentSkills.Add(new PopularStudentSkill { count = 1, name = WebUtility.HtmlEncode(skill.ToUpper()) });
                }
            }
        }

        /// <summary>
        /// This method removes student skills from the PopularStudentSkill database table.
        /// </summary>
        /// <param name="prevSkills"></param>
        private void RemovePopularStudentSkills(IQueryable<StudentSkill> prevSkills)
        {
            foreach (var skill in prevSkills)
            {
                var popSkill = _context.PopularStudentSkills.Where(s => s.name == skill.SkillName.ToUpper());
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
                        _context.PopularStudentSkills.Remove(p);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the adding and removing of recommended opportunities for students based on skills.
        /// </summary>
        /// <param name="student"></param>
        private void UpdateRecommendedOpportunities(Student student)
        {
            var oldRecommendations = _context.Recommendations.Where(r => r.OpportunityId == student.ID);

            if (oldRecommendations != null)
                _context.Recommendations.RemoveRange(oldRecommendations);

            _context.SaveChanges();

            var studentSkills = _context.StudentSkills.Where(s => s.StudentID == student.ID);
            var completedCourses = _context.CompletedCourses.Where(s => s.StudentID == student.ID);
            var interests = _context.Interests.Where(s => s.StudentID == student.ID);
            var suitableOpportunities = _context.Opportunities.Where(o => o.RequiredSkills.All(s => studentSkills.Any(p => p.SkillName.ToUpper() == s.SkillName.ToUpper())));

            student.RecommendedOpportunities = new List<Recommendation>();

            foreach (var o in suitableOpportunities)
            {
                _context.Recommendations.Add(new Recommendation { opportunity = o, student = student });
            }
            _context.Update(student);
        }

        /// <summary>
        /// Returns the Edit page for the requested Student entity.
        /// </summary>
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);

            // verify student
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (student.Sid != user.Id)
                return View("../Shared/Permission_Denied");

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Edits an existing Student (application) entity using the given bindings.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Sid,Uid,StudentName,AvailableHoursPerWeek,DegreePlan,PersonalStatement,Resume,ExpectedGraduationDate,ProfileCreationDate,GPA,Active,Skills,FinishedCourses,StudentInterests")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            //  verify student
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (student.Sid != user.Id)
                return View("../Shared/Permission_Denied");

            if (ModelState.IsValid)
            {
                try
                {
                    storeSkillsCoursesInterestsAsync(student);
                    _context.Update(student);
                    await _context.SaveChangesAsync();

                    UpdateRecommendedOpportunities(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
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
            return View(student);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }

        /// <summary>
        /// HttpPost method to set status of Student (application) inactive/active.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Apply(int id, string toggle)
        {
            var studentApp = await _context.Students.FindAsync(id);

            if (WebUtility.HtmlEncode(toggle) == "apply")
            {
                studentApp.Active = true;
                _context.Update(studentApp);
                var result = await _context.SaveChangesAsync();
                if (result == 1)
                    return Ok(new { success = true, message = "set to active!" });
                else
                    return BadRequest(new { success = false, message = "bad request" });
            }
            else
            {
                studentApp.Active = false;
                _context.Update(studentApp);
                var result = await _context.SaveChangesAsync();
                if (result == 1)
                    return Ok(new { success = true, message = "set to inactive!" });
                else
                    return BadRequest(new { success = false, message = "bad request" });
            }
        }

        /// <summary>
        /// HttpPost method to find matching students with the given skills in message.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Professor")]
        public async Task<string> Find(string message)
        {
            HashSet<StudentSummaryObject> matched_students = new HashSet<StudentSummaryObject>();
            HashSet<int> exists = new HashSet<int>();
            string[] skills = WebUtility.HtmlEncode(message).Split(" ");
            foreach (var s in _context.StudentSkills.ToArray())
            {
                if (skills.Contains(s.SkillName.ToUpper()))
                {
                    var student = await _context.Students
                        .Include(o => o.StudentSkills)
                        .AsNoTracking()
                        .FirstAsync(stud => stud.ID == s.StudentID);

                    // don't allow duplicates
                    if (!exists.Contains(student.ID))
                    {
                        matched_students.Add(new StudentSummaryObject(student));
                        exists.Add(student.ID);
                    }
                }
            }

            var result = matched_students.ToArray();

            return JsonConvert.SerializeObject(result, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }
            );
        }
    }
}