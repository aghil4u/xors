using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Models;

namespace Command
{
    class EmployeeRepo
    {
        HttpClient client = new HttpClient();

        public EmployeeRepo()
        {
            client.BaseAddress = new Uri("http://mwsams.xo.rs/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<Employee> GetEmployees()
        {
            Console.WriteLine(" DOWNLOADING Employee LIST FROM SERVER");

            HttpResponseMessage response =  client.GetAsync("api/Employees").Result;
            if (response.IsSuccessStatusCode)
            {
                var stringResult =  response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Employee>>(stringResult);
            }

            return new List<Employee>();
        }

        public void UpdateEmployees(List<Employee> eqpt)
        {


            int failCount = 0;
            for (int i = 0; i < eqpt.Count; i++)
            {
                Employee Employee = eqpt[i];
                HttpResponseMessage response =  client.PutAsync("api/Employees/"+Employee.id,
                    new StringContent(JsonConvert.SerializeObject(Employee), Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                {
                    failCount++;
                }
                Console.Write("\r UPDATING Employee " + i + "/" + eqpt.Count + " (FAILS " + failCount + ")");
            }


          
        }

        public void AddEmployees(List<Employee> eqpt)
        {


            int failCount = 0;
            for (int i = 0; i < eqpt.Count; i++)
            {
                Employee Employee = eqpt[i];
                HttpResponseMessage response = client.PostAsync("api/Employees",
                    new StringContent(JsonConvert.SerializeObject(Employee), Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                {
                    failCount++;
                }
                Console.Write("\r ADDING ITEM " + i + "/" + eqpt.Count + " (FAILS " + failCount + ")");
            }



        }

        public void DeleteEmployees(List<Employee> eqpt)
        {
            int failCount = 0;
            
           

            for (int i = 0; i < eqpt.Count; i++)
            {
                Employee Employee = (Employee)eqpt[i];
                HttpResponseMessage response = client.DeleteAsync("api/Employees/"+Employee.id).Result;
                if (!response.IsSuccessStatusCode)
                {
                    failCount++;
                }
                Console.Write("\r DELETING ITEM " + i +"/"+eqpt.Count+" (FAILS "+failCount+")");
            }


            
        }
    }
}