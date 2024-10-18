using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using WorkBuddy.Api.Models;

namespace WorkBuddy.Api.Entities
{
    
    public class LeaveRequest
    {
        /*
        public enum RequestType
        {
            SickLeave,
            StudentLeave,
            Vacation,
            UnpaidLeave
        }

        public enum RequestStatus
        {
            Pending,
            Approved,
            Rejected
        }
        */

        public int Id { get; set; }
        public int AppUserId {  get; set; }

        public RequestType Type { get; set; }

        public RequestStatus Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string Description { get; set; } = "";

    }
}
