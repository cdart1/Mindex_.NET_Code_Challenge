using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpPost("createcompensation")]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.Employee}'");

            _employeeService.CreateSalary(compensation);

            return CreatedAtRoute("getCompensationById", compensation);
        }

        // This endpoint retrieves compensation by employeeId
        //[HttpGet("{id}/getcompensation", Name = "getCompensationById")]
        //public IActionResult ReadCompensation(String id)
        //{
        //    _logger.LogDebug($"Received compensation get request for '{id}'");

        //    var compensationById = _employeeService.GetById(id);

        //    if (compensationById == null)
        //        return NotFound();
        //    // The built-in helper method Ok returns JSON-formatted data
        //    return Ok(compensationById);
        //}

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // This method retrieves the ReportingStructure for the specified employeeId.
        [HttpGet("{id}/getreport", Name = "getReportingStructure")]
        public IActionResult GetReportingStructure(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var reportingStructure = _employeeService.GetReporting(id);

            if (reportingStructure == null)
                return NotFound();

            return Ok(reportingStructure);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }
    }
}
