using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using URC.Areas.Identity.Data;

namespace URC.Models
{
    public class Application
    {

        /// <summary>
        /// An int representing the application id.
        /// 
        /// Note: this is the primary key.
        /// </summary> 
        public int ID { get; set; }

        /// <summary>
        /// A string representing the application statement.
        /// </summary>
        [Required]
        [MaxLength(1000, ErrorMessage = "Please limit your statement to 1000 characters"), MinLength(10)]
        public string Statement { get; set; }

        /// <summary>
        /// A string representing the applications's resume (file name).
        /// </summary> 
        [Required]
        public string Resume { get; set; }

        /// <summary>
        /// A string representing the applications's status (accepted/denied/-).
        /// </summary> 
        public string Status { get; set; }

        /// <summary>
        /// An int representing the application's student user id.
        /// </summary>
        public string StudentID { get; set; }

        /// <summary>
        /// An int representing the applications's opportunity.
        /// 
        /// Note: this is a foreign key.
        /// </summary> 
        public int OpportunityID { get; set; }

        ///// <summary>
        ///// A Student object representing the applications's Student.
        ///// 
        ///// Note: this is a navigation property.
        ///// </summary> 
        //public Student Student { get; set; }

        /// <summary>
        /// An Opportunity object representing the applications's related Opportunity.
        /// 
        /// Note: this is a navigation property.
        /// </summary> 
        public Opportunity Opportunity { get; set; }
    }
}
