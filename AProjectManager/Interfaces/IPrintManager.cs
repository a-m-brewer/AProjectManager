using System.Collections.Generic;
using AProjectManager.Managers;
using AProjectManager.Models;

namespace AProjectManager.Interfaces
{
    public interface IPrintManager
    {
        List<RepositoryPrintRow> GetRepositoryData();
        List<SessionPrintRow> GetSessionData();
        List<GroupPrintRow> GetGroupData();
        List<SourcePrintRow> GetRepositorySourcesData();
    }
}