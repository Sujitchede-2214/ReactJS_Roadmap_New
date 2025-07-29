namespace ReactJS_RoadMap
{
    public class ProgramModel
    {
            public int DayNumber { get; set; }
            public string Title { get; set; }
            public bool Completed { get; set; }
            public DateTime EntryDate { get; set; }
            public DateTime? UpdateDate { get; set; }

            public int WeekNumber => (int)Math.Ceiling(DayNumber / 7.0);
       
    }
}
