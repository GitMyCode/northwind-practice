using System;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Northwind2.Application.Customers.Commands.CreateCustomer;
using Northwind2.Application.Customers.Queries.GetCustomerDetails;
using Northwind2.Application.Exceptions;
using Northwind2.Persistence;

namespace Northwind2.ConsoleApp
{
    class Program
    {
        static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            // request & notification handlers
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.Register<NorthwindDbContext>((c, p) =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<NorthwindDbContext>();

                optionsBuilder.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=NorthwindTraders;Trusted_Connection=True;Application Name=NorthwindTraders;");

                return new NorthwindDbContext(optionsBuilder.Options);
            });

            // finally register our custom code (individually, or via assembly scanning)
            // - requests & handlers as transient, i.e. InstancePerDependency()
            // - pre/post-processors as scoped/per-request, i.e. InstancePerLifetimeScope()
            // - behaviors as transient, i.e. InstancePerDependency()
            builder.RegisterAssemblyTypes(typeof(CreateCustomerCommand).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan
            //builder.RegisterType<MyHandler>().AsImplementedInterfaces().InstancePerDependency();          // or individually  Console.WriteLine("Hello World!");
            return builder.Build();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("start");
            Task.Run(MainAsync).Wait();
            Console.WriteLine("Finish");
            Console.Read();
        }

        static async Task MainAsync()
        {
            var container = BuildContainer();

            var mediator = container.Resolve<IMediator>();
            //await mediator.Send(new CreateCustomerCommand
            //{
            //    Id = "12345",
            //    Address = "allo",
            //    CompanyName = "company",
            //    Country = "country",
            //    Region = "region",
            //    City = "sdsdf",
            //    ContactName = "Moi",
            //    ContactTitle = "Chose"
            //});

            try
            {
                var model = await mediator.Send(new GetCustomerDetailQuery
                {
                    Id = "12344"
                });

                Console.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
