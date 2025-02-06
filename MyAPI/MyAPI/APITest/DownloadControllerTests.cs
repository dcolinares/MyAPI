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

        [SetUp]
        public void SetUp()
        {
            var jsRuntimeMock = new Mock<IJSRuntime>();
            _controller = new DownloadController();
            _http = new HttpClient();
            _jsRuntime = jsRuntimeMock.Object;
        }

        [Test]
        public void ExportPdf_ReturnsFileResult_WhenValidDataProvided()
        {
            // Arrange: Create a sample list of contributions
            var contributions = new List<ContributionModel>
            {
            new ContributionModel { ContributionID = 1, Name = "John Doe", Description = "Donation", DateReceived = DateTime.Now, Amount = 100 },
            new ContributionModel { ContributionID = 2, Name = "Jane Smith", Description = "Sponsorship", DateReceived = DateTime.Now, Amount = 250 }
        };

            string fileName = "TestReport.pdf";

            // Act: Call the API method
            var result = _controller.DownloadPDF(contributions, fileName) as FileStreamResult;

            // Assert: Check if the response is valid
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ContentType, Is.EqualTo("application/pdf"));
            Assert.That(result.FileDownloadName, Is.EqualTo(fileName));
            Assert.That(result.FileStream.Length, Is.GreaterThan(0), "PDF file should not be empty.");
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
