using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ErrorResponse
    {
        public bool Success { get; set; } = false;
        public List<ErrorDetail> Errors { get; set; }
    }
}
