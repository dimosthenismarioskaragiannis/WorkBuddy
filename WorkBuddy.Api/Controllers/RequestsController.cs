using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using WorkBuddy.Api.Controllers;
using WorkBuddy.Api.Data;
using WorkBuddy.Api.Entities;
using WorkBuddy.Api.Models;
using WorkBuddy.Api.DTOs;
// Ensure only authenticated users can access this endpoint
public class RequestsController(DataContext context) : BaseApiController
{




    // I use the username from JWtoken instead of passing it down through the get/post request ,for "security"
    // I know this approach isnt the most efficient performance wise (compared to passing userid as i have to parse the users table )
    // to get the Id of the user and then parse the Requests table. (I know how to make multiline comments but i hate them ^_^ )
    private string GetUsernameFromToken()
    {
        // Extract the username from the token via HttpContext.User
        return HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    [Authorize]
    // GET: api/requests
    [HttpGet]
    public async Task<IActionResult> GetRequestsByUser()
    {
        // Extract the username from the JWT token
        var username = GetUsernameFromToken();

        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized("Invalid token or user not authenticated.");
        }

        // Fetch the user by username
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);

        if (user == null)
        {
            return NotFound($"User with username {username} not found.");
        }

        // Fetch the requests for the user
        var requests = await context.Requests
            .Where(r => r.AppUserId == user.Id)
            .ToListAsync();

        //System.Diagnostics.Debug.WriteLine(requests[0].Type);


        if (requests == null || !requests.Any())
        {
            return NotFound("No requests found for the user.");
        }

        // Return the requests
        return Ok(requests);
    }


    //Get A user Request

    
    [HttpPost("newrequest")]
    [Authorize]
    public async Task<IActionResult> SubmitLeaveRequest(RequestDto requestDto)
    {
        
        //System.Diagnostics.Debug.WriteLine(requestDto.Description);
        var username = GetUsernameFromToken();
       

        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized("Invalid token or user not authenticated.");
        }

        var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);

        if (user == null)
        {
            return NotFound($"User with username {username} not found.");
        }

        if (requestDto.StartDate > requestDto.EndDate)
        {
            return BadRequest("Start date must be before the end date.");
        }

        //int leaveDuration = (requestDto.EndDate - requestDto.StartDate).Days + 1;
        //Computes leave duration. Because leave days doesnt include weekends.
        //TODO: I will come back to make it more efficient.


        DateTime dt = requestDto.StartDate;
        int leaveDuration = 0;
        while (dt <= requestDto.EndDate)
        {
            if(dt.DayOfWeek!=DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
            {
                leaveDuration++;
            }
            dt = dt.AddDays(1);
            
        }



        System.Diagnostics.Debug.WriteLine(leaveDuration);

        RequestStatus status=RequestStatus.Pending;
        //Auto reject if Leave Duration exceeds Remaining Available leave
        if (requestDto.Type == RequestType.Vacation)
        {
            if (leaveDuration > user.RemainingVaccationDays) { 
                status = RequestStatus.Rejected;
            }
            else
            {
                user.RemainingVaccationDays -= leaveDuration;
            }
             

        }else if(requestDto.Type == RequestType.StudentLeave)
        {
            if (leaveDuration > user.RemainingStudentLeave) { status = RequestStatus.Rejected; }
            else { user.RemainingStudentLeave -= leaveDuration; }

        }
        else
        {

        }


        // Create the LeaveRequest entity
        var leaveRequest = new LeaveRequest
        {
            AppUserId = user.Id,
            Type = requestDto.Type,  // Enum stored as integer
            StartDate = requestDto.StartDate,
            Description = requestDto.Description,
            EndDate = requestDto.EndDate,
            Status = status

        };
       
        context.Requests.Add(leaveRequest);

        
        
        await context.SaveChangesAsync();

        return Ok();


    }


}