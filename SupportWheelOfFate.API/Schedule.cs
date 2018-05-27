using System;

namespace SupportWheelOfFate.API
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Employee { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}