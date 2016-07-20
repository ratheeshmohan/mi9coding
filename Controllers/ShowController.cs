using System;
using System.Collections;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace mi9challenge.Controllers
{
    [Route("/")]
    public class ShowController : Controller
    {
        [HttpPost]
        public IActionResult Post([FromBody]dynamic showInfo)
        {
            if(!ModelState.IsValid || showInfo == null || showInfo.payload == null) 
             {
                return HandleInvalidRequest();
             }
            else
            {
               try{                
                    var filteredShows = from payload in ((IEnumerable)showInfo.payload).Cast<dynamic>()
                             where
                             payload.drm !=null && bool.Parse(payload.drm.ToString()) &&
                             payload.episodeCount != null  && int.Parse(payload.episodeCount.ToString()) > 0 &&
                             payload.image != null
                             select new {
                                image = payload.image.showImage,
                                slug = payload.slug,
                                title = payload.title
                            };
                     return Json(new { response = filteredShows});
                     }
                    catch(Exception)
                    {                
                        return HandleInvalidRequest();
                    }
            }
         }
        
        private JsonResult HandleInvalidRequest()
        {
          Response.StatusCode = (int)HttpStatusCode.BadRequest;
          return Json(new { error = "Could not decode request: JSON parsing failed"});
        }
    }
}
