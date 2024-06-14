using System.IO;
using MediatR;

namespace HCM.Application.Features.TimeSheet.TimeSheetFeature.Queries.GetTimeSheet;

public class GetTimeSheetQuery : IRequest<GetTimeSheetQueryResult>
{
    public Stream InputFile { get; init; }
}