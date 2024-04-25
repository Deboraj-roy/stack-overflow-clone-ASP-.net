namespace Stackoverflow.Domain.Exceptions
{
    public class DuplicateTitleException : Exception
    {
        public DuplicateTitleException() : base("Title is duplicate") { }
    }
}
