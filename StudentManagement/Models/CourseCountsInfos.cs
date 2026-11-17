namespace StudentManagement.Models
{
    public class CourseCountsInfos
    {
        public string CountHeaderName { get; set; } = string.Empty;
        public int HeaderCount { get; set; }

        public CourseCountsInfos(string countOf, int counts)
        {
            this.CountHeaderName = countOf;
            this.HeaderCount = counts;
        }
        public CourseCountsInfos() { }
    }
}
