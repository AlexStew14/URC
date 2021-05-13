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
 *    Contains C# code for the database initializer.
 */

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URC.Areas.Identity.Data;

namespace URC.Data
{
    /// <summary>
    /// This class represents the initializer for a database.
    /// </summary>
    public class DbInitializer
    {
        /// <summary>
        /// Initializes the needed databases after ensuring creation.
        /// </summary>
        /// <param name="services">The given service provider.</param>
        public static void Initialize(IServiceProvider services)
        {
            // Get required services
            var urc_db = services.GetRequiredService<URC_Context>();
            var user_roles_db = services.GetRequiredService<UsersRolesDB>();
            var userManager = services.GetRequiredService<UserManager<URCUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure database creation
            urc_db.Database.EnsureCreated();
            user_roles_db.Database.EnsureCreated();

            // Initialize Opportunities/Skills/Tags
            Opportunity_Seeding.Initialize(urc_db);

            // Initialize UserRolesDB
            SeedUsersRolesDB.Initialize(userManager, roleManager, user_roles_db).Wait();

            // Initialize Student Applications
            Student_Application_Seeding.Initialize(urc_db, userManager);
        }
    }
}