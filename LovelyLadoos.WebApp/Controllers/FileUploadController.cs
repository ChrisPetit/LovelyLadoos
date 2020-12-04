using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
        public async Task<IActionResult> Result(List<IFormFile> files, string url)
        {
            // 
            if (string.IsNullOrEmpty(url) && files.Count.Equals(0)) return NoContent();
            var responseBody = "";

            foreach (var formFile in files.Where(formFile => formFile.Length > 0))
            {
                // Create an Array to send to the Api
                var fileBytes = await CreateFileBytesArray(formFile);

                // process uploaded file
                var imageModel = CreateImageModel(formFile);
                var byteArrayContent = CreateByteArrayContent(fileBytes, imageModel);
                using var client = new HttpClient();
                var multipartContent = CreateMultipartFormDataContent(byteArrayContent, formFile);
                var request = CreateHttpRequestMessage(imageModel, multipartContent);
                responseBody = await CreateResponseBody(client, request);
            }
            // Process an Url if that is filled in.
            if (string.IsNullOrEmpty(url)) return Result(responseBody);
            {
                var urlModel = CreateUrlModel(url);
                using var client = new HttpClient();
                var request = CreateHttpRequestMessage(urlModel);
                responseBody = await CreateResponseBody(client, request);
            }

            return Result(responseBody);
        }

        private static async Task<string> CreateResponseBody(HttpClient client, HttpRequestMessage request)
        {
            var responseMessage = await client.SendAsync(request);
            responseMessage.EnsureSuccessStatusCode();
            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            return responseBody;
        }

        public ActionResult Result(string jsonBody)
        {
            var json = jsonBody;
            var objJson = JsonConvert.DeserializeObject<ResultModel>(json); //here we will map the Json to C# class
            //here we will return this model to view
            return View(objJson);  //you have to pass model to view
        }

        private static MultipartFormDataContent CreateMultipartFormDataContent(HttpContent byteArrayContent,
            IFormFile formFile)
        {
            var multipartContent = new MultipartFormDataContent
            {
                {byteArrayContent, "ScanImage", formFile.FileName}
            };
            return multipartContent;
        }

        private static HttpRequestMessage CreateHttpRequestMessage(ImageModel imageModel, HttpContent multipartContent)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(imageModel.ImageUrl),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), imageModel.ContentTypeHeader},
                    {"Prediction-Key", imageModel.PredictionKeyHeader}
                },
                Content = multipartContent
            };
            return request;
        }

        private static HttpRequestMessage CreateHttpRequestMessage(UrlModel urlModel)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(urlModel.ImageUrl),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.ContentType.ToString(), urlModel.ContentTypeHeader},
                    {"Prediction-Key", urlModel.PredictionKeyHeader}
                },
                Content = new StringContent($"{{\"Url\" : \"{urlModel.Body}\"}}", Encoding.UTF8, "application/json")
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
            var imageModel = new ImageModel(_configuration["Api:PredictionKey"])
            {
                ImageUrl = _configuration["Api:ImageUrl"],
                Body = formFile
            };
            return imageModel;
        }

        private UrlModel CreateUrlModel(string url)
        {
            var urlModel = new UrlModel(_configuration["Api:PredictionKey"])
            {
                ImageUrl = _configuration["Api:UrlUrl"],
                Body = url
            };
            return urlModel;
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