namespace StudentManagement.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name{ get; set; }
        public string LastName { get; set; }
        public int Midterm { get; set; }
        public int Final { get; set; }

        public Student(int id, string name, string lastName, int midterm, int final)
        {
            this.StudentId = id;
            this.Name = name;
            this.LastName = lastName;
            this.Midterm = midterm;
            this.Final = final;
        }

        public Student()
        {

        }
    }


}
