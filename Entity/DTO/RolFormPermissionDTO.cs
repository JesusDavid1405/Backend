using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class RolFormPermissionDTO
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int PermissionId { get; set; }
        public int FormId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
