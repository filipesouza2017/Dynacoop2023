using Dynacoop2023.AlfaPeople.MyFirstPlugins.DynacoopISV;
using Dynacoop2023.AlfaPeople.SharedProject.VO;
using Microsoft.Xrm.Sdk.Workflow;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynacoop2023.AlfaPeople.SharedProject.Extensions;

namespace Dynacoop2023.AlfaPeople.MyFirstPlugins.Actions
{
    public class BuscaNomedoProdutonaAPIExterna : ActionCore
    {
        [Input("ProductId")]
        public InArgument<string> ProductId { get; set; }

        [Output("ProductName")]
        public OutArgument<string> ProductName { get; set; }

        public string Log { get; set; }

        public override void ExecuteAction(CodeActivityContext context)
        {
            try
            {
                this.Log += "Entrou no processo";

                RestResponse response = GetProductsOnAPI();
                ProductVO productFound = GetProductWithID(context, response);

                this.Log += "Setando valor";

                ProductName.Set(context, productFound.ProductName);
            }
            catch(Exception ex)
            {
                throw new Exception(this.Log + " - " + ex.ToString());
            }
        }

        private ProductVO GetProductWithID(CodeActivityContext context, RestResponse response)
        {
            this.Log += "GetProductWithID";

            this.Log += response.Content;

            List<ProductVO> productsVO = JsonConvert.DeserializeObject<List<ProductVO>>(response.Content);

            //List<ProductVO> productsVO = new List<ProductVO>();
            //productsVO.Add(new ProductVO()
            //{
            //    ProductId = "PROD-0001",
            //    ProductName = "Box"
            //});

            this.Log += "Converteu JSON";

            try
            {
                var productFound = (from p in productsVO
                                    where p.ProductId == ProductId.Get(context)
                                    select p).ToList().FirstOrDefault();

                if (productFound == null)
                {
                    throw new Exception("Produto com esse ID não encontrado");
                }

                return productFound;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel ler o Parametro Product Id");
            }
        }

        private RestResponse GetProductsOnAPI()
        {
            this.Log += "GetProductsOnAPI";

            var options = new RestClientOptions("https://dynaccop2023-productapi.azurewebsites.net")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("/product", Method.Post);
            RestResponse response = client.Execute(request);
            return response;
        }
    }
}
