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
 *    Contains C# code modeling a Student entity in the URC_Context database. "Code first" approach.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace URC.Models
{
    /// <summary>
    /// Represents a Student entity.
    /// </summary>
    public class Student
    {
        /// <summary>
        /// An int representing the Students's ID.
        ///
        /// Note: this is the primary key.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// A string representing the Students's Uid.
        /// </summary>
        [RegularExpression(@"^u[0-9]{7}$", ErrorMessage = "Please enter uID in format: u1234567")]
        public string Uid { get; set; }

        /// <summary>
        /// An int representing the Students's available hours per week.
        /// </summary>
        [Range(0, 80)]
        public int AvailableHoursPerWeek { get; set; }

        /// <summary>
        /// A string representing the Students's degree plan (e.g. CS, CE, ECE, etc.).
        /// </summary>
        public string DegreePlan { get; set; }

        /// <summary>
        /// A string representing the Students's name.
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// A string representing the Students's ID associated with this application.
        /// </summary>
        public string Sid { get; set; }

        /// <summary>
        /// A string representing the Students's personal statement.
        /// </summary>
        [MaxLength(1000, ErrorMessage = "Please limit your statement to 1000 characters"), MinLength(10)]
        public string PersonalStatement { get; set; }

        /// <summary>
        /// A string used to gather StudentSkills from form.
        /// </summary>
        public string Skills { get; set; }

        /// <summary>
        /// A string used to gather CompletedCourses from form.
        /// </summary>
        public string FinishedCourses { get; set; }

        /// <summary>
        /// A string used to gather Interests from form.
        /// </summary>
        public string StudentInterests { get; set; }

        /// <summary>
        /// A DateTime object representing the Students's expected graduation date.
        /// </summary>
        public DateTime ExpectedGraduationDate { get; set; }

        /// <summary>
        /// A DateTime object representing the Opportunity's end date.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ProfileCreationDate { get; set; }

        /// <summary>
        /// An float representing the Students's GPA.
        /// </summary>
        [Range(0, 4)]
        public float GPA { get; set; }

        /// <summary>
        /// A bool representing if the Student is actively looking for a position or not.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// A collection of StudentSkill objects representing the Students's skills.
        ///
        /// Note: this is a navigation property.
        /// </summary>
        public ICollection<StudentSkill> StudentSkills { get; set; }

        /// <summary>
        /// A collection of Interest objects representing the STudnents's Interests.
        ///
        /// Note: this is a navigation property.
        /// </summary>
        public ICollection<Interest> Interests { get; set; }

        /// <summary>
        /// A collection of CompletedCourse objects representing the Students's completed courses.
        ///
        /// Note: this is a navigation property.
        /// </summary>
        public ICollection<CompletedCourse> CompletedCourses { get; set; }

        ///// <summary>
        ///// A collection of Application objects representing the Opportunity's applications.
        /////
        ///// Note: this is a navigation property.
        ///// </summary>
        //public ICollection<Application> Applications { get; set; }

        /// <summary>
        /// A collection of Opportunity objects representing recommended opportunities for the student.
        ///
        /// Note: this is a navigation property.
        /// </summary>
        public ICollection<Recommendation> RecommendedOpportunities { get; set; }
    }
}