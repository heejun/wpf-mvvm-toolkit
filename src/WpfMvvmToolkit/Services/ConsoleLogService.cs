using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMvvmToolkit.Core.Services;

namespace WpfMvvmToolkit.Services
{
    public sealed class ConsoleLogService : ILogService
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }
    }
}
