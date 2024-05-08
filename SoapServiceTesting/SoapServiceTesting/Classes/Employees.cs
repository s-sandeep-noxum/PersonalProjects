using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoapServiceTesting.Classes
{
    public class Employees : List<Employee>
    {
        private List<Employee> _employees;

        public Employees()
        {
            if (_employees == null) { _employees = new List<Employee>(); }

            _employees.Add(new Employee { Aadhar = "123", FirstName = "Sandeep", LastName = "Shenoy" });
            _employees.Add(new Employee { Aadhar = "456", FirstName = "Ajith", LastName = "Singh" });
            _employees.Add(new Employee { Aadhar = "789", FirstName = "Abhinav", LastName = "V" });
        }

        public bool AddEmployee(Employee employee)
        {
            EmployeesList.Add(employee);
            return true;
        }

        public List<Employee> EmployeesList => _employees;

    }
}