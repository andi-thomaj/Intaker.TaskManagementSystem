using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intaker.TaskManagementSystem.domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Intaker.TaskManagementSystem.domain.TaskManagement.Configurations
{
    public class TaskConfiguration : BaseConfiguration, IEntityTypeConfiguration<>
    {
    }
}
