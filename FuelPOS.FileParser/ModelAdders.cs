using POSFileParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace POSFileParser
{
    public static class ModelAdders
    {
        public static FuelInfoModel AddToItem(this FuelInfoModel fuelInfo, string[] headers, string item)
        {
            switch (headers[0])
            {
                case "NAM":
                    fuelInfo.Name = item;
                    break;
                case "CRD":
                    fuelInfo.CardCode = int.Parse(item);
                    break;
                case "GRP":
                    fuelInfo.Group = int.Parse(item);
                    break;
                case "REP":
                    fuelInfo.ReportCode = int.Parse(item);
                    break;
                case "VAT":
                    fuelInfo.VatCode = int.Parse(item);
                    break;
                case "QUAUNIT":
                    fuelInfo.UnitQuantity = int.Parse(item);
                    break;
                case "BFL":
                    fuelInfo.Bfl = int.Parse(item);
                    break;
                case "BRATIO":
                    fuelInfo.BlendRatio = int.Parse(item);
                    break;
                case "BASEPRI":
                    fuelInfo.BasePrice = double.Parse(item);
                    break;
                case "ALTPRI":
                    fuelInfo.AltPrices.Add(double.Parse(item));
                    break;
                case "EXTREF":
                    fuelInfo.ExternalRef = item;
                    break;
                default:
                    break;
            }

            return fuelInfo;
        }
        public static ArticleSoldInfoModel AddToItem(this ArticleSoldInfoModel articleInfo, string[] headers, string item)
        {
            if (articleInfo.ArticleNumber == null)
            {
                articleInfo.ArticleNumber = headers[1];
            }
            switch (headers[0])
            {
                case "NAM":
                    articleInfo.Name = item;
                    return articleInfo;
                case "CRD":
                    articleInfo.CardCode = item;
                    return articleInfo;
                case "GRP":
                    articleInfo.Group = item;
                    return articleInfo;
                case "REP":
                    articleInfo.ReportGroup = item;
                    return articleInfo;
                case "VAT":
                    articleInfo.VatCode = int.Parse(item);
                    return articleInfo;
                case "PRI":
                    articleInfo.Price = double.Parse(item);
                    return articleInfo;
                case "QUAUNIT":
                    articleInfo.Quantity = int.Parse(item);
                    return articleInfo;
                default:
                    return articleInfo;
            }
        }
    }
}
