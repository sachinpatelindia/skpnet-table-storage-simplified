using SKP.Net.Core;
using SKP.Net.Core.Domain.SEO;
using System.Collections.Generic;

namespace SKP.Net.Services.SEO
{
    public interface IUrlRecordService
    {
        string GetSeName<T>(T entity, bool returnDefaultValue = true) where T : BaseEntity;
        string GetSeName(string entityRowKey, string entityName, bool returnDefaultValue = true);
        string GetSeName(string name, bool allowUniCodeCharsInUrls);
        void InsertUrlRecord(UrlRecord urlRecord);
        void UpdateUrlRecord(UrlRecord urlRecord);
        void DeleteUrlRecord(UrlRecord urlRecord);
        void SaveSlug<T>(T entity, string slug) where T : BaseEntity;
        string ValidateSeName<T>(T entity, string seName, string name, bool ensureNotEmpty) where T : BaseEntity;
        string ValidateSeName(string entityRowKey, string entityName, string seName, string name, bool ensureNotEmpty);
        IEnumerable<UrlRecord> UrlRecords { get; }
        UrlRecord GetBySlug(string slug);
    }
}
