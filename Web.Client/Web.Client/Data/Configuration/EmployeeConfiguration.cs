using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Client.Data.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasData(
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Gabriel Figueroa",
                    Position = "Software Engineer",
                    Age = 35
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Becky Whisler",
                    Position = "Human Resoures Technician",
                    Age = 38
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Valerie Irene",
                    Position = "Software Testing Engineer",
                    Age = 28
                }
            );
        }
    }
}
