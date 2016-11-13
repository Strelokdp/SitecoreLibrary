using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using SitecoreLibrary;
using Ninject.Web.Mvc;
using SitecoreLibrary.BAL.Contracts;
using SitecoreLibrary.BAL.Services;
using SitecoreLibrary.DAL.Contracts;
using SitecoreLibrary.DAL.Repository;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace SitecoreLibrary
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            kernel.Bind<IBookRepository>().To<BookRepository>();
            kernel.Bind<IBookHistoryRepository>().To<BookHistoryRepository>();
            kernel.Bind<IBookHistoryService>().To<BookHistoryService>();
            kernel.Bind<IBookService>().To<BookService>();
            kernel.Bind<IPostService>().To<PostService>();
        }
    }
}