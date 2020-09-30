using POSFileParser.Models;
using SharpConfig;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace POSFileParser
{
    public static class Parser
    {
        public static void LoadFile()
        {
            Configuration.CultureInfo.DateTimeFormat.FullDateTimePattern = "yyyyMMddHHmmss";
            Configuration.CultureInfo.DateTimeFormat.LongDatePattern = "yyyyMMddHHmmss";
            Configuration.CultureInfo.DateTimeFormat.ShortDatePattern = "yyyyMMddHHmmss";
            Configuration.CultureInfo = CultureInfo.CurrentUICulture;
            var file = Configuration.LoadFromFile("C:\\Users\\omgit\\source\\repos\\FuelPOSToolkit\\POSFileParser\\Tests\\61111042.PU", Encoding.Unicode);
            
            var dayReport = ParsePUFile(file);

            Console.WriteLine(dayReport.Status.Open);
        }

        public static DayReportModel ParsePUFile(Configuration file)
        {
            DayReportModel dayReport = new DayReportModel();

            var dayR = file["STATUS"].ToObject<StatusModel>();

            foreach (var section in file)
            {
                switch (section.Name)
                {
                    case "STATUS":
                        dayReport.Status = Parse<StatusModel>(section).FirstOrDefault();
                        break;
                    case "HEADER_RECEIPT_INFO":
                        dayReport.HeaderReceipt = Parse<HeaderReceiptModel>(section).FirstOrDefault();
                        break;
                    case "MERCHANT_INFO":
                        dayReport.MerchantInfo = Parse<MerchantInfoModel>(section);
                        break;
                    case "START_FUEL_INFO":
                        dayReport.StartFuelInfo = Parse<FuelInfoModel>(section);
                        break;
                    case "FUEL_INFO":
                        dayReport.StartFuelInfo = Parse<FuelInfoModel>(section);
                        break;
                    case "ARTICLE_SOLD_INFO":
                        dayReport.ArticleSoldInfo = Parse<ArticleSoldInfoModel>(section);
                        break;
                    case "CURRENCY_INFO":
                        dayReport.CurrencyInfo = Parse<CurrencyInfoModel>(section);
                        break;
                    case "LION_LOYALTY":
                        dayReport.LionLoyalty = Parse<LionLoyaltyModel>(section);
                        break;
                    default:
                        break;
                }
            }

            return dayReport;
        }

        public static List<T> Parse<T>(Section section) where T : ICanParse, new()
        {
            List<T> items = new List<T>();
            T newItem = new T();
            bool first = true;

            foreach (var item in section)
            {
                string[] headers = item.Name.SplitKey();

                if (headers.Length == 1)
                {
                    newItem.AddToItem(headers, item.StringValue);
                    continue;
                }
                else if (headers.Length > 1)
                {
                    if (first)
                    {
                        newItem.IDKey = headers[1];
                        first = false;
                    }

                    if (headers[1] == newItem.IDKey)
                    {
                        newItem.AddToItem(headers, item.StringValue);
                    }
                    else
                    {
                        items.Add(newItem);
                        newItem = new T { IDKey = headers[1] };

                        newItem.AddToItem(headers, item.StringValue);
                    }
                }
                
            }

            items.Add(newItem);

            return items;
        }

        public static string[] SplitKey(this string item)
        {
            char[] matchOn = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            var subString = item.IndexOfAny(matchOn);

            if (subString != -1)
            {
                var output = item.Insert(subString, ",")
                .Split(',');
                return output;
            }
            else
            {
                string[] output = new string[] { item };
                return output;
            }
        }
    }
}
