﻿using System;

namespace Webapp
{
    //todo: share these routes between js and c# using CEF
    public static class Routing
    {

        public const string GetExcelSheets = "api/excel/getSheets";
        public const string GetExcelHeaders = "api/excel/getHeaders";
        public const string ExcelImport = "api/excel/import";

        public const string Sage50Import = "api/sage50/import";

        public const string GetLicence = "api/licence/get";
        public const string UpdateLicence = "api/licence/update";

        public const string NominalCodesSearch = "api/search/nominalCodes";
        public const string NominalCodesExport = "api/export/nominalCodes";
        public const string UserSearch = "api/search/users";
        public const string UserExport = "api/export/users";  
        public const string EndingSearch = "api/search/ending";
        public const string EndingExport = "api/export/ending";
        public const string DuplicatesSearch = "api/search/duplicates";
        public const string DuplicatesExport = "api/export/duplicates";

        public const string Sage50DataLocations = "api/userdata/sage50DataLocations";
        public const string ExcelDataFiles = "api/userdata/excelDataFiles";

        public const string SearchCapability = "api/session/searchCapability";
        public const string ClearImport = "api/session/clearImport";


        public const string Openfile = "api/openfile";        
        public const string ShowDevTools = "api/devtools";
        public const string ChooseDirectory = "api/chooseDirectory";
        public const string ChooseExcelFile = "api/chooseExcelFile";

        public const string InternalDomain = @"http://localhost:1337";        

        public static string AddInternalDomain(string address)
        {
            return String.Format(@"{0}/{1}", InternalDomain, address);
        }

        public static string GetViewUrl(string location)
        {
            return AddInternalDomain("views/" + location);
        }
    }    
}
