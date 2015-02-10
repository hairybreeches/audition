﻿using Audition.Chromium;
using Autofac;
using CsvExport;
using ExcelImport;
using Licensing;
using Native;
using Persistence;
using Sage50;
using SqlImport;
using UserData;
using Webapp;
using Webapp.Session;
using Module = Autofac.Module;

namespace Audition
{
    public class AuditionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<Sage50Module>();
            builder.RegisterModule<NativeModule>();
            builder.RegisterModule<ChromiumModule>();
            builder.RegisterModule<CsvExportModule>();            
            builder.RegisterModule<SessionModule>();            
            builder.RegisterModule<PersistenceModule>();
            builder.RegisterModule<HttpModule>();
            builder.RegisterModule<LicensingModule>();
            builder.RegisterModule<UserDataModule>();
            builder.RegisterModule<ExcelImportModule>();
            builder.RegisterModule<SqlImportModule>();
        }
    }
}