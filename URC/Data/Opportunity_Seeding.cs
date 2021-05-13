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
 *    Contains C# code for seeding the opportunity/skill/tag tables in the URC_Context database.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URC.Data;
using URC.Models;

namespace URC.Data
{
    /// <summary>
    /// This class represents the methods needed to seed the given database with Opportunities/RequiredSkills/Tags if needed.
    /// </summary> 
    public static class Opportunity_Seeding
    {
        /// <summary>
        /// Seeds the given database with Opportunities/RequiredSkills/Tags if needed.
        /// </summary> 
        /// <param name="context">The context (database) to be used.</param>
        public static void Initialize(URC_Context context)
        {
            // Look for any opportunities.
            if (context.Opportunities.Any())
            {
                return; // DB has been seeded
            }

            var opportunities = new Opportunity[]
            {
                new Opportunity{ProjectName="Artificial Muscle Polymer Experiments",ProfessorName="professor",ProjectDescription="Twisted coiled polymer actuators (TCPAs) are an emergent type of thermomotive artificial muscle that have recently biome-chanically surpassed human skeletal muscles (HSM). This all results in tensile contraction (TC) and Tensile Stress (TS). We are trying to optimize the polymer material properties to improve action.",ProjectImage="uofu.jpg",ProjectMentor="John Smith",BeginDate=DateTime.Parse("2021-03-03"),EndDate=DateTime.Parse("2021-06-03"),Pay=20,Filled=false},
                new Opportunity{ProjectName="City Climate Change Research Assistant",ProfessorName="professor_mary",ProjectDescription="I am analyzing city climate action plans across the world and trying to find key themes, characteristics, and patterns of these climate action plans. Specifically, I will be focusing on Ho Chi Minh City, Buenos Aires, Seoul, and Mexico City, but will also be looking at other city environmental actions as needed for this project.",ProjectImage="uofu.jpg",ProjectMentor="Suzy Smith",BeginDate=DateTime.Parse("2021-05-10"),EndDate=DateTime.Parse("2021-10-10"),Pay=25,Filled=false},
                new Opportunity{ProjectName="Multiple Computational Research Positions",ProfessorName="Larry Longbottom",ProjectDescription="We are looking for motivated candidates to join the Utah Waves and Architected Materials Group.",ProjectImage="uofu.jpg",ProjectMentor="Larry Longbottom",BeginDate=DateTime.Parse("2021-08-21"),EndDate=DateTime.Parse("2021-12-03"),Pay=30,Filled=false},
                new Opportunity{ProjectName="Undergraduate Research Assistant",ProfessorName="Oliver Olson",ProjectDescription="Patient with acute ischemic stroke within the last 7 days will have a small blood pressure on their finger for 15 minutes.We will also use the NeurOptics NPi - 200 Pupillometer to measure each enrolled participants pupil reactivity prior to the finger blood pressure measurement.",ProjectImage="uofu.jpg",ProjectMentor="Bobby Joe Hill",BeginDate=DateTime.Parse("2020-12-04"),EndDate=DateTime.Parse("2021-04-03"),Pay=10,Filled=false},
                new Opportunity{ProjectName="Undergrad Research Scholars - Wynne Laboratory",ProfessorName="Jerry Wynne",ProjectDescription="Our laboratory researches hypertension, and in particular, salt-sensitive hypertension. We investigate this from a cell/molecular basis using cell culture, microscopy and whole animal experiments. We are interested in the renal and cardiovascular physiology during hypertension, with an emphasis on inflammation.",ProjectImage="uofu.jpg",ProjectMentor="Bobby Joe Hill",BeginDate=DateTime.Parse("2021-04-08"),EndDate=DateTime.Parse("2021-08-04"),Pay=20,Filled=false},
                new Opportunity{ProjectName="Part Time - Studying Genetic Diseases",ProfessorName="Joe Montgomery",ProjectDescription="I would like to mentor motivated students. Projects will depend on the previous experience of students.",ProjectImage="uofu.jpg",ProjectMentor="Joe Montgomery",BeginDate=DateTime.Parse("2021-06-03"),EndDate=DateTime.Parse("2021-09-03"),Pay=10,Filled=false},
                new Opportunity{ProjectName="Machine Learning Based Lensless Cameras",ProfessorName="Susan Donald",ProjectDescription="By exploiting recent advances in machine learning, you will work in a small team to develop several uncoventional cameras. One example of a such a camera is a lensless camera that is extremely thin lightweight, yet able to perform high resolution imaging. Important applications in autonomous driving, drones, security, etc. will be explored.",ProjectImage="uofu.jpg",ProjectMentor="Susan Donald",BeginDate=DateTime.Parse("2021-03-03"),EndDate=DateTime.Parse("2021-06-03"),Pay=20,Filled=false},
                new Opportunity{ProjectName="Wildlife Conservation Research",ProfessorName="Jeniffer Jacobs",ProjectDescription="The Wasatch Mountains (WM) are a stronghold for wildlife but contain the most highly recreated National Forests in the country. There is a lack of research into how wildlife are affected by human recreation, and this hampers conservation action, especially in the WM’s.",ProjectImage="uofu.jpg",ProjectMentor="Jeniffer Jacobs",BeginDate=DateTime.Parse("2021-03-03"),EndDate=DateTime.Parse("2021-06-03"),Pay=20,Filled=false}
            };

            context.Opportunities.AddRange(opportunities);
            context.SaveChanges();

            var requiredSkills = new RequiredSkill[]
            {
                new RequiredSkill{SkillName="C#",OpportunityID=1},
                new RequiredSkill{SkillName="ASP.NET Core",OpportunityID=1},
                new RequiredSkill{SkillName="Entity Framework",OpportunityID=1},
                new RequiredSkill{SkillName="Javascript",OpportunityID=1},
                new RequiredSkill{SkillName="Java",OpportunityID=2},
                new RequiredSkill{SkillName="Bootstrap",OpportunityID=2},
                new RequiredSkill{SkillName="MongoDB",OpportunityID=2},
                new RequiredSkill{SkillName="ElasticSearch",OpportunityID=2},
                new RequiredSkill{SkillName="Python",OpportunityID=3},
                new RequiredSkill{SkillName="PDB",OpportunityID=3},
                new RequiredSkill{SkillName="Flask",OpportunityID=3},
                new RequiredSkill{SkillName="Splunk",OpportunityID=3},
                new RequiredSkill{SkillName="SQL",OpportunityID=4},
                new RequiredSkill{SkillName="C",OpportunityID=4},
                new RequiredSkill{SkillName="GCC",OpportunityID=4},
                new RequiredSkill{SkillName="GDB",OpportunityID=4},
                new RequiredSkill{SkillName="SpringBoot",OpportunityID=5},
                new RequiredSkill{SkillName="Prometheus",OpportunityID=5},
                new RequiredSkill{SkillName="Grafana",OpportunityID=5},
                new RequiredSkill{SkillName="Alertmanager",OpportunityID=5},
                new RequiredSkill{SkillName="Docker",OpportunityID=6},
                new RequiredSkill{SkillName="Docker Swarm",OpportunityID=6},
                new RequiredSkill{SkillName="Linux CLI",OpportunityID=6},
                new RequiredSkill{SkillName="BASH",OpportunityID=6},
                new RequiredSkill{SkillName="Javascript",OpportunityID=7},
                new RequiredSkill{SkillName="CSS",OpportunityID=7},
                new RequiredSkill{SkillName="HTML5",OpportunityID=7},
                new RequiredSkill{SkillName="C#",OpportunityID=7},
                new RequiredSkill{SkillName="HTML",OpportunityID=8},
                new RequiredSkill{SkillName="Java",OpportunityID=8},
                new RequiredSkill{SkillName="Kubernetes",OpportunityID=8},
                new RequiredSkill{SkillName="Docker",OpportunityID=8}
            };

            context.RequiredSkills.AddRange(requiredSkills);
            context.SaveChanges();

            // Seed Popular Student Skills
            foreach (var s in requiredSkills)
            {
                context.PopularRequiredSkills.Add(new PopularRequiredSkill { name = s.SkillName.ToUpper(), count = 1 });
            }

            var tags = new Tag[]
            {
                new Tag{TagName="Computer Science",OpportunityID=1},
                new Tag{TagName="Biology",OpportunityID=1},
                new Tag{TagName="Ecology",OpportunityID=2},
                new Tag{TagName="Computer Science",OpportunityID=3},
                new Tag{TagName="Information Systems",OpportunityID=3},
                new Tag{TagName="Biochemistry",OpportunityID=4},
                new Tag{TagName="Computer Networking",OpportunityID=5},
                new Tag{TagName="Computer Science",OpportunityID=5},
                new Tag{TagName="Biology",OpportunityID=6},
                new Tag{TagName="Genetics",OpportunityID=6},
                new Tag{TagName="Machine Learning",OpportunityID=7},
                new Tag{TagName="Computer Science",OpportunityID=7},
                new Tag{TagName="Biology",OpportunityID=8},
                new Tag{TagName="Ecology",OpportunityID=8}
            };

            context.Tags.AddRange(tags);
            context.SaveChanges();

        }
    }
}
