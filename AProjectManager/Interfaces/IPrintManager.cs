using System.Collections.Generic;
using AProjectManager.Managers;

namespace AProjectManager.Interfaces
{
    public interface IPrintManager
    {
        List<RepositoryPrintRow> GetRepositoryData();
    }
}