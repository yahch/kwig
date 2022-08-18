using System;
using System.Collections.Generic;
using System.Text;

namespace KSharpEditor.Args
{
    public class ErrorArgs: EventArgs
    {
        public ErrorArgs(Exception ex)
        {
            exception = ex;
        }
        public Exception exception { get; }
    }
}
