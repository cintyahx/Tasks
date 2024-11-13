using Microsoft.EntityFrameworkCore;
using Miotto.Tasks.Domain.Entities;
using Miotto.Tasks.Infra;
using System.Globalization;

namespace Miotto.Tasks.Tests.Repository
{
    public class TasksContextMock
    {
        public readonly TasksContext _context;
        private readonly Fixture _fixture;

        public Project ProjectWithNoTask;
        public Project Project2;
        public Project Project3;


        public TasksContextMock()
        {
            var options = new DbContextOptionsBuilder<TasksContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new TasksContext(options);

            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            _fixture = FixtureHelper.CreateFixture();
        }

        private void EntitiesSeed()
        {
            ProjectWithNoTask = _fixture.Create<Project>();
            Project2 = _fixture.Create<Project>();
            Project3 = _fixture.Create<Project>();

        }
    }
}
