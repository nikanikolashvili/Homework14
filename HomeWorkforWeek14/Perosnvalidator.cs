using FluentValidation;
using HomeWorkforWeek14.Models;
using System.Data;

namespace HomeWorkforWeek14
{
    public class Perosnvalidator : AbstractValidator<Person>

    {
        
        
        public Perosnvalidator() 
        {

            RuleFor(person => person.Firstname)
               .NotEmpty().WithMessage("Firstname cannot be empty.")
               .Length(0, 50).WithMessage("Firstname must be between 0 and 50 characters.");

            RuleFor(person => person.Lastname)
                .NotEmpty().WithMessage("Lastname cannot be empty.")
                .Length(0, 50).WithMessage("Lastname must be between 0 and 50 characters.");

            RuleFor(person => person.JobPosition)
                .NotEmpty().WithMessage("JobPosition cannot be empty.")
                .Length(0, 50).WithMessage("JobPosition must be between 0 and 50 characters.");

            RuleFor(person => person.Salary)
                .InclusiveBetween(0, 10000).WithMessage("Salary must be between 0 and 10,000.");

            RuleFor(person => person.WorkExperince)
                .NotEmpty().WithMessage("WorkExperience cannot be empty.");

            RuleFor(person => person.Address)
                .NotNull().WithMessage("Address cannot be null.")
                .SetValidator(new AddressValidator()); // Address Validator





        }
        public class AddressValidator : AbstractValidator<Address>
        {
            public AddressValidator()
            {
                RuleFor(address => address.Country)
                    .NotEmpty().WithMessage("Country cannot be empty.");

                RuleFor(address => address.City)
                    .NotEmpty().WithMessage("City cannot be empty.");

                RuleFor(address => address.HomeNumber)
                    .NotEmpty().WithMessage("HomeNumber cannot be empty.");
            }
        }


    }
}
