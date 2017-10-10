﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnglishForKidAPI.Models
{
    public class Category : BaseDataObject
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
    }
}