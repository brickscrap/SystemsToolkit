using AutoMapper;
using Microsoft.VisualBasic;
using POSFileParser.Attributes;
using SharpConfig;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace POSFileParser.Models
{
    public class StatusModel : IFileSection
    {
        #region Public Properties
        public string IDKey { get; set; }
        [FieldName("OPEN")]
        public DateTime Open { get; set; }
        [FieldName("CLOSE")]
        public DateTime Close { get; set; }
        [FieldName("POSDISCONNECTED")]
        public bool POSDisconnected { get; set; } = false;
        [FieldName("CLOSE_TYPE")]
        public int ClosureType { get; set; }
        [FieldName("SWVER")]
        public string SoftwareVersion { get; set; }
        [FieldName("STID")]
        public string StationID { get; set; }
        [FieldName("STNAME")]
        public string StationName { get; set; }
        [FieldName("STADDRESS")]
        public AddressModel StationAddress { get; set; } = new AddressModel();
        public string PostCode { get; set; }
        public string City { get; set; }
        [FieldName("FDATI")]
        public DateTime LastFuelSale { get; set; }
        [FieldName("SDATI")]
        public DateTime LastArticleSale { get; set; }
        public int ReportNumber { get; set; }
        public int AccountingDayReportNumber { get; set; }
        public int VATCountryCode { get; set; }
        public int VatNumber { get; set; }
        public int NumberOfPOS { get; set; }
        #endregion

        private static readonly IDictionary<string, Func<StatusModel, string, StatusModel>> _mappings = new Dictionary<string, Func<StatusModel, string, StatusModel>>
        {
            { "OPEN", (model, value) => { model.Open = value.ParseFuelPOSDate(); return model; } },
            { "CLOS", (model, value) => { model.Close = value.ParseFuelPOSDate(); return model; } },
            { "POSDISCONNECTED", (model, value) => { model.POSDisconnected = value.StringToBool(); return model; } },
            { "CLOSE_TYPE", (model, value) => { model.ClosureType = int.Parse(value); return model; } },
            { "SWV", (model, value) => { model.SoftwareVersion = value; return model; } },
            { "STID", (model, value) => { model.StationID = value; return model; } },
            { "STNAME", (model, value) => { model.StationName = value; return model; } },
            { "STADDRESS1", (model, value) => { model.StationAddress.Line1 = value; return model; } },
            { "STADDRESS2", (model, value) => { model.StationAddress.Line2 = value; return model; } },
            { "STZIP", (model, value) => { model.StationAddress.PostCode = value; return model; } },
            { "STCITY", (model, value) => { model.StationAddress.City = value; return model; } },
            { "FDATI", (model, value) => { model.LastFuelSale = value.ParseFuelPOSDate(); return model; } },
            { "SDATI", (model, value) => { model.LastArticleSale = value.ParseFuelPOSDate(); return model; } },
            { "REP_NR", (model, value) => { model.ReportNumber = int.Parse(value); return model; } },
            { "ACC_REP_NR", (model, value) => { model.AccountingDayReportNumber = int.Parse(value); return model; } },
            { "VAT_ISO_C", (model, value) => { model.VATCountryCode = int.Parse(value); return model; } },
            { "VAT_NR", (model, value) => { model.VatNumber = int.Parse(value); return model; } },
            { "NRPOSSES", (model, value) => { model.NumberOfPOS = int.Parse(value); return model; } }
        };

        public IFileSection Create(IGrouping<string, Setting> groupedData)
        {
            foreach (var item in groupedData)
            {
                Func<StatusModel, string, StatusModel> function;
                // TODO: THIS DOESNT WORK ANYMORE
                if (_mappings.TryGetValue(item.Name, out function))
                {
                    function(this, item.Name);
                }
            }

            return this;
        }
    }
}
