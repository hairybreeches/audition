using System;
using Autofac;
using Model;
using NodaTime;
using Xero;

namespace Audition
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<XeroModule>();
            using (var container = builder.Build())
            {
                var searcher = container.Resolve<IJournalSearcher>();
                var journals =
                    searcher.FindJournalsWithin(new TimeFrame(DayOfWeek.Monday, DayOfWeek.Friday, new LocalTime(9, 0),
                        new LocalTime(5, 0)));

                Console.WriteLine(String.Join(",\r\n", journals));
                Console.WriteLine();
            }
        }

        
    }
}
