using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }

        [Required]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Name must be text.")]
        public string Name{ get; set; }
        [Required]
        [RegularExpression(@"^[^\d]+$", ErrorMessage = "Last name must be text")]
        public string LastName { get; set; }
        [Required]
        [Range(0,100)]
        public int? Midterm { get; set; }
        [Required]
        [Range(0, 100)]
        public int? Final { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public double? Average
        {
            get
            {
                if(Midterm!= null && Final != null) 
                { return Midterm * 0.4 + Final * 0.6; }
                return null;
            }
        }

        [NotMapped]
        public string Status => Average >= 50 ? "Passed" : "Not Passed";


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
