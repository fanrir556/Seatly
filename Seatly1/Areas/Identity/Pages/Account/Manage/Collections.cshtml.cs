using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Seatly1.Data;
using Seatly1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Collections.ObjectModel;

namespace Seatly1.Areas.Identity.Pages.Account.Manage
{
    public class CollectionsModel : CollectionItem
    {
        private readonly SeatlyContext _context;

        public CollectionsModel(SeatlyContext context)
        {
            _context = context;
        }

        public List<CollectionItem> Collections { get; set; }

        public void OnGet()
        {
            var collectionItems = _context.CollectionItems;



        }
        //public async Task<IActionResult> OnGetAsync()
        //{
        //    // ?取?前用?的 ID，假?您使用了 ASP.NET Core Identity
        //    var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    //// ?取?前用?的收藏?据
        //    //Collections = await _context.Collections
        //    //    .Include(c => c.Activity)
        //    //    .Where(c => c.UserName == userName)
        //    //    .ToListAsync();

        //    return Page();
        //}

    }
}

