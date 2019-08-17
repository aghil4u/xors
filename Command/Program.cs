using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Google.Protobuf.WellKnownTypes;
using OfficeOpenXml;
using Server.Models;



namespace Command
{
    class Program
    {
        private static CancellationTokenSource tokenSource;
        private static EquipmentRepo EqpRepo;
        private static EmployeeRepo EmpRepo;
        public static string BaseDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.Parent.Parent.Parent.ToString();
        public static string FAR = BaseDirectory+@"\ASSETS\FAR 2200.xlsx";
        public static string EQM = BaseDirectory+@"\ASSETS\EQM 2200.xlsx";
        public static string EMP = BaseDirectory+@"\OTHERS\EMPLIST.xlsx";
        public static List<Equipment> ETEMP = new List<Equipment>();
        public static List<Equipment> Equipments = new List<Equipment>();
        public static List<Employee> Employees = new List<Employee>();

      
        
        
        
 

        static void Main(string[] args)
        {
             Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            tokenSource = new CancellationTokenSource();

            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("------------EMMA SERVER UPDATE------3.5--------------");
            Console.WriteLine("-----------------------------------------------------");
           // Console.WriteLine(EQM);

            EqpRepo = new EquipmentRepo();
            EmpRepo = new EmployeeRepo();

           Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\Dell\auth.json");



            HandleCommand();
        }

        private static void HandleError(Task<List<Equipment>> task)
        {
            Console.WriteLine("\nThere was a problem retrieving data");
            Environment.Exit(1);
        }

        private static void HandleCancellation(Task<List<Equipment>> task)
        {
            Console.WriteLine("\nThe operation was canceled");
            Environment.Exit(0);
        }

        private static void HandleCommand()
        {
            while (true)
                switch (Console.ReadLine().ToUpper())
                {
                   
                  
                    case "CLOSE":
                        Environment.Exit(0);
                        break;
                    case "UPLOAD":
                        // UploadEquipments();
                        SmartUploadEquipments();
                        break;
                    case "INITIAL UPLOAD":
                        InitialUpload();
                        break;
                    case "FIRE":
                        ReadFar();
                        ReadEqm();
                        ProcessFar();
                        UploadFirestoreAsync();
                        break;

                    case "FIREINTHEHOLE":
                        ReadFar();
                        ReadEqm();
                        ProcessFar();
                        UpdateFirestoreAsync();
                        break;

                    case "UPLOAD EMPLOYEES":
                        UploadEmployees();
                        break;
                    case "UPDATE":
                        UploadEquipments();
                        break;
                    case "WIPE":
                        WipeEquipments();
                        break;
                    case "WIPE VERIFICATIONS":
                        WipeVerifications();
                        break;

                    case "CORRECT":
                        CorrectImageSource();
                        break;
                    default:
                        Console.WriteLine("UNIDENTIFIED COMMAND");
                        HandleCommand();
                        break;
                }
        }

