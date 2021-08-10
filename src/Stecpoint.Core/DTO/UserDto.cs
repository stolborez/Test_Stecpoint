using System;
using Stecpoint.Data;

namespace Stecpoint.Core.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
    }
}