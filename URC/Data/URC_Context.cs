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
 *    Contains C# code for setting up and describing the URC_Context database.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using URC.Models;

namespace URC.Data
{
    /// <summary>
    /// This class represents a DbContext for the URC_Context database.
    /// </summary>
    public class URC_Context : DbContext
    {
        /// <summary>
        /// Creates the URC_Context using the giving DbContextOptions.
        /// </summary>
        /// <param name="options">The DbContextOptions to be used.</param>
        public URC_Context(DbContextOptions<URC_Context> options)
            : base(options)
        {
        }

        /// <summary>
        /// Represents the Opportunities DbSet property.
        /// </summary>
        public DbSet<Opportunity> Opportunities { get; set; }

        /// <summary>
        /// Represents the RequiredSkills DbSet property.
        /// </summary>
        public DbSet<RequiredSkill> RequiredSkills { get; set; }

        /// <summary>
        /// Represents the Tags DbSet property.
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Represents the Students DbSet property.
        /// </summary>
        public DbSet<Student> Students { get; set; }

        /// <summary>
        /// Represents the StudentSkills DbSet property.
        /// </summary>
        public DbSet<StudentSkill> StudentSkills { get; set; }

        /// <summary>
        /// Represents the Interests DbSet property.
        /// </summary>
        public DbSet<Interest> Interests { get; set; }

        /// <summary>
        /// Represents the CompletedCourses DbSet property.
        /// </summary>
        public DbSet<CompletedCourse> CompletedCourses { get; set; }

        /// <summary>
        /// Represents the Recommendations DbSet Property.
        /// </summary>
        public DbSet<Recommendation> Recommendations { get; set; }

        /// <summary>
        /// Represents the PopularStudentSkills DbSet Property.
        /// </summary>
        public DbSet<PopularStudentSkill> PopularStudentSkills { get; set; }

        /// <summary>
        /// Represents the PopularRequiredSkills DbSet Property.
        /// </summary>
        public DbSet<PopularRequiredSkill> PopularRequiredSkills { get; set; }

        /// <summary>
        /// Builds the tables associated with this URC_Context.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // opportunities
            modelBuilder.Entity<Tag>().ToTable("Tag");
            modelBuilder.Entity<RequiredSkill>().ToTable("RequiredSkill");
            modelBuilder.Entity<Opportunity>().ToTable("Opportunity");

            // students
            modelBuilder.Entity<StudentSkill>().ToTable("StudentSkill");
            modelBuilder.Entity<Interest>().ToTable("Interest");
            modelBuilder.Entity<CompletedCourse>().ToTable("CompletedCourse");
            modelBuilder.Entity<Student>().ToTable("Student")
                .Property(p => p.ProfileCreationDate)
                .ValueGeneratedOnAdd();

            // Code for configuring many to many relationship between opportunities and students for recommendations
            modelBuilder.Entity<Student>().HasMany(m => m.RecommendedOpportunities)
                .WithOne(m => m.student).HasForeignKey(k => k.StudentId);

            modelBuilder.Entity<Opportunity>().HasMany(m => m.RecommendedStudents)
                .WithOne(m => m.opportunity).HasForeignKey(k => k.OpportunityId);

            modelBuilder.Entity<Recommendation>().HasOne(m => m.opportunity)
                .WithMany(m => m.RecommendedStudents).HasForeignKey(k => k.StudentId);

            modelBuilder.Entity<Recommendation>().HasOne(m => m.student)
                .WithMany(m => m.RecommendedOpportunities).HasForeignKey(k => k.OpportunityId);

            modelBuilder.Entity<Recommendation>().ToTable("Recommendation");

            // Popular skills
            modelBuilder.Entity<PopularStudentSkill>().ToTable("PopularStudentSkill");
            modelBuilder.Entity<PopularRequiredSkill>().ToTable("PopularRequiredSkill");
        }

        /// <summary>
        /// Overrides SaveChangesAsync to manually modify/save ProfileCreationDate for a Student application.
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.Now;

            foreach (var item in ChangeTracker.Entries<Student>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                item.Property("ProfileCreationDate").CurrentValue = now;
            }

            return base.SaveChangesAsync();
        }

        /// <summary>
        /// Overrides SaveChangesAsync to manually modify/save ProfileCreationDate for a Student application.
        /// </summary>
        public DbSet<URC.Models.Application> Application { get; set; }
    }
}