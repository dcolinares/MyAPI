using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using MyAPI.Model;
using DocumentFormat.OpenXml.Drawing;
using SixLabors.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;

namespace MyAPI.Controllers
{
    
    [ApiController]
    [Route("api/[Controller]")]
    public class DownloadController : Controller
    {
        [HttpPost("excel")]
        public IActionResult ExportExcel([FromBody]List<ContributionModel> contributions, [FromQuery]string fileName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Contributions");
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Description";
            worksheet.Cell(1, 4).Value = "Date";
            worksheet.Cell(1, 5).Value = "Amount";

            for (int i = 0; i < contributions.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = contributions[i].ContributionID;
                worksheet.Cell(i + 2, 2).Value = contributions[i].Name;
                worksheet.Cell(i + 2, 3).Value = contributions[i].Description;
                worksheet.Cell(i + 2, 4).Value = contributions[i].DateReceived;
                worksheet.Cell(i + 2, 5).Value = contributions[i].Amount;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpPost("pdf")]
        public IActionResult DownloadPDF([FromBody]List<ContributionModel> contributions , [FromQuery]string fileName)
        {
            string content = "Replace with your data";

            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12, XFontStyle.Regular);

            double startX = 40;  // Left margin
            double startY = 50;  // Top margin
            double rowHeight = 20; // Row height
            double colWidth = 100; // Column width

            // Draw table header
            gfx.DrawString("ContributionID", font, XBrushes.Black, new XPoint(startX, startY));
            gfx.DrawString("Name", font, XBrushes.Black, new XPoint(startX + colWidth, startY));
            gfx.DrawString("Description", font, XBrushes.Black, new XPoint(startX + 2 * colWidth, startY));
            gfx.DrawString("DateReceived", font, XBrushes.Black, new XPoint(startX + 3 * colWidth, startY));
            gfx.DrawString("Amount", font, XBrushes.Black, new XPoint(startX + 4 * colWidth, startY));
            gfx.DrawString(content, font, XBrushes.Black, new XPoint(40, 40));
            // Draw a line under the header
            gfx.DrawLine(XPens.Black, startX, startY + 5, startX + 3 * colWidth, startY + 5);

            startY += rowHeight; // Move to the next row

            // Loop through user list and add data to table
            foreach (var c in contributions)
            {
                gfx.DrawString(c.ContributionID.ToString(), font, XBrushes.Black, new XPoint(startX, startY));
                gfx.DrawString(c.Name, font, XBrushes.Black, new XPoint(startX + colWidth, startY));
                gfx.DrawString(c.Description, font, XBrushes.Black, new XPoint(startX + 2 * colWidth, startY));
                gfx.DrawString(c.DateReceived.ToShortDateString(), font, XBrushes.Black, new XPoint(startX + 3 * colWidth, startY));
                gfx.DrawString(c.Amount.ToString("C2"), font, XBrushes.Black, new XPoint(startX + 4 * colWidth, startY));

                startY += rowHeight;

                // Page Break (if needed)
                if (startY > page.Height - 50)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    startY = 50; // Reset start position
                }
            }

            var stream = new MemoryStream();
                document.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);// Ensure the stream starts from the beginning
            return File(stream, "application/pdf", fileName);
        }
    }
}
