using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TireProject
{
    public class ReportData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string TermCondition { get; set; }
        public string RefNo { get; set; }
        public string Date { get; set; }
        public string ExtraRefNo { get; set; }
        public string ExtraDate { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string HomeNo { get; set; }
        public string WorkNo { get; set; }
        public string Email { get; set; }
        public string PlateNo { get; set; }
        public string CarYear { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public ETireSeason TireSeason { get; set; }
        public string NoOfTires { get; set; }
        public string TireSize1 { get; set; }
        public string TireSize2 { get; set; }
        public string TireSize3 { get; set; }
        public string TireSize4 { get; set; }
        public EIfStag IfStaggered { get; set; }
        public string MakeModel { get; set; }
        public ERimAttached RimAttached { get; set; }
        public ERimTypes TypeRim { get; set; }
        public string DepthLF { get; set; }
        public string DepthLR { get; set; }
        public string DepthRF { get; set; }
        public string DepthRR { get; set; }
        public string BarcodeLF { get; set; }
        public string BarcodeLR { get; set; }
        public string BarcodeRF { get; set; }
        public string BarcodeRR { get; set; }
        public string TireStoredUpto { get; set; }
        public string REP { get; set; }
        public string Pic1 { get; set; }
        public string Pic2 { get; set; }
        public string Pic3 { get; set; }
        public string Pic4 { get; set; }
        public string CustomerSign { get; set; }
        public string CusSignDate { get; set; }
    }
    public enum ETireSeason
    {
        AllSeason,
        Winter,
        Summer,
        Other
    }
    public enum ERimAttached
    {
        Yes,
        No
    }
    public enum EIfStag
    {
        Yes,
        No
    }
    public enum ERimTypes
    {
        Steel,
        Alloy,
        OEM,
        Other
    }

    public class TempDataPass
    {
        public string datefrom { get; set; }
        public string dateto { get; set; }
        public int ii { get; set; }
        public int eTireSeason { get; set; }
        public string warehouse { get; set; }

        public TempDataPass(string datefrom, string dateto, int ii, int eTireSeason, string warehouse)
        {
            this.datefrom = datefrom;
            this.dateto = dateto;
            this.ii = ii;
            this.eTireSeason = eTireSeason;
            this.warehouse = warehouse;
        }
    }
}
