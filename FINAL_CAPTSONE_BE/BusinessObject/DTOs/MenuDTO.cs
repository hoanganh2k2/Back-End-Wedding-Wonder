using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class MenuDTO
    {
        public int MenuId { get; set; } 
        public string MenuName { get; set; } = null!; 
        public decimal? Price { get; set; } 
        public string? MenuContent { get; set; } 
        public int CateringId { get; set; } 
        public int MenuType { get; set; } 
        public bool? Status { get; set; } 
    }
}
