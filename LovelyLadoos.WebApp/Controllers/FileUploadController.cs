using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LovelyLadoos.WebApp.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Newtonsoft.Json;


namespace LovelyLadoos.WebApp.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IConfiguration _configuration;

        public FileUploadController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> Result(List<IFormFile> files)
        {

            var responseBody = "";

            foreach (var formFile in files.Where(formFile => formFile.Length > 0))
            {
                var fileBytes = await CreateFileBytesArray(formFile);

                // process uploaded files
                var image = CreateImageModel(formFile);

                var byteArrayContent = CreateByteArrayContent(fileBytes, image);

                using var client = new HttpClient();
                var multipartContent = CreateMultipartFormDataContent(byteArrayContent, formFile);

                var request = CreateHttpRequestMessage(image, multipartContent);

                var responseMessage = await client.SendAsync(request);
                responseMessage.EnsureSuccessStatusCode();
                responseBody = await responseMessage.Content.ReadAsStringAsync();
            }

            return Result(responseBody);
            //return Ok(new {responseBody});
        }

        public ActionResult Result(string jsonBody)
        {
            var json = jsonBody;
            var objJson = JsonConvert.DeserializeObject<ResultModel>(json); //here we will map the Json to C# class
            //here we will return this model to view
            return View(objJson);  //you have to pass model to view
        }

        private static MultipartFormDataContent CreateMultipartFormDataContent(ByteArrayContent byteArrayContent,
            IFormFile formFile)
        {
            var multipartContent = new MultipartFormDataContent
            {
                {byteArrayContent, "ScanImage", formFile.FileName}
            };
            return multipartContent;
        }

        private static HttpRequestMessage CreateHttpRequestMessage(ImageModel image, MultipartFormDataContent multipartContent)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(image.ImageUrl),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), image.ContentTypeHeader},
                    {"Prediction-Key", image.PredictionKeyHeader}
                },

                Content = multipartContent
            };
            return request;
        }

        private static async Task<byte[]> CreateFileBytesArray(IFormFile formFile)
        {
            await using var ms = new MemoryStream();
            await formFile.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            return fileBytes;
        }

        private ImageModel CreateImageModel(IFormFile formFile)
        {
            var image = new ImageModel(_configuration["Api:PredictionKey"])
            {
                ImageUrl = _configuration["Api:ImageUrl"],
                ContentTypeHeader = "application/octet-stream",
                Body = formFile
            };
            return image;
        }

        private static ByteArrayContent CreateByteArrayContent(byte[] fileBytes, ImageModel image)
        {
            var byteArrayContent = new ByteArrayContent(fileBytes);
            byteArrayContent.Headers.Add("Content-Type", image.ContentTypeHeader);
            byteArrayContent.Headers.Add("Prediction-Key", image.PredictionKeyHeader);
            return byteArrayContent;
        }
    }
}