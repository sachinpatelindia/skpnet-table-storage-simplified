using Microsoft.WindowsAzure.Storage.Table;
using SKP.Net.Core;
using System.Collections.Generic;

namespace SKP.Net.Storage.Operations
{
    public partial interface ITableStorage<T> where T : TableEntity
    {
        T Insert(T entity);
        T Update(T entity);
        T Delete(T entity);
        T Get<T>(object searchPattern) where T : ITableEntity, new();
        IEnumerable<T> GetAll<T>()
      where T : ITableEntity, new();
    }
}
