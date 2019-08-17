using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string AccesiblePlants { get; set; }
        public string AccesibleLocations { get; set; }
        public string AccesibleFunctions { get; set; }
        public string Authority { get; set; }
    }
}
