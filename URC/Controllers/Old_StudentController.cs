/**
 * Author:    Jantzen Allphin
 * Partner:   None
 * Date:      September 10, 2020
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
 *    Contains C# code for the student controller of the web site.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace URC.Controllers
{
    /// <summary>
    /// This class represents the controller for the Student views.
    /// </summary> 
    public class Old_StudentController : Controller
    {
        /// <summary>
        /// Returns the student Application page.
        /// </summary> 
        [Authorize(Roles = "Student")]
        public IActionResult Application()
        {
            return View();
        }
    }
}
