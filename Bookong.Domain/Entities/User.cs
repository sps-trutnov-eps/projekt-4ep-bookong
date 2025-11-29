using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bookong.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public Guid PublicId { get; set; } = Guid.NewGuid();

        public required string SamAccountName { get; set; }

        public required Guid ObjectGuid { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string OrganizationalUnit { get; set; }

        public ICollection<TeachingMaterial> TeachingMaterials { get; set; } = new List<TeachingMaterial>();
    }
}
