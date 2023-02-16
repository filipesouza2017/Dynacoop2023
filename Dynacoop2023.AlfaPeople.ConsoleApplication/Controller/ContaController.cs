using Dynacoop2023.AlfaPeople.ConsoleApplication.Model;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Rest;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynacoop2023.AlfaPeople.ConsoleApplication.Controller
{
    public class ContaController
    {
        public CrmServiceClient ServiceClient { get; set; }
        public Conta Conta { get; set; }

        public ContaController(CrmServiceClient crmServiceCliente)
        {
            ServiceClient = crmServiceCliente;
            this.Conta = new Conta(ServiceClient);
        }

        public Guid Create()
        {
            return Conta.Create();
        }

        public bool Update(Guid accountId, string telephone1)
        {
            return Conta.Update(accountId, telephone1);
        }

        public bool Delete(Guid accountId)
        {
            return Conta.Delete(accountId);
        }

        public Entity GetAccountById(Guid id)
        {
            return Conta.GetAccountById(id);
        }

        public Entity GetAccountByName(string name)
        {
            return Conta.GetAccountByName(name);
        }

        public Entity GetAccountByContactName(string name, string[] columns)
        {
            return Conta.GetAccountByContactName(name, columns);
        }

        public Entity GetAccountByTelephone(string telephone)
        {
            return Conta.GetAccountByTelephone(telephone);
        }

        public EntityCollection GetAccountByLike(string like)
        {
            return Conta.GetAccountByLike(like);
        }

        public void UpsertMultipleRequest(EntityCollection entityCollection)
        {
            Conta.UpsertMultipleRequest(entityCollection);
        }
    }
}
