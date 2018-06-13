using System;

namespace DateTimeProviderService
{
    public interface IDateTimeProvider
    {
        DateTime SystemDateTime { get; }
    }
    
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime SystemDateTime => DateTime.UtcNow;
    }
}