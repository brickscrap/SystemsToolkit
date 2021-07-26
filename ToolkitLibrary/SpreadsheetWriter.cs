using System;
using System.Collections.Generic;
using System.Text;
using FuelPOS.StatDevParser;
using FuelPOS.StatDevParser.Models;
using ToolkitLibrary.Spreadsheets;

namespace ToolkitLibrary
{
    public class SpreadsheetWriter
    {
        public void CreateFuelPOSSurvey(List<StatdevModel> data)
        {
            FuelPOSSurveySheet survey = new FuelPOSSurveySheet();

            survey.CreateSurvey(data);
        }
    }
}
