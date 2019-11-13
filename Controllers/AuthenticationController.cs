using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookingMeet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookingMeet.Controllers
{
    [ApiController]
	public class AuthenticationController : ControllerBase
    {
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		public AuthenticationController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		[Route("authentication/SignInWithGoogle")]
		public IActionResult SignInWithGoogle()
		{
			var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action(nameof(HandleExternalLogin)));
			return Challenge(authenticationProperties, "Google");
		}

		[Route("google-callback")]
		public async Task<IActionResult> HandleExternalLogin()
		{
			var info = await _signInManager.GetExternalLoginInfoAsync();
			if (info == null)
				return Redirect("http://localhost:55169");
			var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

			if (!result.Succeeded) //user does not exist yet
			{
				var newUser = new ApplicationUser
				{
					UserName = "Andrew",
					Email = info.Principal.FindFirstValue(ClaimTypes.Email),
					EmailConfirmed = true,
					PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone),
			};
				var createResult = await _userManager.CreateAsync(newUser);
				if (!createResult.Succeeded)
					throw new Exception(createResult.Errors.Select(e => e.Description).Aggregate((errors, error) => $"{errors}, {error}"));

				await _userManager.AddLoginAsync(newUser, info);
				var newUserClaims = info.Principal.Claims.Append(new Claim("userId", newUser.Id));
				await _userManager.AddClaimsAsync(newUser, newUserClaims);
				await _signInManager.SignInAsync(newUser, isPersistent: false);
				await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
			}

			return Redirect("http://localhost:55169/home");
		}

		[Route("authentication/logout")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Redirect("http://localhost:55169/home");
		}

		[Route("authentication/isAuthenticated")]
		public IActionResult IsAuthenticated()
		{
			return new ObjectResult(User.Identity.IsAuthenticated);
		}

		[Route("authentication/fail")]
		public IActionResult Fail()
		{
			return Unauthorized();
		}

		[Route("authentication/name")]
		[Authorize]
		public IActionResult Name()
		{
			var claimsPrincial = (ClaimsPrincipal)User;
			var givenName = claimsPrincial.FindFirst(ClaimTypes.GivenName).Value;
			return Ok(givenName);
		}

		[Route("authentication/[action]")]
		public IActionResult Denied()
		{
			return Content("You need to allow this application access in Google order to be able to login");
		}
	}
}