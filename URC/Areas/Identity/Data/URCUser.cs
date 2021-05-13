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
 *     Contains C# code for representing a URCUser which is the default user in the UsersRolesDB database.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace URC.Areas.Identity.Data
{
    /// <summary>
    /// Represents a URCUser.
    /// 
    /// Note: Add profile data for application users by adding properties to the URCUser class.
    /// </summary>
    public class URCUser : IdentityUser
    {
    }
}
