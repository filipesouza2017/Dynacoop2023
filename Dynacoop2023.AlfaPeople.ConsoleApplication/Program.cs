using Dynacoop2023.AlfaPeople.ConsoleApplication.Controller;
using Dynacoop2023.AlfaPeople.ConsoleApplication.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynacoop2023.AlfaPeople.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            CrmServiceClient serviceClient = Singleton.GetService();

            ContaController contaController = new ContaController(serviceClient);

            EntityCollection accountsToExecute = new EntityCollection();

            Entity account1 = new Entity("account");
            account1["name"] = "Conta 1";
            accountsToExecute.Entities.Add(account1);

            Entity account2 = new Entity("account", new Guid("81883308-7ad5-ea11-a813-000d3a33f3b4"));
            account2["name"] = "Conta 2";
            accountsToExecute.Entities.Add(account2);

            contaController.UpsertMultipleRequest(accountsToExecute);

            Console.WriteLine("Contas Criadas e Atualizadas com Sucesso");

            Console.ReadKey();
        }

        private static void RetrieveMethods(ContaController contaController)
        {
            Console.WriteLine("1 - Pesquisar uma conta por id");
            Console.WriteLine("2 - Pesquisar uma conta por nome");
            Console.WriteLine("3 - Pesquisar uma conta por nome do contato");
            Console.WriteLine("4 - Pesquisar uma conta por telefone");
            Console.WriteLine("5 - Pesquisar várias contas");

            var answer = Console.ReadLine();

            if (answer == "1")
            {
                Console.WriteLine("Qual o id da conta que você deseja pesquisar");
                var accountId = Console.ReadLine();
                Entity account = contaController.GetAccountById(new Guid(accountId));
                ShowAccountName(account);
            }
            else
            {
                if (answer == "2")
                {
                    Console.WriteLine("Qual o nome da conta que você deseja pesquisar");
                    var name = Console.ReadLine();
                    Entity account = contaController.GetAccountByName(name);
                    Console.WriteLine($"O telefone da conta recuperada é {account["telephone1"].ToString()}");
                }
                else
                {
                    if (answer == "3")
                    {
                        Console.WriteLine("Qual o nome do contato relacionado a conta que você deseja pesquisar");
                        var name = Console.ReadLine();
                        Entity account = contaController.GetAccountByContactName(name, new string[] { "name" });
                        ShowAccountName(account);
                    }
                    else
                    {
                        if (answer == "4")
                        {
                            Console.WriteLine("Qual o telefone da conta que você deseja pesquisar");
                            var telephone = Console.ReadLine();
                            Entity account = contaController.GetAccountByTelephone(telephone);
                            ShowAccountName(account);
                        }
                        else
                        {
                            if (answer == "5")
                            {
                                Console.WriteLine("A conta que você pesquisa, começa com?");
                                var like = Console.ReadLine();
                                EntityCollection accounts = contaController.GetAccountByLike(like);

                                foreach (Entity account in accounts.Entities)
                                {
                                    Console.WriteLine(account["name"].ToString());
                                }
                            }
                            else
                            {
                                Console.WriteLine("Opção inválida, reinicie o aplicativo");
                            }
                        }
                    }
                }
            }
        }

        private static void ShowAccountName(Entity account)
        {
            Console.WriteLine($"A conta recuperada se chama {account["name"].ToString()}");
        }

        private static void CreateUpdateDelete(ContaController contaController)
        {
            Console.WriteLine("Digite 1 para Create/Update");
            Console.WriteLine("Digite 2 para Delete");

            var answerWhatToDo = Console.ReadLine();

            if (answerWhatToDo.ToString() == "1")
            {
                MakeCreateAndUpdate(contaController);
            }
            else
            {
                if (answerWhatToDo.ToString() == "2")
                {
                    MakeDelete(contaController);
                }
                else
                {
                    Console.WriteLine("Opção inválida, reinicie o aplicativo");
                }
            }
        }

        private static void MakeDelete(ContaController contaController)
        {
            Console.WriteLine("Digite o id da conta que você quer deletar");
            var accountId = Console.ReadLine();
            contaController.Delete(new Guid(accountId));
            Console.WriteLine("Deletado com sucesso!");
        }

        private static void MakeCreateAndUpdate(ContaController contaController)
        {
            Console.WriteLine("Aguarde enquanto a nova Conta é criada");
            Guid accountId = contaController.Create();
            Console.WriteLine("Conta criada com sucesso");

            Console.WriteLine($"https://dynacoop2023.crm2.dynamics.com/main.aspx?appid=4d306bb3-f4a9-ed11-9885-000d3a888f48&pagetype=entityrecord&etn=account&id={accountId}");

            Console.WriteLine("Deseja fazer a atualização da conta recém criada? (S/N)");
            var answerToUpdate = Console.ReadLine();

            if (answerToUpdate.ToString().ToUpper() == "S")
            {
                Console.WriteLine("Por favor informe o novo telefone");
                var newTelephone = Console.ReadLine();
                bool contaAtualizada = contaController.Update(accountId, newTelephone);

                if (contaAtualizada)
                    Console.WriteLine("Conta atualizada com sucesso");
                else
                    Console.WriteLine("Erro na atualização da conta");
            }
        }
    }
}
