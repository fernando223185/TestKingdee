using System;
using System.Text;
using System.Text.Json;
using ServiceReference1;
using Newtonsoft.Json;
using TestKingdee.Data;
using Microsoft.Extensions.Configuration;


class Program
{
    static async Task Main(string[] args)
    {
        var client = new WS_AndroidSoapClient(WS_AndroidSoapClient.EndpointConfiguration.WS_AndroidSoap);
        try
        {
            IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())  
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) 
            .Build();

            string appId = config["SoapServiceConfig:AppID"];
            string appSecret = config["SoapServiceConfig:AppSecret"];

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var productRepo = new ProductRepository(configuration);
            var totalPages = 2;
            for (int i = 0; i <= totalPages; i++)
            {
                var jsonData = new
                {
                    PageNo = i,
                    PageSize = 5000
                };

                string filterJson = JsonConvert.SerializeObject(jsonData);

                MySoapHeader soapHeader = new MySoapHeader
                {
                    AppID = appId,
                    AppSecret = appSecret
                };

                var result = await client.GetMaterialAsync(soapHeader, filterJson);
                var data = JsonConvert.DeserializeObject<API.response>(result.GetMaterialResult);

                Console.WriteLine("JSON Response: " + JsonConvert.SerializeObject(data, Formatting.Indented));
                foreach (var row in data.data)
                {
                    Console.WriteLine("JSON Data: " + row);
                    bool success = await productRepo.InsertOrUpdateProduct(
                        id: row.ID,
                        code: row.code,
                        name: row.name,
                        categoryId: row.category_id,
                        brandId: row.brand_id,
                        barcode: row.barcode,
                        spec: row.spec,
                        unit: row.unit,
                        retailPrice: row.retail_price,
                        wholesalePrice: row.wholesale_price,
                        marketPrice: 0,
                        vipPrice: row.vip_price,
                        price4: row.price4,
                        price5: row.price5,
                        price6: row.price6,
                        price7: row.price7,
                        taxRate: row.tax_rate,
                        aliasName: row.alias_name.ToString(),
                        attribute: "",
                        phoneticCode: row.phonetic_code,
                        memo: row.memo,
                        netContent: row.net_content.ToString(),
                        originPlace: row.origin_place,
                        weight: row.weight,
                        volume: row.volume,
                        disabled: row.disabled,
                        purchasingCycle: row.purchasing_cycle,
                        qualityPeriod: row.quality_period,
                        externalId: "",
                        price8: 0

                        );

                    Console.WriteLine(success ? "Producto insertado/actualizado correctamente. " + row.code : "Error al procesar el producto." + row.code);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            client.Close();
        }
    }
}
