using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynacoop2023.AlfaPeople.MyFirstPlugins.DynacoopISV
{
    public abstract class PluginCore : IPlugin
    {
        public IPluginExecutionContext Context { get; set; }
        public IOrganizationServiceFactory ServiceFactory { get; set; }
        public IOrganizationService Service { get; set; }
        public ITracingService TracingService { get; set; }

        public void Execute(IServiceProvider serviceProvider)
        {
            this.Context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            this.ServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            this.Service = this.ServiceFactory.CreateOrganizationService(this.Context.UserId);
            this.TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            ExecutePlugin(serviceProvider);
        }

        public abstract void ExecutePlugin(IServiceProvider serviceProvider);
    }
}
