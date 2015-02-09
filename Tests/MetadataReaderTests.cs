using System.Collections;
using System.Collections.Generic;
using Autofac.Builder;
using ExcelImport;
using Model;
using Native;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MetadataReaderTests
    {
        private const string Examplesage50Export = "..\\..\\..\\ExcelImport\\ExampleSage50Export.xlsx";


        [Test]
        public void CanReadMetadata()
        {
            var reader = CreateReader();
            var metadata = reader.ReadSheets(Examplesage50Export);
            CollectionAssert.AreEqual(new []
            {
                new SheetMetadata("Sage 50 journals export", 
                    new[]
            {
                "Column A",
                "Column B",
                "Column C",
                "Column D",
                "Column E",
                "Column F",
                "Column G",
                "Column H",
                "Column I",
                "Column J",
                "Column K",
                "Column L",
                "Column M",
                "Column N",
                "Column O",
                "Column P",
                "Column Q",
                "Column R",
                "Column S",
                "Column T",
            }, 
            new[]
            {
                "No",
                "Type",
                "Account",
                "Nominal",
                "Dept",
                "Details",
                "Date",
                "Ref",
                "Ex.Ref",
                "Net",
                "Tax",
                "T/C",
                "Paid",
                "Amount Paid",
                "Bank",
                "Bank Rec Date",
                "VAT",
                "VAT Rtn No.",
                "VAT Rec Date",
                "User Name",
            }),
            new SheetMetadata("Unrelated data", 
                new[]{"Column A","Column B","Column C","Column D","Column E"}, 
                new[]{"Date", "Widgets bought", "Divisional output", "Widgets sold", "Overtime"}), 
            },
            metadata);
        }      
  
       private static MetadataReader CreateReader()
       {
           return new MetadataReader(new ExcelColumnNamer(), new ExcelToSqlDataConverter(new FileSystem()));
       }       

    }    
}
