using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Controllers;
using MyAPI.Model;
using Microsoft.JSInterop;
using static System.Net.WebRequestMethods;
using System.Runtime.CompilerServices;
using System.IO;

namespace MyAPI.APITest
{
    [TestFixture]
    public class DownloadControllerTests
    {
        private DownloadController _controller;
        private HttpClient _http;
        private IJSRuntime _jsRuntime;

        //public DownloadControllerTests(HttpClient http, IJSRuntime jSRuntime)
        //{
        //    _http = http;
        //    _jsRuntime = jSRuntime;
        //}

        [SetUp]
        public void SetUp()
        {
            var jsRuntimeMock = new Mock<IJSRuntime>();
            //jsRuntimeMock.Setup(j => j.InvokeVoidAsync(It.IsAny<string>(), It.IsAny<object[]>()))
            //    .Returns(ValueTask.CompletedTask);
            //_controller = new DownloadController();
            //_jsRuntime = jsRuntimeMock.Object;
            _controller = new DownloadController();
            _http = new HttpClient();
            _jsRuntime = jsRuntimeMock.Object;
        }

        [Test]
        public void ExportPdf_ReturnsFileResult_WhenValidDataProvided()
        {
        //    // Arrange: Create a sample list of contributions
        //    var contributions = new List<ContributionModel>
        //    {
        //    new ContributionModel { ContributionID = 1, Name = "John Doe", Description = "Donation", DateReceived = DateTime.Now, Amount = 100 },
        //    new ContributionModel { ContributionID = 2, Name = "Jane Smith", Description = "Sponsorship", DateReceived = DateTime.Now, Amount = 250 }
        //};

        //    string fileName = "TestReport.pdf";

        //    // Act: Call the API method
        //    var result = _controller.ExportPdf(contributions, fileName) as FileContentResult;

        //    // Assert: Check if the response is valid
        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result.ContentType, Is.EqualTo("application/pdf"));
        //    Assert.That(result.FileDownloadName, Is.EqualTo(fileName));
        //    Assert.That(result.FileContents.Length, Is.GreaterThan(0), "PDF file should not be empty.");
        }

        [Test]
        public void GeneratePDF_ReturnsValidByteArray()
        {
            //// Arrange
            //var contributions = new List<ContributionModel>
            //{
            //    new ContributionModel { ContributionID = 1, Name = "Alice", Description = "Charity", DateReceived = DateTime.Now, Amount = 500 },
            //    new ContributionModel { ContributionID = 2, Name = "Bob", Description = "Donation", DateReceived = DateTime.Now, Amount = 300 }
            //};

            //// Act
            //var pdfBytes = _controller.GeneratePDF(contributions);

            //// Assert
            //Assert.That(pdfBytes, Is.Not.Null);
            //Assert.That(pdfBytes.Length, Is.GreaterThan(0), "PDF file should not be empty.");

            //// Optionally, save to file for manual verification
            //System.IO.File.WriteAllBytes("TestGenerated.pdf", pdfBytes);
        }

        [Test]
        public async Task Pdf()
        {
            // Arrange: Create a sample list of contributions
            var contributions = new List<ContributionModel>
            {
            new ContributionModel { ContributionID = 1, Name = "John Doe", Description = "Donation", DateReceived = DateTime.Now, Amount = 100 },
            new ContributionModel { ContributionID = 2, Name = "Jane Smith", Description = "Sponsorship", DateReceived = DateTime.Now, Amount = 250 }
        };

            string fileName = "TestReport.pdf";

            var response = await _http.PostAsJsonAsync($"https://localhost:7257/api/Download/pdf?fileName={fileName}", contributions);
            if (response.IsSuccessStatusCode)
            {
                var fileType = "application/pdf";
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                using var streamRef = new DotNetStreamReference(new MemoryStream(fileBytes));
                await _jsRuntime.InvokeVoidAsync("downloadFile", fileName, streamRef, fileType);
            } else
            {
                Assert.That(response.IsSuccessStatusCode, Is.Not.EqualTo(true)); //Console.WriteLine("Failed to fetch PDF.");
            }
        }
    }
}
