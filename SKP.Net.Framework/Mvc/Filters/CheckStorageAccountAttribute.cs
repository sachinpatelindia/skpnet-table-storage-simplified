using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SKP.Net.Storage.Common;
using System;

namespace SKP.Net.Web.Framework.Mvc.Filters
{
    public class CheckStorageAccountAttribute : TypeFilterAttribute
    {

        public CheckStorageAccountAttribute() : base(typeof(CheckStorageAccountFilter))
        {

        }
        private class CheckStorageAccountFilter : IActionFilter
        {
            private readonly StorageAccountConnection _storageAccount;
            public CheckStorageAccountFilter(StorageAccountConnection storageAccount)
            {
                _storageAccount = storageAccount;
            }
            public void OnActionExecuted(ActionExecutedContext context)
            {
                
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                try
                {
                    var storageAccount = _storageAccount.CreateBlobStorageAccount();
                }
                catch (FormatException ex)
                {
                    var html = "Check azure storage account connection string " + "[ Original Error : " + ex.Message + "]";
                    context.Result = new ContentResult { Content = html };
                }
            }
        }
    }
}
