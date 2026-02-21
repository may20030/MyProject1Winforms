using Autofac;
using ECommerceManagement.Application.Interfaces.Services;
using ECommerceManagement.Presentation;
using ECommerceManagement.Presentation.DependencyInjection;
using ECommerceManagement.Presentation.Forms.LoginForms;
using System;
using System.Windows.Forms;

namespace ECommerceManagement
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            // Build Autofac container
            var container = AutoFacConfiguration.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var userService = scope.Resolve<IUserService>();
                var cartService = scope.Resolve<ICartItemService>();

                // Run LoginForm
                System.Windows.Forms.Application.Run(new LoginForm(userService, cartService));
            }
        }
    }
}