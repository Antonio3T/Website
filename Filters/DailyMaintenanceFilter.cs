using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Website.Filters
{
    public class DailyMaintenanceFilter : ActionFilterAttribute
    {
        public int[] From { get; set; }
        public int[] To { get; set; }

        TimeSpan _From, _To;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _From = new TimeSpan(From[0], From[1], From[2]);
            _To = new TimeSpan(To[0], To[1], To[2]);

            DateTime _justNow = DateTime.Now;
            TimeSpan _now = new TimeSpan(_justNow.Hour, _justNow.Minute, _justNow.Second);

            if ((_From <= _To && _now >= _From && _now <= _To) || (_From > _To && (_now > _From || _now < _To)))
            {
                context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new { controller = "Security", action = "Maintenance" })
                        );
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }
    }
}