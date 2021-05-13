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
 *    Contains C# code modeling a CompletedCourse entity in the URC_Context database. "Code first" approach.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URC.Models
{
    /// <summary>
    /// Represents an CompletedCourse entity.
    /// </summary> 
    public class CompletedCourse
    {
        /// <summary>
        /// An int representing the CompletedCourse's ID.
        /// 
        /// Note: this is the primary key.
        /// </summary> 
        public int ID { get; set; }

        /// <summary>
        /// A string representing the CompeletedCourse's name.
        /// </summary> 
        public string CompletedCourseName { get; set; }

        /// <summary>
        /// An int representing the CompletedCourse's StudentID.
        /// 
        /// Note: this is a foreign key.
        /// </summary> 
        public int StudentID { get; set; }


        /// <summary>
        /// A Student object representing the CompletedCourse's Student.
        /// 
        /// Note: this is a navigation property.
        /// </summary> 
        public Student Student { get; set; }
    }
}
