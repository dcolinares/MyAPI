using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using MyAPI.Model;

namespace MyAPI.Controllers
{
    
    [ApiController]
    [Route("api/[Controller]")]
    public class ExcelController : Controller
    {
        [HttpPost("export")]
        public IActionResult ExportExcel(List<ContributionModel> contributions)
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

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UsersReport.xlsx");
        }
    }
}
