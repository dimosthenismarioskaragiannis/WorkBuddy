using System.ComponentModel.DataAnnotations;

namespace WorkBuddy.Api.DTOs
{
    public class RegisterDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Mail { get; set; }

        //Fk to Department
        public int DepartmentId { get; set; }

        [DataType(DataType.Date)]
        public required DateTime DateOfBirth { get; set; }

        public required string Gender { get; set; }

    }
}
