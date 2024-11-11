using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miotto.Tasks.Domain.Entities
{
    public class User : BaseEntity
    {
        public int Role { get; set; }
    }
}
