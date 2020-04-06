using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using AuthWithPhoto.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace AuthWithPhoto.Controllers
{
    [Route("Photo")]
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IMemoryCache _cache;

        public PhotoController(UserManager<ApplicationUser> userManager, IMemoryCache cache)
        {
            _userManager = userManager;
            _cache = cache;
        }

        public async Task<IActionResult> Get()
        {
            byte[] photo = null;
            var userid = User.Identity.Name;
            if (userid != null)
            {
                photo = await _cache.GetOrCreateAsync(userid, async cacheitem =>
                {
                    cacheitem.SetSlidingExpiration(TimeSpan.FromMinutes(5));
                    var user = await _userManager.GetUserAsync(User);
                    return user.Photo;
                });
            }
            if (photo != null && photo.Length > 0)
            {
                return File(photo, "image/png");
            }
            else
            {
                return File(Url.Content("~/images/blank.png"), "image/png");
            }
        }
    }
}
