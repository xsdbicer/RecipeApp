﻿using RecipeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Core.ViewModels.OutViewModels
{
    public class RecipeOutVM
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Score { get; set; }


        

        public string UserId { get; set; }
      

        public int CategoryId { get; set; }
     
    }
}