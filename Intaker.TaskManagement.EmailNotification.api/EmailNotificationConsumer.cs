using System.Text.Json;
using Intaker.TaskManagement.Contracts;
using MassTransit;

namespace Intaker.TaskManagement.EmailNotification.api
{
    public class TaskCreatedEmailNotificationConsumer : IConsumer<TaskCreated>
    {
        public Task Consume(ConsumeContext<TaskCreated> context)
        {
            Console.WriteLine(JsonSerializer.Serialize(context.Message));
            // send email notification

            return Task.CompletedTask;
        }
    }

    public class TaskUpdatedEmailNotificationConsumer : IConsumer<TaskUpdated>
    {
        public async Task Consume(ConsumeContext<TaskUpdated> context)
        {
            Console.WriteLine(JsonSerializer.Serialize(context.Message));
            // send email notification

            await Task.CompletedTask;

            if (new Random().Next(0, 3) == 0)
            {
                throw new Exception("Transient failure occurred!");
            }
        }
    }
}
