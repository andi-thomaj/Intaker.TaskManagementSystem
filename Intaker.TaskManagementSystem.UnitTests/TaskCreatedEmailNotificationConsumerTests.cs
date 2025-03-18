using System.Text.Json;
using Intaker.TaskManagement.Contracts;
using Intaker.TaskManagement.EmailNotification.api;
using MassTransit;
using Moq;
using TaskStatus = Intaker.TaskManagementSystem.domain.TaskManagement.TaskStatus;

namespace Intaker.TaskManagementSystem.UnitTests
{
    public class TaskCreatedEmailNotificationConsumerTests
    {
        [Fact]
        public async Task Consume_ValidMessage_WritesToConsole()
        {
            var message = new TaskCreated(1, "Update Notifications Component", "Notifications Component needs to be updated to Green color", TaskStatus.NotStarted,"Andi");
            var contextMock = new Mock<ConsumeContext<TaskCreated>>();
            contextMock.Setup(c => c.Message).Returns(message);

            await using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            var consumer = new TaskCreatedEmailNotificationConsumer();

            await consumer.Consume(contextMock.Object);

            var expectedOutput = JsonSerializer.Serialize(message) + Environment.NewLine;
            Assert.Equal(expectedOutput, consoleOutput.ToString());
        }
    }
}
