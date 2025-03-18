namespace Intaker.TaskManagementSystem.infrastructure.Helpers
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public List<string> Errors { get; set; } = [];
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
