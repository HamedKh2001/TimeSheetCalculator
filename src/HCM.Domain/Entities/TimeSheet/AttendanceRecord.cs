using System;

namespace HCM.Domain.Entities.TimeSheet;

public class AttendanceRecord
{
    public string EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public DateTime RecordDateTime { get; set; }
}