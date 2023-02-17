using Dynacoop2023.AlfaPeople.ConsoleApplication.Controller;
using Dynacoop2023.AlfaPeople.ConsoleApplication.Model;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynacoop2023.AlfaPeople.MyFirstPlugins
{
    //.net FRAMEWORK 4.6.2
    public class AccountManager : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity opportunity = (Entity)context.InputParameters["Target"];

            EntityReference accountReference = opportunity.Contains("parentaccountid") ? (EntityReference)opportunity["parentaccountid"] : null;

            tracingService.Trace("Iniciou processo do Plugin");

            if(accountReference != null)
            {
                tracingService.Trace("Referência de conta encontrada");
                ContaController contaController = new ContaController(service);
                Entity oppAccount = contaController.GetAccountById(accountReference.Id, new string[] { "dcp_nmr_total_opp" });
                contaController.IncrementNumberOfOpp(oppAccount);
                tracingService.Trace("Conta atualizada");
            }
        }
    }
}
