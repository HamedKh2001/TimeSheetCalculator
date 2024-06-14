using System.IO;

namespace HCM.Application.Features.TimeSheet.TimeSheetFeature.Queries.GetTimeSheet;

public class GetTimeSheetQueryResult
{
    public MemoryStream CsvOutput { get; init; }
}