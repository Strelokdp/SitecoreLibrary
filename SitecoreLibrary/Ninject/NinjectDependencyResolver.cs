using Ninject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using SitecoreLibrary.Contracts;
using SitecoreLibrary.Repository;

namespace SitecoreLibrary.Ninject
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IBookRepository>().To<BookRepository>();
            kernel.Bind<IBookHistoryRepository>().To<BookHistoryRepository>();
        }
    }
}