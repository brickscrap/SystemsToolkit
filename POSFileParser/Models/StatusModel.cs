﻿using AutoMapper;
using Microsoft.VisualBasic;
using POSFileParser.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace POSFileParser.Models
{
    public class StatusModel : ICanParse
    {
        public string IDKey { get; set; }
        private DateTime _open;
        public DateTime Open
        {
            get { return _open; }
            set { _open = value; }
        }
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
        public List<string> StationAddress { get; set; } = new List<string>();
        public string PostCode { get; set; }
        public string City { get; set; }
        [FieldName("FDATI")]
        public DateTime LastFuelSale { get; set; }
        [FieldName("SDATI")]
        public DateTime LastArticleSale { get; set; }
        public int ReportNumber { get; set; }
        public int AccountingDayReportNumber { get; set; }
        public int VATCountryCode { get; set; }
        public int NumberOfPOS { get; set; }

        public void AddToItem(string[] headers, string value)
        {
            Mapper.MapFile(this, headers[0], value, this);

            /*
            switch (headers[0])
            {
                case "OPEN":
                    Open = value.ParseFuelPOSDate();
                    break;
                case "CLOS":
                    Close = value.ParseFuelPOSDate();
                    break;
                case "POSDISCONNECTED":
                    POSDisconnected = value.Equals("YES");
                    break;
                case "CLOSE_TYPE":
                    ClosureType = int.Parse(value);
                    break;
                case "SWV":
                    SoftwareVersion = value;
                    break;
                case "STID":
                    StationID = value;
                    break;
                case "STNAME":
                    StationName = value;
                    break;
                case "STADDRESS":
                    StationAddress.Add(value);
                    break;
                case "STZIP":
                    PostCode = value;
                    break;
                case "STCITY":
                    City = value;
                    break;
                case "FDATI":
                    LastFuelSale = value.ParseFuelPOSDate();
                    break;
                case "SDATI":
                    LastArticleSale = value.ParseFuelPOSDate();
                    break;
                case "REP_NR":
                    ReportNumber = int.Parse(value);
                    break;
                case "ACC_REP_NR":
                    AccountingDayReportNumber = int.Parse(value);
                    break;
                case "VAT_ISO_C":
                    VATCountryCode = int.Parse(value);
                    break;
                case "NRPOSSES":
                    NumberOfPOS = int.Parse(value);
                    break;
                default:
                    break;
            }
            */
        }
    }
}
