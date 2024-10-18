using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkBuddy.Api.Data;
using WorkBuddy.Api.Entities;

namespace WorkBuddy.Api.Controllers
{
    public class DepartmentsController(DataContext context):BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments(){

            var departments = await context.Departments.ToListAsync();

            return departments;

        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Department>> GetDepartments(int id)
        {

            var department = await context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }


    }
}
