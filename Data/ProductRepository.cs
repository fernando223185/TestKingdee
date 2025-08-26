using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TestKingdee.Data
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> InsertOrUpdateProduct(
            int id, string code, string name, string categoryId, int brandId, string barcode, string spec, string unit,
            decimal retailPrice, decimal wholesalePrice, decimal marketPrice, decimal vipPrice, decimal price4, decimal price5,
            decimal price6, decimal price7, decimal taxRate, string aliasName, string attribute, string phoneticCode, string memo,
            string netContent, string originPlace, decimal weight, decimal volume, int disabled, int purchasingCycle, int qualityPeriod,
            string externalId, decimal price8, string branch_code, int branch_id, string branch_name)
        {
            Console.WriteLine($"Retail Price: {retailPrice}, Wholesale Price: {wholesalePrice}");

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_starnet_products_api", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@category_id", categoryId);
                    cmd.Parameters.AddWithValue("@brand_id", brandId);
                    cmd.Parameters.AddWithValue("@barcode", barcode);
                    cmd.Parameters.AddWithValue("@spec", spec);
                    cmd.Parameters.AddWithValue("@unit", unit);
                    cmd.Parameters.AddWithValue("@retail_price", retailPrice);
                    cmd.Parameters.AddWithValue("@wholesale_price", wholesalePrice);
                    cmd.Parameters.AddWithValue("@market_price", marketPrice);
                    cmd.Parameters.AddWithValue("@vip_price", vipPrice);
                    cmd.Parameters.AddWithValue("@price4", price4);
                    cmd.Parameters.AddWithValue("@price5", price5);
                    cmd.Parameters.AddWithValue("@price6", price6);
                    cmd.Parameters.AddWithValue("@price7", price7);
                    cmd.Parameters.AddWithValue("@tax_rate", taxRate);
                    cmd.Parameters.AddWithValue("@alias_name", aliasName);
                    cmd.Parameters.AddWithValue("@attribute", attribute);
                    cmd.Parameters.AddWithValue("@phonetic_code", phoneticCode);
                    cmd.Parameters.AddWithValue("@memo", memo);
                    cmd.Parameters.AddWithValue("@net_content", netContent);
                    cmd.Parameters.AddWithValue("@origin_place", originPlace);
                    cmd.Parameters.AddWithValue("@weight", weight);
                    cmd.Parameters.AddWithValue("@volume", volume);
                    cmd.Parameters.AddWithValue("@disabled", disabled);
                    cmd.Parameters.AddWithValue("@purchasing_cycle", purchasingCycle);
                    cmd.Parameters.AddWithValue("@quality_period", qualityPeriod);
                    cmd.Parameters.AddWithValue("@external_id", externalId);
                    cmd.Parameters.AddWithValue("@price8", price8);
                    cmd.Parameters.AddWithValue("@branch_code", branch_code);
                    cmd.Parameters.AddWithValue("@branch_id", branch_id);
                    cmd.Parameters.AddWithValue("@branch_name", branch_name);


                    try
                    {
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return false;
                    }
                }
            }
        }
    }
}
