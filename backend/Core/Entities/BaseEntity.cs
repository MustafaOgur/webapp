using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BaseEntity : IEntity
    {
        public BaseEntity() { 
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
    }
}