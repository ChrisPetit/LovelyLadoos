using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LovelyLadoos.WebApp.Models
{
    public class PredictionModel
    {
        public float probability { get; set; }
        public string tagId { get; set; }
        public string tagName { get; set; }
    }
}
