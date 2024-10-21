using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ParticipantAndFacilitatorByCategoryDto
    {
        public long Id { get; set; }
        public string Category {  get; set; }
        public int FacilitatorList {  get; set; }
        public int ParticipantList {  get; set; }
    }
}
