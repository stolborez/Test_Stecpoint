using System;

namespace Stecpoint.Data
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public Organization Organization { get; set; }
    }
}