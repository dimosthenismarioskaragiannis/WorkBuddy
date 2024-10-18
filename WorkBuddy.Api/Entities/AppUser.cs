using System.ComponentModel.DataAnnotations;

namespace WorkBuddy.Api.Entities
{
    //Creates a User entity for the application users that is going to exist in our database
    //Ill use pwd hashing and salting for the storage of passwords in my database
    //Todo: Use asp.net Identity 
    public class AppUser
    {
        public int Id { get; set; }

        public required string UserName { get; set; }

        public required string FirstName {  get; set; }

        public required string LastName { get; set; }

        public required string Mail { get; set; }

        public required  byte[] PwdHash { get; set; }
        public required byte[] PwdSalt { get; set; }

        //Fk to Department
        public int DepartmentId { get; set; }


        [DataType(DataType.Date)]
        public required DateTime DateOfBirth { get; set; }

        public required string Gender {  get; set; }

        public int RemainingVaccationDays { get; set; } = 20;
        public int RemainingStudentLeave { get; set; } = 14;

        


    }






}
