﻿
namespace PatientManagement.Api.Parameters
{
    using Domain.ApplicationUser;

    public class CreateProfessionalParameters
    {
        public CreateProfessionalParameters()
        {
        }

        public CreateProfessionalParameters(
            Guid applicationUserId,
            string? title,
            string? firstName,
            string? middleName,
            string? lastName,
            string? phoneNumber,
            int age,
            string? qualification,
            string? license,
            UserRole userRole)
        {
            ApplicationUserId = applicationUserId;
            Title = title;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Age = age;
            Qualification = qualification;
            License = license;
            UserRole = userRole;
        }

        public Guid ApplicationUserId { get; set; }

        public string? Title { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public int Age { get; set; }

        public string? Qualification { get; set; }

        public string? License { get; set; }

        public UserRole UserRole { get; set; }
    }
}
