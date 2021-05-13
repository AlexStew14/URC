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
 *     Contains C# code for seeding Users, Roles, and UserRoles in the UsersRolesDB database.
 */

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URC.Data;

namespace URC.Areas.Identity.Data
{
    /// <summary>
    /// This class represents the methods needed to seed the UserRolesDB.
    /// </summary>
    public class SeedUsersRolesDB
    {
        /// <summary>
        /// Seeds the given database with Users/Roles/UserRoles if needed.
        /// </summary> 
        public static async Task Initialize(UserManager<URCUser> userManager, RoleManager<IdentityRole> roleManager, UsersRolesDB context)
        {

            // seed roles if needed
            if (roleManager.Roles.ToArray().Count() == 0)
            {
                var roles = new IdentityRole[]
                {
                    new IdentityRole{ Name="Administrator" },
                    new IdentityRole{ Name="Professor" },
                    new IdentityRole{ Name="Student" }
                };

                foreach (IdentityRole role in roles)
                {
                    var result = await roleManager.CreateAsync(role);
                }
            }

            // seed users if needed
            if (userManager.Users.ToArray().Count() == 0)
            {
                var users = new URCUser[]
                {
                    new URCUser{ UserName="admin", Email="admin@utah.edu", EmailConfirmed=true },
                    new URCUser{ UserName="professor", Email="professor@utah.edu", EmailConfirmed=true },
                    new URCUser{ UserName="professor_mary", Email="professor_mary@utah.edu", EmailConfirmed=true },
                    new URCUser{ UserName="u0000000", Email="u0000000@utah.edu", EmailConfirmed=true },
                    new URCUser{ UserName="u1234567", Email="u1234567@utah.edu", EmailConfirmed=true },
                    new URCUser{ UserName="u0000001", Email="u0000001@utah.edu", EmailConfirmed=true },
                    new URCUser{ UserName="u0000002", Email="u0000002@utah.edu", EmailConfirmed=true },
                    new URCUser{ UserName="u0000003", Email="u0000003@utah.edu", EmailConfirmed=true }
                };

                foreach (URCUser user in users)
                {
                    var result = await userManager.CreateAsync(user, "123ABC!@#def");


                    // add user roles
                    if(result.Succeeded)
                    {
                        var currUser = await userManager.FindByEmailAsync(user.Email);

                        if (currUser.Email.Contains("admin"))
                        {
                            await userManager.AddToRoleAsync(currUser, "Administrator");
                        }
                        else if (currUser.Email.Contains("professor"))
                            await userManager.AddToRoleAsync(currUser, "Professor");
                        else
                            await userManager.AddToRoleAsync(currUser, "Student");
                    }
                }
            }
        }
    }
}
