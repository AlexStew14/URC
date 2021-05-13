/*
 * Author:    Alex Stewart
 * Partner:   None
 * Date:      12/3/2020
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
 *    Contains C# code for the Recommendation model.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URC.Models
{
    /// <summary>
    /// This model represents the join table for the many-to-many relationship between
    /// students and opportunities.
    /// </summary>
    public class Recommendation
    {
        /// <summary>
        /// Primary key for Recommendation
        /// </summary>
        public int RecommendationId { get; set; }

        /// <summary>
        /// Foreign key for Opportunity
        /// </summary>
        public int OpportunityId { get; set; }

        /// <summary>
        /// Foreign key for Student
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Opportunity connected by this Recommendation.
        /// <note>This is a navigation property</note>
        /// </summary>
        public Opportunity opportunity { get; set; }

        /// <summary>
        /// Student connected by this Recommendation.
        /// <note>This is a navigation property</note>
        /// </summary>
        public Student student { get; set; }
    }
}