using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HCM.Application.Features.TimeSheet.TimeSheetFeature.Queries.GetTimeSheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HCM.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TimeSheetController : ApiControllerBase
    {
        [HttpPost("CalculateTimesheet")]
        public async Task<ActionResult<List<GetTimeSheetQueryResult>>> CalculateTimesheet(IFormFile file, CancellationToken cancellationToken)
        {
            await using Stream stream = file.OpenReadStream();
            GetTimeSheetQueryResult outPut = await Mediator.Send(new GetTimeSheetQuery { InputFile = stream }, cancellationToken);
            return File(outPut.CsvOutput, "text/csv", $"{file.Name} Report", true);
        }
    }
}
