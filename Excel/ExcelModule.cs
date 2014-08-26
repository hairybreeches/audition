﻿using Autofac;

namespace Excel
{
    public class ExcelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExcelExporter>();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
        }
    }
}