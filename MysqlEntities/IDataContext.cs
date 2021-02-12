using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace MysqlEntities
{
    public interface IDataContext
    {
        Task<long> AddAsync<T>(T obj);
        Task<long> UpdateAsync<T>(T obj, string q = "", params object[] p);
        Task<List<T>> SelectAsync<T>();
        Task<long> DeleteAsync<T>(string q = "", params object[] p);
    }
}
