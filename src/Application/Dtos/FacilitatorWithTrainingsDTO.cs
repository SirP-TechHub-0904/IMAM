using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class FacilitatorWithTrainingsDTO
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
        public string UserStatus { get; set; }
        public List<string> Trainings { get; set; } = new();
    }
}
