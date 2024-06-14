using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HCM.Application.Common.Extensions;
using HCM.Domain.Entities.TimeSheet;
using MediatR;
using SharedKernel.Extensions;

namespace HCM.Application.Features.TimeSheet.TimeSheetFeature.Queries.GetTimeSheet;

public class GetTimeSheetQueryHandler : IRequestHandler<GetTimeSheetQuery, GetTimeSheetQueryResult>
{
    public Task<GetTimeSheetQueryResult> Handle(GetTimeSheetQuery request, CancellationToken cancellationToken)
    {
        List<AttendanceRecord> attendanceRecords = request.InputFile.GetAttendanceRecords();
        var processedRecords = GroupingAttendanceRecords(attendanceRecords);
        string csvOutput = GenerateCsvOutput(processedRecords);

        return Task.FromResult(new GetTimeSheetQueryResult
        {
            CsvOutput = csvOutput.GenerateCsvMemoryStream()
        });
    }

    #region Helper

    private static Dictionary<string, Dictionary<DateTime, List<AttendanceRecord>>> GroupingAttendanceRecords(
        List<AttendanceRecord> records)
    {
        var groupedRecords = new Dictionary<string, Dictionary<DateTime, List<AttendanceRecord>>>();

        foreach (var record in records)
        {
            if (!groupedRecords.ContainsKey(record.EmployeeId))
                groupedRecords[record.EmployeeId] = new Dictionary<DateTime, List<AttendanceRecord>>();

            Dictionary<DateTime, List<AttendanceRecord>> employeeRecords = groupedRecords[record.EmployeeId];
            DateTime recordDate = record.RecordDateTime.Date;

            if (!employeeRecords.ContainsKey(recordDate))
                employeeRecords[recordDate] = new List<AttendanceRecord>();

            employeeRecords[recordDate].Add(record);
        }

        return groupedRecords;
    }

    private string GenerateCsvOutput(Dictionary<string, Dictionary<DateTime, List<AttendanceRecord>>> records)
    {
        StringBuilder csvContent = new StringBuilder();
        csvContent.AppendLine("ردیف,تاریخ روز,نام فرد,نوع کارکرد,اولین ورود,آخرین خروج,رکوردها");

        int rowIndex = 1;

        foreach (var employeeRecords in records)
        {
            string employeeId = employeeRecords.Key;

            foreach (var dateRecords in employeeRecords.Value)
            {
                DateTime date = dateRecords.Key;
                var dailyRecords = dateRecords.Value;

                if (dailyRecords.Count % 2 != 0) // چک خطا برای رکورد های فرد
                {
                    AppendCsvRow(csvContent, rowIndex++, date, dailyRecords.First().EmployeeName, "خطا", null, null,
                        dailyRecords);
                    continue;
                }

                var firstEntry = dailyRecords.First().RecordDateTime;
                var lastExit = dailyRecords.Last().RecordDateTime;
                TimeSpan totalWorkTime = lastExit - firstEntry;
                string workType = GetWorkType(firstEntry, lastExit, totalWorkTime, dailyRecords);

                AppendCsvRow(csvContent, rowIndex++, date, dailyRecords.First().EmployeeName, workType, firstEntry,
                    lastExit, dailyRecords);
            }
        }

        return csvContent.ToString();
    }

    private string GetWorkType(DateTime firstEntry, DateTime lastExit, TimeSpan totalWorkTime,
        List<AttendanceRecord> dailyRecords)
    {
        TimeSpan minimumWorkDay = TimeSpan.FromHours(8.5);
        TimeSpan startOfWorkTime = new TimeSpan(8, 30, 0);
        TimeSpan endOfWorkTime = new TimeSpan(17, 15, 0);
        TimeSpan floatTime = TimeSpan.FromMinutes(15);
        double earlyEnterMinutes = startOfWorkTime.Add(floatTime).TotalMinutes - firstEntry.ToTotalMinutes();

        //محاسبه دقایق تعجیل در ورود جهت کسر از ماکزیمم زمان خروج
        double validEarlyEnterMinutes = earlyEnterMinutes;
        if (earlyEnterMinutes > floatTime.TotalMinutes)//محدود کردت به 15 دقیقه
            validEarlyEnterMinutes = floatTime.TotalMinutes;

        bool isLateEntry = firstEntry.TimeOfDay > startOfWorkTime.Add(floatTime);
        bool isEarlyExit = lastExit.TimeOfDay < endOfWorkTime.Add(-TimeSpan.FromMinutes(validEarlyEnterMinutes));
        bool isIncompleteDay = totalWorkTime < minimumWorkDay;
        bool isHourlyLeave = isIncompleteDay && dailyRecords.Count > 2 &&
                             firstEntry.TimeOfDay <= startOfWorkTime.Add(floatTime);

        if (dailyRecords.Count % 2 != 0) return "خطا";
        if (dailyRecords.Count == 0) return "مرخصی روزانه";
        if (isLateEntry || isEarlyExit) return "تاخیر";
        if (isHourlyLeave) return "مرخصی ساعتی";

        return "عادی";
    }

    private void AppendCsvRow(StringBuilder csvContent, int rowIndex, DateTime date, string employeeName,
        string workType, DateTime? firstEntry, DateTime? lastExit, List<AttendanceRecord> dailyRecords)
    {
        string formattedDate = date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
        string dayOfWeek = GetPersianDayOfWeek(date);
        string firstEntryStr = firstEntry?.ToString("HH:mm") ?? "-";
        string lastExitStr = lastExit?.ToString("HH:mm") ?? "-";
        string recordsStr = string.Join(" ", dailyRecords.Select(r => r.RecordDateTime.ToString("HH:mm")));

        csvContent.AppendLine(
            $"{rowIndex},{formattedDate} {dayOfWeek},{employeeName},{workType},{firstEntryStr},{lastExitStr},{recordsStr}");
    }

    private string GetPersianDayOfWeek(DateTime date)
    {
        PersianCalendar persianCalendar = new PersianCalendar();
        DayOfWeek dayOfWeek = persianCalendar.GetDayOfWeek(date);
        string[] persianDayNames = { "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه" };

        return persianDayNames[(int)dayOfWeek];
    }

    #endregion
}