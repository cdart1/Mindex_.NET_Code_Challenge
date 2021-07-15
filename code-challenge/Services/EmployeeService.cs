using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;
using challenge.Enum;

namespace challenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Compensation CreateSalary(Compensation compensation)
        {
            if (compensation != null)
            {
                _employeeRepository.AddSalary(compensation);
                _employeeRepository.SaveAsync().Wait();
            }
            return compensation;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }
            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        // This method returns a compensation object for the employee by which 
        // employeeId is provided.
        //public Compensation GetCompById(String id)
        //{
            //var compensationById = GetById(id);
            //var salary = GetSalary(employee);
            //Compensation compensation = new Compensation();
            //compensation.Employee = employee;
            //compensation.Salary = (int)salary;
            //compensation.EffectiveDate = DateTime.Now;
            //return compensationById;
        //}

        // This method returns the numberOfReports for an employee and all of their direct reports.
        public ReportingStructure GetReporting(string id)
        {
            int count = 0;
            var employee = GetById(id);
            ReportingStructure reportingStructure = new ReportingStructure();
            reportingStructure.Employee = employee;
            RecursiveReports(employee, ref count);
            reportingStructure.NumberOfReports = count;
            return reportingStructure;
        }

        // This method iterates through the currentEmployee, adding one to the count each time
        // the currentEmployee has DirectReports.
        private void RecursiveReports(Employee employee, ref int count)
        {
            if (employee.DirectReports == null)
            {
                return;
            }
            if (employee.DirectReports != null)
            {
                foreach (var currentEmployee in employee.DirectReports)
                {
                    count++;
                    RecursiveReports(currentEmployee, ref count);
                }
            }
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }
    }
}
