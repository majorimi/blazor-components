using System;

namespace Majorsoft.Blazor.Server.Logging.Console
{
    internal class NullScope : IDisposable
    {
        private NullScope()
        {
        }

        public static NullScope Instance { get; } = new NullScope();

        public void Dispose()
        {
        }
    }
}