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
 *    Contains C# code for the Admin controller.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using URC.Areas.Identity.Data;
using URC.Data;

namespace URC.Controllers
{
    /// <summary>
    /// This class represents the controller for the Admin views.
    /// </summary> 
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UsersRolesDB _context;
        private readonly UserManager<URCUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Creates an AdminController object.
        /// </summary> 
        /// <param name="context">The context (database) to be used.</param>
        public AdminController(UsersRolesDB context, UserManager<URCUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Returns the Admin (Users/Roles) page.
        /// </summary> 
        public IActionResult Admin()
        {
            return View();
        }

        /// <summary>
        /// Changes the role for the given user_id. Based on the value of add_remove,
        ///     it will either remove or add the given role to the user.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Change_Role(string user_id, string role, string add_remove)
        {
            var user = await _userManager.FindByIdAsync(user_id);

            if (add_remove == "add")
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                if (result.Succeeded)
                    return Ok(new { success = true, message = "added!" });
                else
                    return BadRequest(new { success = false, message = "bad request" });
            }
            else
            {
                var result = await _userManager.RemoveFromRoleAsync(user, role);
                if (result.Succeeded)
                    return Ok(new { success = true, message = "removed!" });
                else
                    return BadRequest(new { success = false, message = "bad request" });
            }
                
        }

    }
}
