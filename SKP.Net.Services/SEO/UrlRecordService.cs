using SKP.Net.Core;
using SKP.Net.Core.Domain.SEO;
using SKP.Net.Storage.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKP.Net.Services.SEO
{
    public partial class UrlRecordService : IUrlRecordService
    {
        private readonly ITableStorage<UrlRecord> _urlRecordStorage;

        public virtual IEnumerable<UrlRecord> UrlRecords => _urlRecordStorage.GetAll<UrlRecord>();

        public UrlRecordService(ITableStorage<UrlRecord> urlRecordStorage)
        {
            _urlRecordStorage = urlRecordStorage;
        }
        public virtual string GetSeName(string entityRowKey, string entityName, bool returnDefaultValue = true)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(result) && returnDefaultValue)
                result = GetActiveSlug(entityRowKey, entityName);
            return result;
        }

        private string GetActiveSlug(string entityRowKey, string entityName)
        {
            var urlRecords = from ur in UrlRecords
                             where ur.EntityRowKey == entityRowKey &&
                             ur.EntityName == entityName &&
                             ur.IsActive
                             orderby ur.EntityName descending
                             select ur.Slug;

            var slug = urlRecords.FirstOrDefault() ?? string.Empty;
            return slug;
        }


        public virtual string GetSeName(string name, bool allowUniCodeCharsInUrls)
        {
            if (string.IsNullOrEmpty(name))
                return name;
            var okChars = "abcdefghijklmnopqrstuvwxyz1234567890 _-";
            name = name.Trim();


            var sb = new StringBuilder();
            foreach (var c in name.ToCharArray())
            {
                var c2 = c.ToString();
                if (allowUniCodeCharsInUrls)
                {
                    if (Char.IsLetterOrDigit(c) || okChars.Contains(c2))
                        sb.Append(c2);
                }
                else if (okChars.Contains(c2))
                {
                    sb.Append(c2);
                }
            }

            var name2 = sb.ToString();
            name2 = name2.Replace(" ", "-");
            while (name2.Contains("--"))
                name2 = name2.Replace("--", "-");
            while (name2.Contains("__"))
                name2 = name2.Replace("__", "_");
            return name2;
        }

        public string GetSeName<T>(T entity, bool returnDefaultValue = true) where T : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            var entityName = entity.GetType().Name;
            return GetSeName(entity.RowKey, entityName, returnDefaultValue);
        }

        public virtual UrlRecord GetBySlug(string slug)
        {
            var urlRecords = from ur in UrlRecords
                             where ur.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase)
                             orderby ur.IsActive descending, ur.RowKey
                             select ur;
            var urlRecord = urlRecords.FirstOrDefault();
            return urlRecord;
        }

        public virtual void InsertUrlRecord(UrlRecord urlRecord)
        {
            if (urlRecord is null)
                throw new ArgumentNullException(nameof(urlRecord));
            _urlRecordStorage.Insert(urlRecord);
        }

        public virtual void UpdateUrlRecord(UrlRecord urlRecord)
        {
            if (urlRecord is null)
                throw new ArgumentNullException(nameof(urlRecord));
            _urlRecordStorage.Update(urlRecord);
        }

        public virtual void DeleteUrlRecord(UrlRecord urlRecord)
        {
            if (urlRecord is null)
                throw new ArgumentNullException(nameof(urlRecord));
            _urlRecordStorage.Delete(urlRecord);
        }

        public virtual void SaveSlug<T>(T entity, string slug) where T : BaseEntity
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));
            var entityId = entity.RowKey;
            var entityName = entity.GetType().Name;

            var query = from ur in UrlRecords
                        where ur.RowKey == entityId &&
                        ur.EntityName == entityName
                        orderby ur.RowKey descending
                        select ur;

            var urlRecords = query.ToList();
            var activeRecord = urlRecords.FirstOrDefault(u => u.IsActive);
            UrlRecord nonActiveUrlRecord;

            if (activeRecord == null && !string.IsNullOrWhiteSpace(slug))
            {
                nonActiveUrlRecord = urlRecords
                    .FirstOrDefault(x => x.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase) && !x.IsActive);
                if (nonActiveUrlRecord != null)
                {
                    nonActiveUrlRecord.IsActive = true;
                    UpdateUrlRecord(nonActiveUrlRecord);
                }
                else
                {
                    var urlRecord = new UrlRecord
                    {
                        EntityRowKey = entityId,
                        EntityName = entityName,
                        Slug = slug,
                        IsActive = true
                    };
                    InsertUrlRecord(urlRecord);

                }
            }

            if (activeRecord != null && string.IsNullOrWhiteSpace(slug))
            {
                activeRecord.IsActive = false;
                UpdateUrlRecord(activeRecord);
            }

            if (activeRecord == null || string.IsNullOrWhiteSpace(slug))
                return;

            if (activeRecord.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase))
                return;

            nonActiveUrlRecord = urlRecords
                .FirstOrDefault(u => u.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase) && !u.IsActive);

            if (nonActiveUrlRecord != null)
            {
                nonActiveUrlRecord.IsActive = true;
                UpdateUrlRecord(nonActiveUrlRecord);

                activeRecord.IsActive = false;
                UpdateUrlRecord(activeRecord);
            }
            else
            {
                var urlRecord = new UrlRecord
                {
                    EntityRowKey = entityId,
                    EntityName = entityName,
                    Slug = slug,
                    IsActive = true
                };
                InsertUrlRecord(urlRecord);

                activeRecord.IsActive = false;
                UpdateUrlRecord(activeRecord);
            }
        }

        public virtual string ValidateSeName<T>(T entity, string seName, string name, bool ensureNotEmpty) where T : BaseEntity
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));
            var entityName = entity.GetType().Name;
            return ValidateSeName(entity.RowKey, entityName, seName, name, ensureNotEmpty);
        }

        public virtual string ValidateSeName(string entityRowKey, string entityName, string seName, string name, bool ensureNotEmpty)
        {
            if (string.IsNullOrWhiteSpace(seName) && !string.IsNullOrWhiteSpace(name))
                seName = name;
            seName = GetSeName(seName, true);

            if (string.IsNullOrWhiteSpace(seName))
            {
                if (ensureNotEmpty)
                {
                    seName = entityRowKey;
                }
                else
                {
                    return seName;
                }
            }
            int i = 2;
            var tempName = seName;
            while (true)
            {
                var urlRecord = GetBySlug(tempName);
                var reserved = urlRecord != null && !(urlRecord.EntityRowKey == entityRowKey && urlRecord.EntityName.Equals(entityName, StringComparison.InvariantCultureIgnoreCase));
                if (!reserved)
                    break;
                tempName = $"{seName}-{i}";
                i++;
            }
            seName = tempName;
            return seName;
        }
    }
}
