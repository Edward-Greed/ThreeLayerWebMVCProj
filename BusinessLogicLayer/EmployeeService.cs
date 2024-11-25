using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private List<Employee> _employees = new List<Employee>();

        public double getTotalSalary() 
        {
            return _employees.Sum(e => e.Salary);
        }
        public int getCountOfPosition(string position) 
        { 
            return _employees.Count(e => e.Position == position);
        }
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }

        //Implement the method from the interface, and here I can add logic to whichever methods I want 
        //to add logic to
        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
           
            return await _employeeRepository.GetEmployeesAsync();
        }

        //This employee being passed in is coming from my controller
        //...and I am writing logic on this employee before passing it to my DAL
        //..where the changes are being executed on my DbSet which goes to the Database
        public async Task AddEmployeeAsync(Employee employee)
        {
            //Writing logic
            var ttlSal = (await _employeeRepository.GetEmployeesAsync()).Sum(e => e.Salary);
            var postions = await _employeeRepository.GetEmployeesAsync();
      
            if (ttlSal + employee.Salary  > 1000000)
            {
                throw new InvalidOperationException("Total salary is too high");

            }

            if (employee.Salary > 100000)
            {
                throw new InvalidOperationException("Employees salary is too high");

            }

            if (employee.Position == "Engineer" && postions.Count(e=> e.Position == "Engineer") > 5)
            {
                throw new InvalidOperationException("Engineer positions are full");

            }

            if (employee.Position == "Manager" && postions.Count(e => e.Position == "Manager") > 5)
            {
                throw new InvalidOperationException("Manager positions are full");
            }

            if (employee.Position == "HR" && postions.Count(e => e.Position == "HR") > 1)
            {
                throw new InvalidOperationException("HR positions are full");
            }

            await _employeeRepository.AddEmployeeAsync(employee);
        }

        //This employee being passed in is coming from my controller
        //...and I am writing logic on this employee before passing it to my DAL
        //..where the changes are being executed on my DbSet which goes to the Database
        //going to the DAL, and getting the employee from the employees DbSet
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }

        //This employee being passed in is coming from my controller
        //...and I am writing logic on this employee before passing it to my DAL
        //..where the changes are being executed on my DbSet which goes to the Database
        public async Task GetDetailsAsync(Employee employee)
        {
            await _employeeRepository.GetDetailsAsync(employee);
        }

        //This employee being passed in is coming from my controller
        //...and I am writing logic on this employee before passing it to my DAL
        //..where the changes are being executed on my DbSet which goes to the Database
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var existingEmp = await _employeeRepository.GetEmployeeByIdAsync(employee.Id);
            var ttlSal = (await _employeeRepository.GetEmployeesAsync()).Sum(e => e.Salary);
            var postions = await _employeeRepository.GetEmployeesAsync();


            if (existingEmp != null) { 
            
                double newTtlSal = ttlSal - existingEmp.Salary + employee.Salary;

                if (newTtlSal > 1000000)
                {
                    throw new InvalidOperationException("Total salary is too high");
                }

                if (employee.Salary > 100000)
                {
                    throw new InvalidOperationException("Employees salary is too high");

                }

                if (employee.Position == "Engineer" && postions.Count(e => e.Position == "Engineer") > 5)
                {
                    throw new InvalidOperationException("Engineer positions are full");

                }

                if (employee.Position == "Manager" && postions.Count(e => e.Position == "Manager") > 5)
                {
                    throw new InvalidOperationException("Manager positions are full");
                }

                if (employee.Position == "HR" && postions.Count(e => e.Position == "HR") > 1)
                {
                    throw new InvalidOperationException("HR positions are full");
                }

                await _employeeRepository.UpdateEmployeeAsync(employee);

            }
        }

    }
}
