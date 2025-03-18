using System.Net.Http.Json;
using FluentAssertions;
using Intaker.TaskManagementSystem.api.Controllers.TaskManagement.Commands;
using TaskStatus = Intaker.TaskManagementSystem.domain.TaskManagement.TaskStatus;

namespace Intaker.TaskManagementSystem.api.IntegrationTests
{
    public class TaskTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task GetAllTasks_ShouldReturn200Ok()
        {
            var response = await _client.GetAsync("/api/tasks");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetTaskById_ShouldReturnTheTask()
        {
            var response = await _client.GetAsync("/api/tasks/1");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetTaskByName_ShouldReturnTheTask()
        {
            string taskName = Uri.EscapeDataString("Task 1");
            var response = await _client.GetAsync($"/api/tasks/name/{taskName}");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateTask_IsRetrievedSuccessfully_AfterInserted()
        {
            var command = new CreateTaskCommand("Create new endpoint", "Create new endpoint in the orders microservice",
                TaskStatus.NotStarted, "Andi");
            await _client.PostAsJsonAsync("/api/tasks", command);

            string taskName = Uri.EscapeDataString(command.Name);
            var response = await _client.GetAsync($"/api/tasks/name/{taskName}");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpdateTask_IsRetrievedSuccessfully_AfterUpdated()
        {
            var command = new CreateTaskCommand("Create new endpoint", "Create new endpoint in the orders microservice",
                TaskStatus.NotStarted, "Andi");
            await _client.PostAsJsonAsync("/api/tasks", command);

            var taskCreatedResponse = await _client.GetAsync($"/api/tasks/name/{command.Name}");
            var taskCreated = await taskCreatedResponse.Content.ReadFromJsonAsync<TaskDto>();

            taskCreated!.Status = TaskStatus.InProgress;
            var response = await _client.PutAsJsonAsync("/api/tasks",
                new { id = taskCreated.Id, status = taskCreated.Status });

            var taskUpdatedResponse = await _client.GetAsync($"/api/tasks/name/{taskCreated.Name}");
            var taskUpdated = await taskUpdatedResponse.Content.ReadFromJsonAsync<TaskDto>();

            taskUpdated!.Status.Should().Be(TaskStatus.InProgress);
        }

        private class TaskDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public TaskStatus Status { get; set; }
            public string AssignedTo { get; set; }
        }
    }
}
