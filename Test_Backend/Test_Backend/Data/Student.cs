using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test_Backend.Data
{
    [Table("Students")] 
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }         

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }    

        [Required]
        [MaxLength(50)]
        public string Lop { get; set; }     

        [Range(0, 10)]
        public double Diem { get; set; }     
    }
}
