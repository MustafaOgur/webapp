using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; } = false;

        public User User { get; set; } = null!;
    }
}
