using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Employee
    {

        [Key] public int id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Nationality { get; set; }
        public string Project { get; set; }
        public string ContactNumber { get; set; }
        public string Landline { get; set; }
        public string EmailAddress { get; set; }
        public string JoiningDate { get; set; }
        public string EmployeeNumber { get; set; }
        public string GlobalNumber { get; set; }

    }
}
