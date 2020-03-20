using System.Collections.Generic;

namespace AProjectManager.Cli.Interfaces
{
    public interface ITablePrinterService
    {
        void Print<T>(IEnumerable<T> rows);
    }
}