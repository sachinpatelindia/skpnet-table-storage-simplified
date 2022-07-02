using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core.Domain.Tracking;
using SKP.Net.Storage.Operations;
using SKP.Net.Web.Areas.Admin.Models.Tracking;
using System;
using System.Linq;

namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class TrackingController : BaseAdminController
    {
        private readonly ITableStorage<WebsiteVisitor> _webSiteVisitor;

        public TrackingController(ITableStorage<WebsiteVisitor> webSiteVisitor)
        {
            _webSiteVisitor = webSiteVisitor;
        }
        public ActionResult Index()
        {
            var visitors = _webSiteVisitor.GetAll<WebsiteVisitor>().Where(v => !v.IPAddress.Contains("::1"))
                .Select(m => new TrackingModel
                {
                    PageUrl = m.HostName + m.RouteName,
                    UserId = m.UserId,
                    VisitedDate =GetDateTime(m.VisitedDate),
                    IPAddress = m.IPAddress,
                    IsBlocked = m.IsBlocked,
                    RowKey = m.RowKey
                }).OrderBy(m => m.VisitedDate);
            return View(visitors);
        }


        public ActionResult Block(string ipAddress)
        {
            var visitors = _webSiteVisitor.GetAll<WebsiteVisitor>().Where(visitor => visitor.IPAddress == ipAddress);
            visitors.ToList().ForEach(visitor =>
            {
                visitor.IsBlocked = true;
                _webSiteVisitor.Update(visitor);
            });
            return RedirectToAction("index");
        }
        public ActionResult Delete(string ipAddress)
        {
            var visitors = _webSiteVisitor.GetAll<WebsiteVisitor>().Where(visitor => visitor.IPAddress == ipAddress);
            visitors.ToList().ForEach(visitor =>
            {
                _webSiteVisitor.Delete(visitor);
            });
            return RedirectToAction("index"); ;
        }

        private DateTime GetDateTime(DateTime dateTime)
        {
            DateTime utc = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            DateTime temp = new DateTime(utc.Ticks, DateTimeKind.Utc);
            DateTime ist = TimeZoneInfo.ConvertTimeFromUtc(temp, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            return ist;
        }
    }
}
