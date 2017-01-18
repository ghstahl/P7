// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.DevAuth;
using Microsoft.AspNetCore.Authentication.DevAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Authentication.DevAuth.Areas.Developer.Controllers
{
    public class AutoPostModel
    {
        public string URI { get; set; }
        public Dictionary<string,string> FormDictionary = new Dictionary<string, string>();
    }

    [Area("Developer")]
    public class HomeController : Controller
    {
        //
        // GET: /Home/Authenticate
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Authenticate(string accessToken = null)
        {
            var token = Request.Cookies[DevAuthHandler.StateCookie];
            var data = DeveloperAuthHelpers.GetStateDataFormat().Unprotect(token);
            return View(new AuthenticateViewModel()
            {
                RequestToken = data,
                LoginModel = new LoginModel()
            });
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authenticate(AuthenticateViewModel model, string returnUrl = null)
        {
            var token = Request.Cookies[DevAuthHandler.StateCookie];
            var data = DeveloperAuthHelpers.GetStateDataFormat().Unprotect(token);
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, model.LoginModel.Email);
                if (VerifyMd5Hash(md5Hash, model.LoginModel.Email, hash))
                {
                    var autoPost = new AutoPostModel {URI = data.CallBackUri};
                    autoPost.FormDictionary.Add("_email",model.LoginModel.Email);
                    autoPost.FormDictionary.Add("_userId", hash);
                    return View("AutoPost",autoPost);
//                    return Redirect(data.CallBackUri);
                }
            }

            return View();
        }
        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
