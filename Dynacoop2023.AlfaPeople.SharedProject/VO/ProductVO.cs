using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dynacoop2023.AlfaPeople.SharedProject.VO
{
    public class ProductVO
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }
    }
}
