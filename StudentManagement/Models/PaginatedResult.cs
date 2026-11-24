namespace StudentManagement.Models
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalStudents { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public PaginatedResult(List<T> items, int totalStudents, int totalPages, int currentPage, int pageSize)
        {
            this.Items = items;
            this.TotalStudents = totalStudents;
            this.TotalPages = totalPages;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
        }

    }
}
