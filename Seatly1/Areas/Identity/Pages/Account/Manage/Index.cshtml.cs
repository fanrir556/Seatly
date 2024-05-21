// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Seatly1.Data;

namespace Seatly1.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        public string MemberRealName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "連絡電話")]
            public string PhoneNumber { get; set; }

            [Display(Name = "性別")]
            public string Sex { get; set; }

            [Display(Name = "生日")]
            [DataType(DataType.Date)]
            public DateTime? Birthday { get; set; } = DateTime.MinValue;

            [Display(Name ="真實姓名")]
            public string MemberRealName { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var realName = user.MemberRealName;
            var sex = user.Sex;  // 加入性別
            var birthday = user.Birthday; // 加入生日
            //var dateTime = new DateTime(1980,01,01);
            var birthdayPlease = "===請選擇===";

            Username = userName;
            MemberRealName = realName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Sex = sex,
                MemberRealName=realName,
                //Birthday = birthday== dateTime ? birthday.Value : dateTime, // 检查生日是否有值，有值则赋值，否则赋默认值
                Birthday = birthday.HasValue ? birthday.Value : null, // 检查生日是否有值，有值则赋值，否则赋默认值

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // 加入電話
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "更新失敗";
                    return RedirectToPage();
                }
            }

            // 加入姓名
            if (Input.MemberRealName != user.MemberRealName)
            {
                user.MemberRealName = Input.MemberRealName;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    StatusMessage = "更新失敗";
                    return RedirectToPage();
                }
            }



            // 加入性別
            if (Input.Sex != user.Sex)
            {
                user.Sex = Input.Sex; 
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    StatusMessage = "更新失敗";
                    return RedirectToPage();
                }
            }

            //加入生日
            if (Input.Birthday != user.Birthday)
            {
                user.Birthday = Input.Birthday; // Update Birthday property
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to update birthday.";
                    return RedirectToPage();
                }
            }




            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "資料已更新！";
            return RedirectToPage();
        }
    }
}
