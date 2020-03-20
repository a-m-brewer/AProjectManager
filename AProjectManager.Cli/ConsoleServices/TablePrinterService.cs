using System.Collections.Generic;
using AProjectManager.Cli.Interfaces;
using ConsoleTables;

namespace AProjectManager.Cli.ConsoleServices
{
    public class TablePrinterService : ITablePrinterService
    {
        public void Print<T>(IEnumerable<T> rows)
        {
            ConsoleTable
                .From(rows)
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Write(Format.Alternative);
        }
    }
}