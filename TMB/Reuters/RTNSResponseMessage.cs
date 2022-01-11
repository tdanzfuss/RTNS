using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace TMB.Reuters
{
    public class RTNSResponseMessage
    {
        private string responseString;

        public bool Success { get; set; }
        public string Reference { get; set; }
        public string ErrorName { get; set; }
        public string ErrorDescription { get; set; }
        public string RTNSResponseStatus { get; set; }


        public RTNSResponseMessage(string response)
        {
            responseString = response;
            XDocument responsedoc = XDocument.Parse(response);

            var responseResult = responsedoc.Element("RWP_1")
                .Element("RESULT");

            RTNSResponseStatus = responseResult.Element("STATUS").Value;
            Success = (RTNSResponseStatus == "SUCCESS");
            ErrorName = (responseResult.Element("ERROR_NAME") == null)
                ? string.Empty
                : responseResult.Element("ERROR_NAME").Value;

            ErrorDescription = (responseResult.Element("ERROR_DESCRIPTION") == null)
                ? string.Empty
                : responseResult.Element("ERROR_DESCRIPTION").Value;

            Reference = (responseResult.Element("REFERENCE") == null)
                ? string.Empty
                : responseResult.Element("REFERENCE").Value;
        }

        public RTNSResponseMessage(bool success, string errorName, string errorMessage)
        {
            Success = success;
            RTNSResponseStatus = string.Empty;
            Reference = string.Empty;
            ErrorName = errorName;
            ErrorDescription = errorMessage;
        }
    }
}
