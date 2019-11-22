using System;
using System.ComponentModel.DataAnnotations;

namespace TestPoint.Data.Models
{
    public interface IEntity
    {
        [Key]
         Guid Id { get; set; }
         DateTime Created { get; set; }
         DateTime? Updated { get; set; }
    }
}
