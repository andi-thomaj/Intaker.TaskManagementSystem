using Intaker.TaskManagement.Contracts;
using Intaker.TaskManagement.EmailNotification.api;
using MassTransit;
using Moq;
using System.Text.Json;
using TaskStatus = Intaker.TaskManagementSystem.domain.TaskManagement.TaskStatus;

namespace Intaker.TaskManagementSystem.UnitTests
{
    public class TaskUpdatedEmailNotificationConsumerTests
    {
        [Fact]
        public async Task Consume_ValidMessage_WritesToConsole()
        {
            var message = new TaskUpdated(1, "Valid Task", "This is a valid task message", TaskStatus.Completed, "Andi");
            var contextMock = new Mock<ConsumeContext<TaskUpdated>>();
            contextMock.Setup(c => c.Message).Returns(message);

            await using var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            var consumer = new TaskUpdatedEmailNotificationConsumer();

            await consumer.Consume(contextMock.Object);

            var expectedOutput = JsonSerializer.Serialize(message) + Environment.NewLine;
            Assert.Equal(expectedOutput, consoleOutput.ToString());
        }
    }
}
