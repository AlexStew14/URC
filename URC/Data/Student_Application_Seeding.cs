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
 *    Contains C# code for seeding the Student/StudentSkill/CompletedCourses tables in the URC_Context database.
 */

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URC.Areas.Identity.Data;
using URC.Models;

namespace URC.Data
{
    public class Student_Application_Seeding
    {
        /// <summary>
        /// Seeds the given database with Student/StudentSkill/CompletedCourses if needed.
        /// </summary>
        /// <param name="context">The context (database) to be used.</param>
        public static void Initialize(URC_Context context, UserManager<URCUser> userManager)
        {
            // Look for any student aplications.
            if (context.Students.Any())
            {
                return; // DB has been seeded
            }

            var sid1 = userManager.FindByEmailAsync("u0000001@utah.edu").Result.Id.ToString();
            var sid2 = userManager.FindByEmailAsync("u0000002@utah.edu").Result.Id.ToString();
            var studentApplications = new Student[]
            {
                new Student{Sid=sid2,StudentName="Ashley Eaton",ExpectedGraduationDate=DateTime.Parse("2022-05-10"),DegreePlan="CE",GPA=(float)3.9,Uid="u0000002",AvailableHoursPerWeek=10,PersonalStatement="Hi, I would like to be considered for a research position. I believe that I will be a great addition to any team. I am a really good student.",ProfileCreationDate=DateTime.Parse("2020-10-11"),Active=true},
                new Student{Sid=sid1,StudentName="Oliver Allphin",ExpectedGraduationDate=DateTime.Parse("2021-05-10"),DegreePlan="CS",GPA=(float)3.7,Uid="u0000001",AvailableHoursPerWeek=20,PersonalStatement="This is my personal statement. I am a great student and am very interested in research. Please consider me! This is my personal statement. I am a great student and am very interested in research. Please consider me! This is my personal statement. I am a great student and am very interested in research. Please consider me!",ProfileCreationDate=DateTime.Parse("2020-10-10"),Active=true}
            };

            context.Students.AddRange(studentApplications);
            context.SaveChanges();

            var studentSkills = new StudentSkill[]
            {
                new StudentSkill{SkillName="C#",StudentID=1},
                new StudentSkill{SkillName="ASP.NET Core",StudentID=1},
                new StudentSkill{SkillName="Entity Framework",StudentID=1},
                new StudentSkill{SkillName="Javascript",StudentID=1},
                new StudentSkill{SkillName="HTML",StudentID=1},
                new StudentSkill{SkillName="Java",StudentID=2},
                new StudentSkill{SkillName="Bootstrap",StudentID=2},
                new StudentSkill{SkillName="MongoDB",StudentID=2},
                new StudentSkill{SkillName="ElasticSearch",StudentID=2}
            };

            context.StudentSkills.AddRange(studentSkills);
            context.SaveChanges();

            // Seed Popular Student Skills
            foreach (var s in studentSkills)
            {
                context.PopularStudentSkills.Add(new PopularStudentSkill { name = s.SkillName.ToUpper(), count = 1 });
            }

            context.SaveChanges();
            var interests = new Interest[]
            {
                new Interest{InterestName="Basketball",StudentID=1},
                new Interest{InterestName="Snowboarding",StudentID=1},
                new Interest{InterestName="Web Software Architecture",StudentID=1},
                new Interest{InterestName="Wake Boarding",StudentID=1},
                new Interest{InterestName="Art",StudentID=2},
                new Interest{InterestName="Piano",StudentID=2},
                new Interest{InterestName="Rock Climbing",StudentID=2},
                new Interest{InterestName="Chess",StudentID=2}
            };

            context.Interests.AddRange(interests);
            context.SaveChanges();

            var completedCourses = new CompletedCourse[]
            {
                new CompletedCourse{CompletedCourseName="CS-3100",StudentID=1},
                new CompletedCourse{CompletedCourseName="CS-2420",StudentID=1},
                new CompletedCourse{CompletedCourseName="CS-4480",StudentID=1},
                new CompletedCourse{CompletedCourseName="CS-3505",StudentID=1},
                new CompletedCourse{CompletedCourseName="CS-2420",StudentID=2},
                new CompletedCourse{CompletedCourseName="CS-1030",StudentID=2},
                new CompletedCourse{CompletedCourseName="CS-1410",StudentID=2},
                new CompletedCourse{CompletedCourseName="MATH-2270",StudentID=2}
            };

            context.CompletedCourses.AddRange(completedCourses);
            context.SaveChanges();
        }
    }
}