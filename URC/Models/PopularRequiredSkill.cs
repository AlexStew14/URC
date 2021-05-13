/*
 * Author:    Alex Stewart
 * Partner:   None
 * Date:      12/4/2020
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Alex Stewart - This work may not be copied for use in Academic Coursework.
 *
 * I, Alex Stewart, certify that I wrote this code (below) from scratch and did
 * not copy it in part or whole from another source.  Any references used
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *
 *    Contains C# code for the PopularRequiredSkill model.
 */

namespace URC.Models
{
    /// <summary>
    /// This model stores the frequency of a particular required skill.
    /// Used for displaying the most opportunity skills.
    /// </summary>
    public class PopularRequiredSkill
    {
        /// <summary>
        /// Primary key for PopularRequiredSkill
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The count of opportunities that have this skill
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// The name of the skill, all caps for normalization
        /// </summary>
        public string name { get; set; }
    }
}