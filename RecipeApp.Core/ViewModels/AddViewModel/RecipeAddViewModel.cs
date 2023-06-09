﻿using RecipeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Core.ViewModels.AddViewModel
{
    public class RecipeAddViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Ingredients { get; set; }


    }
}
