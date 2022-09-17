﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class Ward
    {
        [Key]
        public string code { get; set; }

        public string name { get; set; }
        public string name_en { get; set; }
        public string full_name { get; set; }
        public string full_name_en { get; set; }
        public string code_name { get; set; }
        public string district_code { get; set; }
        public string administrative_unit_id { get; set; }
    }
}
