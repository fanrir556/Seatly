using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Seatly1.Models;
using System.ComponentModel.DataAnnotations;


namespace Seatly1.Areas.Identity.Pages.Account.Manage
{
    public class FriendsModel : Friends
    {
        private readonly SeatlyContext _context;

        public FriendsModel(SeatlyContext context)
        {
            _context = context;
        }

        public List<Friends> FriendsList { get; set; }

        public void OnGet()
        {
            var friends = _context.Friends;

 

        }


    }
}