        private static void CorrectImageSource()
        {
            Console.WriteLine();

            List<Verification> eListServer = EqpRepo.GetVerifications();
            List<Verification> EquipmentsToUpdate = new List<Verification>();
  
            int Updates = 0;
            int Additions = 0;
            for (int i = 0; i < eListServer.Count; i++)
            {
                try
                {

                    Verification eServer = eListServer[i];
                    if (eServer != null)
                    {
                        if (eServer.ImageUrl != "") 
                        {
                            eServer.ImageUrl = eServer.ImageUrl.Substring(12);

                            EquipmentsToUpdate.Add(eServer);
                        }
                    }


                    Console.Write("\r FOUND " + Updates + " UPDATES & " + Additions + " ADDITIONS");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            Console.WriteLine("UPLOADING UPDATES TO SERVER");
            if (EquipmentsToUpdate.Count > 0) EqpRepo.UpdateVerifications(EquipmentsToUpdate);

        }

        private static async Task UploadFirestoreAsync()
        {

            for (int i = 0; i < Equipments.Count; i++)
            {
                Equipment equipment = Equipments[i];
                try
                {

                    FirestoreDb db = FirestoreDb.Create("mws-ams");



                    CollectionReference collection = db.Collection("assets");
                    Dictionary<string, object> user = new Dictionary<string, object>
            {
                { "AssetNumber", equipment.AssetNumber },
                { "EquipmentNumber",  equipment.EquipmentNumber },

         {"AcquisitionDate" , equipment.AcquisitionDate.ToUniversalTime()},
         {"PendingUpdate" ,equipment.PendingUpdate},
         {"AcquisitionValue" ,equipment.AcquisitionValue},
         {"BookValue",equipment.BookValue},
         {"AssetDescription" ,equipment.AssetDescription},
         {"EquipmentDescription" ,equipment.EquipmentDescription},
         {"OperationId",equipment.OperationId},
         {"SubType",equipment.SubType},
         {"Weight",equipment.Weight},
         {"WeightUnit" ,equipment.WeightUnit},
         {"Dimensions",equipment.Dimensions},
         {"Tag",equipment.Tag},
         {"Type" ,equipment.Type},
         {"Connection" ,equipment.Connection},
         {"Length",equipment.Length},
         {"ModelNumber" ,equipment.ModelNumber},
         {"SerialNumber",equipment.SerialNumber},
         {"AssetLocation" ,equipment.AssetLocation},
         {"AssetLocationText" ,equipment.AssetLocationText},
         {"EquipmentLocation",equipment.EquipmentLocation},
         {"TimeStamp",DateTime.UtcNow.ToString("yyyyMMddHHmmssffff")},

    };
                    await collection.Document(equipment.AssetNumber).SetAsync(user);
                    Console.Write("\r"+i+" Lines Uploaded...");



                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            }


        
        }


        private static async Task UpdateFirestoreAsync()
        {
            Console.WriteLine();
            Console.WriteLine(" DOWNLOADING EQUIPMENT LIST FROM FIRESTORE");


            FirestoreDb db = FirestoreDb.Create("mws-ams");
            // [START fs_get_multiple_docs]
            Query capitalQuery = db.Collection("assets"); //.WhereEqualTo("Capital", true);
            QuerySnapshot capitalQuerySnapshot = await capitalQuery.GetSnapshotAsync();
            Console.WriteLine();
            Console.WriteLine(" DOWNLOAD COMPLETED. NOW PROCESSING");


            for (int i = 0; i < Equipments.Count; i++)
            {
                Equipment equipment = Equipments[i];
                equipment.TimeStamp = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssffff"));
                try
                {
                    DocumentSnapshot eqsnapshot = capitalQuerySnapshot.Documents.FirstOrDefault(e=>e.Id==equipment.AssetNumber);
                    if (eqsnapshot == null)
                    {
//                        Dictionary<string, object> user = new Dictionary<string, object>
//                        {
//                            { "AssetNumber", equipment.AssetNumber },
//                            { "EquipmentNumber",  equipment.EquipmentNumber },
//
//                            {"AcquisitionDate" , equipment.AcquisitionDate.ToUniversalTime()},
//                            {"PendingUpdate" ,equipment.PendingUpdate},
//                            {"AcquisitionValue" ,equipment.AcquisitionValue},
//                            {"BookValue",equipment.BookValue},
//                            {"AssetDescription" ,equipment.AssetDescription},
//                            {"EquipmentDescription" ,equipment.EquipmentDescription},
//                            {"OperationId",equipment.OperationId},
//                            {"SubType",equipment.SubType},
//                            {"Weight",equipment.Weight},
//                            {"WeightUnit" ,equipment.WeightUnit},
//                            {"Dimensions",equipment.Dimensions},
//                            {"Tag",equipment.Tag},
//                            {"Type" ,equipment.Type},
//                            {"Connection" ,equipment.Connection},
//                            {"Length",equipment.Length},
//                            {"ModelNumber" ,equipment.ModelNumber},
//                            {"SerialNumber",equipment.SerialNumber},
//                            {"AssetLocation" ,equipment.AssetLocation},
//                            {"AssetLocationText" ,equipment.AssetLocationText},
//                            {"EquipmentLocation",equipment.EquipmentLocation},
//
//                        };

                        await db.Collection("assets").Document(equipment.AssetNumber).SetAsync(equipment);
                    }
                    else
                    {
                        Equipment serverEquipment = eqsnapshot.ConvertTo<Equipment>();
                        if (serverEquipment.EquipmentDescription!=equipment.EquipmentDescription)
                        {
                            serverEquipment.EquipmentDescription = equipment.EquipmentDescription;

                            await db.Collection("assets").Document(equipment.AssetNumber).SetAsync(equipment);
                        }

                    }

                    
                    Console.Write("\r UPDATING "+i+ " OF "+Equipments.Count);



                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            }



          

        }

   

        private static void WipeEquipments()
        {

            List<Equipment> equipments = EqpRepo.GetEquipments();

            EqpRepo.DeleteEquipments(equipments);


        }
        private static void WipeVerifications()
        {

            

            List<Verification> ver = EqpRepo.GetVerifications();

            EqpRepo.DeleteVerifications(ver);


        }

        private static void UploadEquipments()
        {
            
            ReadFar();
            ReadEqm();
            ProcessFar();
            UpdateEquipmentListToServer();
        }

        private static void SmartUploadEquipments()
        {

            ReadFar();
            ReadEqm();
            ProcessFar();
            SmartUploadEquipmentsToServer();
        }

        private static void InitialUpload()
        {

            ReadFar();
            ReadEqm();
            ProcessFar();
            InitialUploadEquipmentListToServer();
        }

        private static void InitialUploadEquipmentListToServer()
        {
            EqpRepo.SmartUpdate(Equipments);
        }

        private static void UploadEmployees()
        {

            ReadEmployeeList();
            UpdateEmployeeListToServer();
        }
        private static void UpdateEmployeeListToServer()
        {
            Console.WriteLine();

            List<Employee> eListServer = EmpRepo.GetEmployees();
            List<Employee> EmployeesToUpdate = new List<Employee>();
            List<Employee> EmployeesToAdd = new List<Employee>();
            int Updates = 0;
            int Additions = 0;
            for (int i = 0; i < Employees.Count; i++)
            {
                try
                {
                    Employee eLocal = Employees[i];
                    Employee eServer = eListServer.SingleOrDefault((e) => e.EmployeeNumber == eLocal.EmployeeNumber);
                    if (eServer != null)
                    {
                        if (eServer.Name != eLocal.Name ||
                            eServer.Designation != eLocal.Designation || eServer.ContactNumber != eLocal.ContactNumber || eServer.Landline != eLocal.Landline || eServer.EmailAddress != eLocal.EmailAddress || eServer.GlobalNumber != eLocal.GlobalNumber)
                        {
                            eServer.Name = eLocal.Name;
                            eServer.Designation = eLocal.Designation;
                            eServer.ContactNumber = eLocal.ContactNumber;
                            eServer.Landline = eLocal.Landline;
                            eServer.EmailAddress = eLocal.EmailAddress;
                            eServer.GlobalNumber = eLocal.GlobalNumber;
                            EmployeesToUpdate.Add(eServer);
                        }
                    }
                    else
                    {
                        Additions++;
                        EmployeesToAdd.Add(eLocal);
                    }

                    Console.Write("\r FOUND " + Updates + " UPDATES & " + Additions + " ADDITIONS");
                }
                catch (Exception e)
                {
                    // Console.WriteLine(e.Message);
                }

            }
            Console.WriteLine("UPLOADING UPDATES TO SERVER");
            if (EmployeesToUpdate.Count > 0) EmpRepo.UpdateEmployees(EmployeesToUpdate);
            if (EmployeesToAdd.Count > 0) EmpRepo.AddEmployees(EmployeesToAdd);

        }



        private static void UpdateEquipmentListToServer()
        {
            Console.WriteLine();
            //List<Equipment> eListServer = new List<Equipment>();
            List<Equipment> eListServer = EqpRepo.GetEquipments();
            List<Equipment> EquipmentsToUpdate = new List<Equipment>();
            List<Equipment> EquipmentsToAdd = new List<Equipment>();
            int Updates = 0;
            int Additions = 0;
            for (int i = 0; i < Equipments.Count; i++)
            {
                try
                {
                    Equipment eLocal = Equipments[i];
                    Equipment eServer = eListServer.SingleOrDefault((e) => e.EquipmentNumber == eLocal.EquipmentNumber);
                    if (eServer!=null)
                    {
                        if (eServer.EquipmentDescription != eLocal.EquipmentDescription ||
                            eServer.AssetDescription != eLocal.AssetDescription || eServer.OperationId != eLocal.OperationId || eServer.SubType!=eLocal.SubType)
                        {
                            eServer.EquipmentDescription = eLocal.EquipmentDescription;
                            eServer.AssetDescription = eLocal.AssetDescription;
                            eServer.OperationId = eLocal.OperationId;
                            eServer.SubType = eLocal.SubType;
                            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                            eServer.TimeStamp = (long)((DateTime.Now.ToUniversalTime() - epoch).TotalMilliseconds);
                            EquipmentsToUpdate.Add(eServer);

                        }
                    }
                    else
                    {
                        
                        Additions++;
                        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        eLocal.TimeStamp = (long)((DateTime.Now.ToUniversalTime() - epoch).TotalMilliseconds);
                        EquipmentsToAdd.Add(eLocal); 
                    }

                    Console.Write("\r FOUND "+Updates+ " UPDATES & " +Additions + " ADDITIONS");
                }
                catch (Exception e)
                {
                  Console.WriteLine(e.Message);
                }

            }
            Console.WriteLine("UPLOADING UPDATES TO SERVER");
            if(EquipmentsToUpdate.Count>0)EqpRepo.UpdateEquipments(EquipmentsToUpdate);
            if (EquipmentsToAdd.Count > 0) EqpRepo.AddEquipments(EquipmentsToAdd);

        }


        private static void SmartUploadEquipmentsToServer()
        {
            Console.WriteLine();
           
            List<Equipment> EquipmentsToAdd = Equipments;
            int Additions = EquipmentsToAdd.Count;
            Console.WriteLine("UPLOADING UPDATES TO SERVER");
         
            if (EquipmentsToAdd.Count > 0) EqpRepo.SmartUpdate(EquipmentsToAdd);

        }


        private static void ProcessFar()
        {
            Console.WriteLine();
            var q = 1;
            for (var i = 0; i < Equipments.Count; i++)
            {
                var e = ETEMP.FirstOrDefault(m => m.AssetNumber == Equipments[i].AssetNumber);
                if (e != null)
                {
                    Equipments[i].EquipmentNumber = e.EquipmentNumber;
                    Equipments[i].EquipmentDescription = e.EquipmentDescription.Trim();
                   // Equipments[i].OperationId = e.OperationId;
                    Equipments[i].SubType = e.SubType;
                    Equipments[i].Weight = e.Weight;
                    Equipments[i].WeightUnit = e.WeightUnit;
                    Equipments[i].Dimensions = e.Dimensions;
                    Equipments[i].ModelNumber = e.ModelNumber;
                    Equipments[i].SerialNumber = e.SerialNumber;
                    Equipments[i].EquipmentLocation = Equipments[i].EquipmentLocation;



                    Console.Write("\r PROCESSING EQM AND FAR " + q);

                    q++;
               
                }
            }

        }

        private static void ReadEqm()
        {
            var fi = new FileInfo(EQM);
            using (var excelPackage = new ExcelPackage(fi))
            {
                var myWorkbook = excelPackage.Workbook;
                var myWorksheet = myWorkbook.Worksheets["Sheet1"];

                for (var i = 1; i < myWorksheet.Dimension.End.Row; i++)
                {
                    var e = new Equipment();
                    e.EquipmentNumber = myWorksheet.Cells[i, 1].Text.Trim();
                    e.AssetNumber = myWorksheet.Cells[i, 2].Text.Trim();

                    e.EquipmentDescription = myWorksheet.Cells[i, 3].Text.Trim();
                    e.OperationId = myWorksheet.Cells[i, 4].Text.Trim();
                    e.SubType = myWorksheet.Cells[i, 5].Text.Trim();
                    e.Weight = myWorksheet.Cells[i, 6].Text.Trim();
                    e.WeightUnit = myWorksheet.Cells[i, 7].Text.Trim();
                    e.Dimensions = myWorksheet.Cells[i, 8].Text.Trim();
                    e.ModelNumber = myWorksheet.Cells[i, 9].Text.Trim();
                    e.SerialNumber = myWorksheet.Cells[i, 10].Text.Trim();
                    e.EquipmentLocation = myWorksheet.Cells[i, 12].Text.Trim();


                    ETEMP.Add(e);

                    Console.Write("\r READING EQM " + i);
                }
            }
        }

        private static void ReadEmployeeList()
        {
            var fi = new FileInfo(EMP);
            using (var excelPackage = new ExcelPackage(fi))
            {
                var myWorkbook = excelPackage.Workbook;
                var myWorksheet = myWorkbook.Worksheets["Sheet1"];

                for (var i = 2; i < myWorksheet.Dimension.End.Row; i++)
                {
                    var e = new Employee();
                    e.EmployeeNumber = myWorksheet.Cells[i, 1].Text.Trim();
                    e.GlobalNumber = myWorksheet.Cells[i, 2].Text.Trim();
                    e.Name = myWorksheet.Cells[i, 3].Text.Trim();
                    e.Designation = myWorksheet.Cells[i, 4].Text.Trim();
                    e.Nationality = myWorksheet.Cells[i, 5].Text.Trim();
                    e.Project = myWorksheet.Cells[i, 6].Text.Trim();
                    e.ContactNumber = myWorksheet.Cells[i, 7].Text.Trim();
                    e.Landline = myWorksheet.Cells[i, 8].Text.Trim();
                    e.EmailAddress = myWorksheet.Cells[i, 9].Text.Trim();
                    e.JoiningDate = myWorksheet.Cells[i, 10].Text.Trim();
                   


                    Employees.Add(e);

                    Console.Write("\r READING EMPLOYEE LIST " + i);
                }
            }
        }


        private static void ReadFar()
        {
           
            var fi = new FileInfo(FAR);
            using (var excelPackage = new ExcelPackage(fi))
            {
                var myWorkbook = excelPackage.Workbook;
                var myWorksheet = myWorkbook.Worksheets["Sheet1"];
                Console.WriteLine();
                for (var i = 2; i < myWorksheet.Dimension.End.Row; i++)
                {
                    var e = new Equipment();
                    e.AcquisitionValue = float.Parse(myWorksheet.Cells[i, 6].Text.Trim().Replace(",", string.Empty));
                    e.BookValue = myWorksheet.Cells[i, 8].Text.Trim().Replace(",", string.Empty);
                    e.AcquisitionDate = DateTime.Parse(myWorksheet.Cells[i, 4].Value.ToString());
                    e.AssetNumber = myWorksheet.Cells[i, 1].Text.Trim();
                    e.EquipmentNumber = myWorksheet.Cells[i, 3].Text.Trim();
                    e.AssetDescription = myWorksheet.Cells[i, 5].Text.Trim();
                    e.AssetLocation = myWorksheet.Cells[i, 10].Text.Trim();
                    e.plant = myWorksheet.Cells[i, 14].Text.Trim();

                    if (e.AssetNumber != "" && e.AssetNumber != "Asset") Equipments.Add(e);
                    
                    Console.Write("\r READING FAR " + i);
                }
            }
        }
   
    }
}