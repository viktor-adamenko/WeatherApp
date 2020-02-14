using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeatherApp.BusinessLogic;
using WebGrease.Css.Ast.Selectors;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly WeatherApiProcessor _weatherApiProcessor;
        public HomeController(WeatherApiProcessor weatherApiProcessor)
        {
            _weatherApiProcessor = weatherApiProcessor;
        }

        public ActionResult Index()
        {
            ViewBag.LastCityName = Request.Cookies["CityName"]?.Value;
            return View();
        }

        public JsonResult GetWeatherData(string cityName)
        {
            Response.Cookies.Add(new HttpCookie("CityName", cityName));
            var result = _weatherApiProcessor.GetWeatherData(cityName);

            if (result.Rain != null) // Логика которая позволяет пользователю показать сообщение о том что идет дождь только один раз по кождому городу
            {
                // Хранится все это дело в кукисах один день
                // Выглядит конечно запутанно, но на данный момент не мог придумать лучше

                #region UltraLogic               
                
                var cookieExpires = DateTime.Now.AddDays(1);

                var rainNotify = Request.Cookies[result.Name]?.Value;
                if (string.IsNullOrEmpty(rainNotify))
                {
                    Response.Cookies.Add(new HttpCookie(result.Name, "true") {Expires = cookieExpires});

                    var rainNotifyAgain = Request.Cookies["RainNotifyShowAgain"];
                    if (rainNotifyAgain != null)
                    {
                        var tmp = Response.Cookies["RainNotifyShowAgain"];
                        tmp.Value = rainNotifyAgain.Value.Replace(result.Name, "");
                        tmp.Expires = cookieExpires;
                    }                        
                }
                else
                {
                    var rainNotifyAgain = Request.Cookies["RainNotifyShowAgain"];
                    if (rainNotifyAgain != null)
                    {
                        if (!rainNotifyAgain.Value.Contains(result.Name))
                        {
                            var tmp = Response.Cookies["RainNotifyShowAgain"];
                            tmp.Expires = cookieExpires;
                            tmp.Value = rainNotifyAgain.Value + "," + result.Name;
                        }
                    }
                    else
                    {
                        Response.Cookies.Add(new HttpCookie("RainNotifyShowAgain", result.Name) { Expires = cookieExpires });
                    }                                
                }

                #endregion
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}