using System.Collections.Generic;
using System.Threading.Tasks;
using Tasinmaz_Proje.Entities;

namespace Tasinmaz_Proje.Services
{
    public interface ILogService
    {
        Task<List<Log>> ListLog();
        Task<Log> GetLogById(int id);
        Task AddLog(Log log);
        Task UpdateLog(Log log);
        Task DeleteLog(int id);
        Task<IEnumerable<Log>> GetAllLogsAsync();
        IEnumerable<Log> SearchLogs(string term);
    }
}
