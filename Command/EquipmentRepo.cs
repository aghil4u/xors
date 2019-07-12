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
    class EquipmentRepo
    {
        HttpClient client = new HttpClient();

        public EquipmentRepo()
        {
             //client.BaseAddress = new Uri("http://mwsams.xo.rs/");
            client.BaseAddress = new Uri("https://localhost:44363/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<Equipment> GetEquipments()
        {
            Console.WriteLine(" DOWNLOADING EQUIPMENT LIST FROM SERVER");

            HttpResponseMessage response =  client.GetAsync("api/Equipments").Result;
            if (response == null)
            {
                return new List<Equipment>();
            }
            if (response.IsSuccessStatusCode)
            {
                var stringResult =  response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Equipment>>(stringResult);
            }

            return new List<Equipment>();
        }

        public void UpdateEquipments(List<Equipment> eqpt)
        {


            int failCount = 0;
            for (int i = 0; i < eqpt.Count; i++)
            {
                Equipment equipment = eqpt[i];
                HttpResponseMessage response =  client.PutAsync("api/Equipments/"+equipment.id,
                    new StringContent(JsonConvert.SerializeObject(equipment), Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                {
                    failCount++;
                }
                Console.Write("\r UPDATING EQUIPMENT " + i + "/" + eqpt.Count + " (FAILS " + failCount + ")");
            }


          
        }

        public void AddEquipments(List<Equipment> eqpt)
        {


            int failCount = 0;
            for (int i = 0; i < eqpt.Count; i++)
            {
                Equipment equipment = eqpt[i];

                HttpResponseMessage response = client.PostAsync("api/Equipments",
                    new StringContent(JsonConvert.SerializeObject(equipment), Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                {
                    failCount++;
                    Console.Write(response.Headers.ToString());
                }
                Console.Write("\r ADDING ITEM " + i + "/" + eqpt.Count + " (FAILS " + failCount + ")");
            }



        }

        public void DeleteEquipments(List<Equipment> eqpt)
        {
            int failCount = 0;
            
           

            for (int i = 0; i < eqpt.Count; i++)
            {
                Equipment equipment = (Equipment)eqpt[i];
                HttpResponseMessage response = client.DeleteAsync("api/Equipments/"+equipment.id).Result;
                if (!response.IsSuccessStatusCode)
                {
                    failCount++;
                }
                Console.Write("\r DELETING ITEM " + i +"/"+eqpt.Count+" (FAILS "+failCount+")");
            }


            
        }

        public void DeleteVerifications(List<Verification> ver)
        {
            int failCount = 0;



            for (int i = 0; i < ver.Count; i++)
            {
                Verification verification = (Verification)ver[i];
                HttpResponseMessage response = client.DeleteAsync("api/Verifications/" + verification.id).Result;
                if (!response.IsSuccessStatusCode)
                {
                    failCount++;
                }
                Console.Write("\r DELETING ITEM " + i + "/" + ver.Count + " (FAILS " + failCount + ")");
            }



        }

        internal List<Verification> GetVerifications()
        {
            Console.WriteLine(" DOWNLOADING VERIFICATIONS LIST FROM SERVER");

            HttpResponseMessage response = client.GetAsync("api/Verifications").Result;
            if (response.IsSuccessStatusCode)
            {
                var stringResult = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Verification>>(stringResult);
            }

            return new List<Verification>();
        }

        internal void UpdateVerifications(List<Verification> verifications)
        {
            int failCount = 0;
            for (int i = 0; i < verifications.Count; i++)
            {
                Verification ver = verifications[i];
                HttpResponseMessage response = client.PutAsync("api/Verifications/" + ver.id,
                    new StringContent(JsonConvert.SerializeObject(ver), Encoding.UTF8, "application/json")).Result;
                if (!response.IsSuccessStatusCode)
                {
                    failCount++;
                }
                Console.Write("\r UPDATING EQUIPMENT " + i + "/" + verifications.Count + " (FAILS " + failCount + ")");
            }

        }
    }
}