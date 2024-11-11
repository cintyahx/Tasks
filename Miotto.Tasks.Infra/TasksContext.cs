using Microsoft.EntityFrameworkCore;
using Miotto.Tasks.Infra.Mappings;

namespace Miotto.Tasks.Infra
{
    public class TasksContext : DbContext
    {
        protected TasksContext() { }

        public TasksContext(DbContextOptions<TasksContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectMapping());
            modelBuilder.ApplyConfiguration(new ProjectTaskMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}