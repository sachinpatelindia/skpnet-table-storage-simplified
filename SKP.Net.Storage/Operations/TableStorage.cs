using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SKP.Net.Storage.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SKP.Net.Storage.Operations
{
    /// <summary>
    /// Table storage operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TableStorage<T> : ITableStorage<T> where T : TableEntity
    {
        private readonly StorageAccountConnection _accountConnection;
        public TableStorage(StorageAccountConnection connection)
        {
            _accountConnection = connection;
        }
        public T Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                CloudTable table = CreateTable(entity.GetType().Name);
                TableOperation deleteOperation = TableOperation.Delete(entity);
                var result = table.ExecuteAsync(deleteOperation).Result;
                return result.Result as T;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Get<T>(object searchPattern) where T : ITableEntity, new()
        {
            var typeName = typeof(T).Name;
            CloudTable table = CreateTable(typeName);
            TableQuery<T> query = new TableQuery<T>();
            query.FilterString = "PartitionKey eq '" + typeName + "' and RowKey eq '" + searchPattern + "'";
            var entity = table.ExecuteQuerySegmentedAsync<T>(query, null).Result.FirstOrDefault();
            return entity;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>() where T : ITableEntity, new()
        {
            var type = typeof(T).Name;
            CloudTable table = CreateTable(type);
            var query = new TableQuery<T>();//.Where(TableQuery.GenerateFilterCondition(partitionKey, QueryComparisons.Equal, partitionKey));
            var results = table.ExecuteQuerySegmentedAsync<T>(query, null).Result;
            return results;
        }

        /// <summary>
        /// Insert an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                CloudTable table = CreateTable(entity.GetType().Name);
                TableOperation insertOperation = TableOperation.Insert(entity);
                var result = table.ExecuteAsync(insertOperation).Result;
                return result.Result as T;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                CloudTable table = CreateTable(entity.GetType().Name);
                TableOperation updateOperation = TableOperation.InsertOrReplace(entity);
                var result = table.ExecuteAsync(updateOperation).Result;
                return result.Result as T;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Utilites

        /// <summary>
        /// Create a table if not exist
        /// </summary>
        /// <returns></returns>
        private CloudTable CreateTable(string tableName)
        {
            CloudStorageAccount storageAccount = _accountConnection.CreateStorageAccount();
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);

            if (!table.CreateIfNotExistsAsync().Result)
            {
                //Table created
            }

            return table;

        }
        #endregion
    }
}
