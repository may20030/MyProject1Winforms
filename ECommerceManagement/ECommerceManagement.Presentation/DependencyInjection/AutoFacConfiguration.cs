using Autofac;
using ECommerceManagement.Application.Interfaces.Repositories;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Application.Services;
using ECommerceManagement.Infrastructure.Data;
using ECommerceManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceManagement.Presentation.DependencyInjection
{
      public static class AutoFacConfiguration
   {
       public static IContainer Build()
       {
           var builder = new ContainerBuilder();

           // DbContext
           builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerLifetimeScope();

           // Generic Repositories
           builder.RegisterGeneric(typeof(GenericRepository<,>))
                  .As(typeof(IGenericRepository<,>))
                  .InstancePerLifetimeScope();

           // Services
           builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
           builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
           builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
           builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
           builder.RegisterType<CartItemService>().As<ICartItemService>().InstancePerLifetimeScope();

           return builder.Build();
       }
   }
}
