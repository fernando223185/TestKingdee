using System;
using System.Text.Json;
using Newtonsoft.Json;
using TestKingdee.Data;
using Microsoft.Extensions.Configuration;
using ServiceReference1;
using System.Threading.Tasks;

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
            int pageNo = 1;
            int pageSize = 100; // Pon un valor razonable para el tamaño de página

            bool hayMasPaginas = true;

            while (hayMasPaginas)
            {
                var jsonData = new
                {
                    PageNo = pageNo,
                    PageSize = pageSize,
                    ApproveDateStamp = "2020-01-01"
                };

                string filterJson = JsonConvert.SerializeObject(jsonData);
                Console.WriteLine($"Solicitando página {pageNo}");

                MySoapHeader soapHeader = new MySoapHeader
                {
                    AppID = appId,
                    AppSecret = appSecret
                };

                var result = await client.GetMaterialAsync(soapHeader, filterJson);

                var data = JsonConvert.DeserializeObject<API.response>(result.GetMaterialResult);

                if (data == null || data.data == null || data.data.Count == 0)
                {
                    Console.WriteLine("No hay más datos.");
                    break;
                }

                /*string rutaArchivo = "C:/Users/PRY-005/jsonResponse_materials.txt";
                string contenido = JsonConvert.SerializeObject(data.data, Formatting.Indented);
                File.AppendAllText(rutaArchivo, $"--- Página {pageNo} ---\n{contenido}\n\n");*/

                Console.WriteLine($"Procesando {data.data.Count} registros en página {pageNo}");

                foreach (var row in data.data)
                {
                    decimal retailPrice = row.MENUDEO;
                    decimal wholesalePrice = row.MAYOREO;  
                    decimal p6a_CAJA = row.CAJA_GENERAL;
                    decimal memberprice = row.MIEMBRO;
                    decimal viPprice = row.VIP;
                    decimal boxprice = row.Box_price;
                    decimal premium = row.PREMIUM;
                    decimal diamante = row.Diamante;
                    decimal diamanteEsp = row.Diamante_Esp; 
                    bool success = await productRepo.InsertOrUpdateProduct(
                        id: row.ID,
                        code: row.code,
                        name: row.name,
                        categoryId: row.category_id, // lo dejas fijo si quieres
                        brandId: row.brand_id,
                        barcode: row.barcode,
                        spec: row.spec,
                        unit: row.unit,
                        retailPrice: retailPrice,
                        wholesalePrice: wholesalePrice,
                        p6a_CAJA: p6a_CAJA,
                        marketPrice: 0,
                        vipPrice: memberprice,
                        price4: boxprice,
                        price5: viPprice,
                        price6: premium,
                        price7: diamante,
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
                        price8: diamanteEsp,
                        branch_code: row.branch_code,
                        branch_id: row.branch_id,
                        branch_name: row.branch_Name
                    );

                    Console.WriteLine(success
                        ? $"✅ Producto {row.code} procesado correctamente."
                        : $"❌ Error al procesar producto {row.code}.");
                } 

                int totalcount = data.totalcount;
                int totalPages = (int)Math.Ceiling((double)totalcount / pageSize);

                if (pageNo >= totalPages)
                {
                    hayMasPaginas = false;
                }
                else
                {
                    pageNo++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error: " + ex.Message);
        }
        finally
        {
            await client.CloseAsync();
        }
    }
}
