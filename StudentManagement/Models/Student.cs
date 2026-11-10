namespace StudentManagement.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name{ get; set; }
        public string LastName { get; set; }
        public int Midterm { get; set; }
        public int Final { get; set; }

        public double Average
        {
            get
            {
                return Midterm * 0.4 + Final * 0.6;
            }
        }
        

        public Student(string name, string lastName, int midterm, int final)
        {        
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
