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
 *    Contains C# code modeling a StudentSummaryObject.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace URC.Models
{
    /// <summary>
    /// Represents a Student summary object for returning as results to a search query on the Search page.
    /// </summary>
    public class StudentSummaryObject
    {
        /// <summary>
        /// Constructs the StudentSummaryObject based on the given Student object.
        /// </summary>
        /// <param name="student"></param>
        public StudentSummaryObject(Student student)
        {
            ID = student.ID;
            StudentName = student.StudentName;
            Uid = student.Uid;
            GPA = student.GPA;
            PersonalStatement = student.PersonalStatement;
            StudentSkills = new string[student.StudentSkills.Count];
            int i = 0;
            foreach(var skill in student.StudentSkills.ToArray())
            {
                StudentSkills[i] = skill.SkillName;
                i++;
            }
        }

        /// <summary>
        /// An int representing the Students's ID.
        /// </summary> 
        public int ID { get; set; }

        /// <summary>
        /// A string representing the Students's name.
        /// </summary> 
        public string StudentName { get; set; }

        /// <summary>
        /// A string representing the Students's Uid.
        /// </summary> 
        public string Uid { get; set; }

        /// <summary>
        /// An float representing the Students's GPA.
        /// </summary> 
        public float GPA { get; set; }

        /// <summary>
        /// A string representing the Students's personal statement.
        /// </summary>  
        public string PersonalStatement { get; set; }

        /// <summary>
        /// A string comma-seperated representing the StudentSkill objects representing the Students's skills.
        /// </summary>  
        public string[] StudentSkills { get; set; }
    }
}
