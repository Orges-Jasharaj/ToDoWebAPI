using System.ComponentModel.DataAnnotations;

namespace ToDoWebAPI.Dtos.Responses
{
    

    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
    }
    

    
}
