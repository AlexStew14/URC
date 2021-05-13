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
 *    Contains C# code modeling a RequiredSkill entity in the URC_Context database. "Code first" approach.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URC.Models
{
    /// <summary>
    /// Represents a RequiredSkill entity.
    /// </summary> 
    public class RequiredSkill
    {
        /// <summary>
        /// An int representing the RequiredSkill's ID.
        /// 
        /// Note: this is the primary key.
        /// </summary> 
        public int ID { get; set; }

        /// <summary>
        /// A string representing the RequiredSkill's name.
        /// </summary> 
        public string SkillName { get; set; }

        /// <summary>
        /// An int representing the RequiredSkill's OpportunityID.
        /// 
        /// Note: this is a foreign key.
        /// </summary> 
        public int OpportunityID { get; set; }


        /// <summary>
        /// An Opportunity object representing the RequiredSkill's Opportunity.
        /// 
        /// Note: this is a navigation property.
        /// </summary> 
        public Opportunity Opportunity { get; set; }
    }
}
