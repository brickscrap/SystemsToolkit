using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Xml;
using System.Drawing;
using OfficeOpenXml.Style;

namespace SpreadsheetUtils
{
    public class SpreadsheetWriter
    {
        public void CreateFuelPOSSurvey()
        {
            FuelPOSSurveySheet survey = new FuelPOSSurveySheet();

            survey.CreateSurvey();
        }
    }
}
