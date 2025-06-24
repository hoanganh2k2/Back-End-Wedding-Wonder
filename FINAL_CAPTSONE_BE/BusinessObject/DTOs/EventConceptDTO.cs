using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class EventConceptDTO
    { 
        public int ConceptId { get; set; }
         
        public string ConceptName { get; set; }
         
        public int PackageId { get; set; }
         
        public bool? Status { get; set; }
    }
}
