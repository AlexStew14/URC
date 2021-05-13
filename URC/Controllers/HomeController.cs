/**
 * Author:    Jantzen Allphin
 * Partner:   None
 * Date:      September 2, 2020
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
 *    Contains C# code for the home controller of the web site.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using URC.Models;

namespace URC.Controllers
{
    /// <summary>
    /// This class represents the controller for the home page.
    /// </summary> 
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Creates the HomeController object
        /// </summary>
        /// <param name="logger">The logger to be used.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns the index page for the home page.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns the "handmade" opportunities page.
        /// </summary>
        public IActionResult Handmade_Opportunities()
        {
            return View();
        }

        /// <summary>
        /// Returns the "handmade" details page for a single opportunity.
        /// </summary>
        public IActionResult Handmade_Details()
        {
            return View();
        }

        /// <summary>
        /// Returns the privacy page for the website.
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Returns the Error page for the website.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
