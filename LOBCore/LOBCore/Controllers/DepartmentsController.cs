using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOBCore.DataAccesses;
using LOBCore.DataAccesses.Entities;
using Microsoft.AspNetCore.Authorization;
using LOBCore.DataTransferObject.DTOs;
using LOBCore.Helpers;
using LOBCore.BusinessObjects.Factories;

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;

        public DepartmentsController(LOBDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetDepartments()
        {
            List<DepartmentResponseDTO> DepartmentResponseDTO = new List<DepartmentResponseDTO>();
            foreach (var item in _context.Departments)
            {
                DepartmentResponseDTO fooObject = new DepartmentResponseDTO()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                DepartmentResponseDTO.Add(fooObject);
            }
            APIResult apiResult = APIResultFactory.Build(true, StatusCodes.Status200OK,
                ErrorMessageEnum.None, payload: DepartmentResponseDTO);
            return Ok(apiResult);
        }

    }
}