﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LovelyLadoos.WebApp.Models
{
    public class ImageModel
    {
        public ImageModel(string predictionKeyHeader)
        {
            PredictionKeyHeader = predictionKeyHeader;
            ContentTypeHeader = "application/octet-stream";
        }

        public string ImageUrl { get; set; }
        public string PredictionKeyHeader { get; set; }
        public string ContentTypeHeader { get; set; }
        public IFormFile Body { get; set; }
    }
}
