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
using LOBCore.DTOs;

namespace LOBCore.Controllers
{
    [Authorize(Roles = "User")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly LOBDatabaseContext _context;
        private readonly APIResult apiResult;

        public DepartmentsController(LOBDatabaseContext context, APIResult apiResult)
        {
            _context = context;
            this.apiResult = apiResult;
        }

        [HttpGet]
        public APIResult GetDepartments()
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
            apiResult.Payload = DepartmentResponseDTO;
            return apiResult;
        }

    }
}