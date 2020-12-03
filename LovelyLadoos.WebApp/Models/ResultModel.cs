using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LovelyLadoos.WebApp.Models
{
    public class ResultModel
    {
        public string id { get; set; }
        public string project { get; set; }
        public string iteration { get; set; }
        public DateTime created { get; set; }
        public PredictionModel[] predictions { get; set; }

    }
}
