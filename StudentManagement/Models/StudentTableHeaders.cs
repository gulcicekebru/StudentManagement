using System.Drawing;


namespace StudentManagement.Models
{
    public class StudentTableHeaders
    {
        public string HeaderName { get; set; }

        public int ButtonBehavior {  get; set; }

        public string UniqueName {  get; set; }

        public StudentTableHeaders(string headerName, int buttonBehavior)
        {
            this.HeaderName = headerName;
            this.ButtonBehavior = buttonBehavior;
        }

        public StudentTableHeaders() { }


    }


}
