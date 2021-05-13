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
 *    Contains C# code modeling an Opportunity entity in the URC_Context database. "Code first" approach.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URC.Models
{
    /// <summary>
    /// Represents an Opportuntity entity.
    /// </summary>
    public class Opportunity
    {
        /// <summary>
        /// An int representing the Opportuntity's ID.
        ///
        /// Note: this is the primary key.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// A string representing the Opportunity's name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// A string representing the Opportunity's professor name.
        /// </summary>
        public string ProfessorName { get; set; }

        /// <summary>
        /// A string representing the Opportunity's project description.
        /// </summary>
        public string ProjectDescription { get; set; }

        /// <summary>
        /// A string representing the Opportunity's image (file name).
        /// </summary>
        public string ProjectImage { get; set; }

        /// <summary>
        /// A string representing the Opportunity's mentor name.
        /// </summary>
        public string ProjectMentor { get; set; }

        /// <summary>
        /// A DateTime object representing the Opportunity's begin date.
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// A DateTime object representing the Opportunity's end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// An int representing the Opportunity's pay.
        /// </summary>
        public int Pay { get; set; }

        /// <summary>
        /// A bool representing if the Opportunity is filled or not.
        /// </summary>
        public bool Filled { get; set; }

        /// <summary>
        /// A string used to gather RequiredSkills from form.
        /// </summary>
        public string Skills { get; set; }

        /// <summary>
        /// A collection of RequiredSkill objects representing the Opportunity's RequiredSkills.
        ///
        /// Note: this is a navigation property.
        /// </summary>
        public ICollection<RequiredSkill> RequiredSkills { get; set; }

        /// <summary>
        /// A collection of Tag objects representing the Opportunity's Tags.
        ///
        /// Note: this is a navigation property.
        /// </summary>
        public ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// A collection of Application objects representing the Opportunity's applications.
        ///
        /// Note: this is a navigation property.
        /// </summary>
        public ICollection<Application> Applications { get; set; }

        /// <summary>
        /// A collection of Student objects representing potentially suitable Students
        ///
        /// Note: this is a navigation property.
        /// </summary>
        public ICollection<Recommendation> RecommendedStudents { get; set; }
    }
}