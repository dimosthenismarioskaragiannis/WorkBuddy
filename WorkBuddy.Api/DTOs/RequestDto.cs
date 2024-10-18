using System.ComponentModel.DataAnnotations;
using WorkBuddy.Api.Models;

namespace WorkBuddy.Api.DTOs
{
    public class RequestDto
    {
        public RequestType Type { get; set; }

        public string Description { get; set; } = "";

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }




    }


}
