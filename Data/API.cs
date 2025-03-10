﻿using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestKingdee.Data
{
    public class API
    {
        public class response
        {
            public int status { get; set; }
            public string message { get; set; }
            public int totalcount { get; set; }
            public List<Data> data { get; set; }
        }

        public class Data 
        {
            public int ID { get; set; }
            public string code { get; set; }
            public string name { get; set; }
            public int category_id { get; set; }
            public int brand_id { get; set; }
            public string barcode { get; set; }
            public string spec { get; set; }
            public int unit_id { get; set; }
            public string unit { get; set; }
            public decimal retail_price { get; set; }
            public decimal wholesale_price { get; set; }
            public decimal vip_price { get; set; }
            public decimal price4 { get; set; }
            public decimal price5 { get; set; }
            public decimal price6 { get; set; } 
            public decimal price7 { get; set; }
            public decimal tax_rate { get; set; }
            public decimal alias_name { get; set; }
            public string phonetic_code { get; set; }
            public string memo { get; set; }
            public decimal net_content { get; set; }
            public string origin_place { get; set; }
            public decimal weight { get; set; }
            public decimal volume { get; set; }
            public int disabled { get; set; }
            public int purchasing_cycle { get; set; }
            public int quality_period { get; set; }

        }
    }
}
