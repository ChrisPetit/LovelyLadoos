# LovelyLadoos
Season of Serverless challenge 2

First I've uploaded a bunch of Ladoos images, pie images, bitterbal images and Donut images to https://www.customvision.ai/ and a Lovely Ladoos project I've created there [using this guide](https://docs.microsoft.com/en-us/azure/cognitive-services/custom-vision-service/getting-started-build-a-classifier). There is always the option to [create and use your own models](https://docs.microsoft.com/en-us/azure/cognitive-services/custom-vision-service/quickstarts/image-classification?tabs=visual-studio&pivots=programming-language-csharp), but since this is serverless the most easy option is the best I Think.

Then I've Published a MVC webapp to Azure...I'm sure there was an easier way, but I've challenged myself to solve al challenges using C#.

So basicly the logic for uploading a File or sending an Url to the API of CustomVision is in the FileUploadController.cs I know the name could be improved upon. 

Through the index page we get variables, ```files``` and ```url```, we check for nuls, create a ```httpRequestMessage```, perform the request using a ```HttpClient``` and display the response through a razor page...

I've tried to keep the naming of the methods in the file pretty clear to make it understandable.

To build this WebApp open the Solution *.sln in Visual Studio 2019. Keep in mind you will need to set secrets ass follows:

right click on the LovelyLadoos.WebApp and choose: Manage User Secrets.
use the following JSON:
```
{
  "Api": {
    "UrlUrl": <Your Prediction API Image URL>,
    "ImageUrl":<Your Prediction API Image File>,
    "PredictionKey": <Your PredictionKey>
  } 
}
```

All in all this was a nice little project.

The Result of my WebApp is published in Azure WebApps and running serverless there:
https://lovelyladooswebapp20201203194115.azurewebsites.net/

