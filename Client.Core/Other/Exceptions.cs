using System;

namespace UASD
{
    public class NotLoggedInException: Exception
    {
        public NotLoggedInException(): base(Properties.Strings.NotLoggedInMessage) { }
    }
    public class NoDataReceivedException: Exception { }
    public class NoProyectionAvailableException: Exception
    {
        public NoProyectionAvailableException(): base(Properties.Strings.NoProyectionMessage) { }
    }
    public class NoSelectionAvailableException: Exception
    {
        public NoSelectionAvailableException(): base(Properties.Strings.NoSelectionMessage) { }
    }
    public class SeleccionErrorsException: Exception
    {
        public SeleccionErrorsException(): base() { }
        public SeleccionErrorsException(string message): base(message) { }
        public SeleccionErrorsException(string message, Exception inner): base(message, inner) { }
    }
    public class ExpiredLoginException: Exception
    {
        public ExpiredLoginException() : base(Properties.Strings.ExpiredLoginMessage) {}
    }
}
