using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Google.Cloud.Firestore;

namespace Server.Models
{
    [FirestoreData]
    public class Equipment
    {
        [Key] public int id { get; set; }
        [FirestoreProperty]
        public string EquipmentNumber { get; set; }
        [FirestoreProperty]
        public string AssetNumber { get; set; }
        
        public DateTime AcquisitionDate { get; set; }
        [FirestoreProperty]
        public bool PendingUpdate { get; set; }
        [FirestoreProperty]
        public float AcquisitionValue { get; set; }
        [FirestoreProperty]
        public string BookValue { get; set; }
        [FirestoreProperty]
        public string AssetDescription { get; set; }
        [FirestoreProperty]
        public string EquipmentDescription { get; set; }
        [FirestoreProperty]
        public string OperationId { get; set; }
        [FirestoreProperty]
        public string SubType { get; set; }
        [FirestoreProperty]
        public string Weight { get; set; }
        [FirestoreProperty]
        public string WeightUnit { get; set; }
        [FirestoreProperty]
        public string Dimensions { get; set; }
        [FirestoreProperty]
        public string Tag { get; set; }
        [FirestoreProperty]
        public string Type { get; set; }
        [FirestoreProperty]
        public string Connection { get; set; }
        [FirestoreProperty]
        public string Length { get; set; }
        [FirestoreProperty]
        public string ModelNumber { get; set; }
        [FirestoreProperty]
        public string SerialNumber { get; set; }
        [FirestoreProperty]
        public string AssetLocation { get; set; }
        [FirestoreProperty]
        public string AssetLocationText { get; set; }
        [FirestoreProperty]
        public string EquipmentLocation { get; set; }
        [FirestoreProperty]
        public long TimeStamp { get; set; }
    }
}