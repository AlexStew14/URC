/**
 * Author:    Jantzen Allphin
 * Partner:   None
 * Date:      October 3, 2020
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
 *     Contains C# code for setting up and describing the UsersRolesDB database.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using URC.Areas.Identity.Data;

namespace URC.Data
{
    /// <summary>
    /// Represents a UsersRolesDB context.
    /// </summary>
    public class UsersRolesDB : IdentityDbContext<URCUser>
    {
        /// <summary>
        /// Constructs a UsersRolesDB context.
        /// </summary>
        /// <param name="options">Database options to be used</param>
        public UsersRolesDB(DbContextOptions<UsersRolesDB> options)
            : base(options)
        {
        }

        /// <summary>
        /// Builds the tables associated with this UsersRolesDB.
        /// </summary> 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
