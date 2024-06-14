using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using HCM.Domain.Entities.TimeSheet;

namespace HCM.Application.Common.Extensions;

public static class AttendanceExtensions
{
    public static List<AttendanceRecord> GetAttendanceRecords(this Stream stream)
    {
        List<AttendanceRecord> records = new List<AttendanceRecord>();

        using StreamReader reader = new StreamReader(stream);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split('\t');
            if (parts.Length >= 3)
            {
                AttendanceRecord record = new AttendanceRecord
                {
                    EmployeeId = parts[0],
                    EmployeeName = parts[1],
                    RecordDateTime = DateTime.Parse(parts[2], null, DateTimeStyles.RoundtripKind)
                };
                records.Add(record);
            }
        }

        return records;
    }
}