using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Interfaces;
using Talabat.DTO.IdentityDTO;
using Talabat.Errors;
using Talabat.Services.Services;


namespace Talabat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// Controller for managing user accounts and authentication operations.
    /// Provides API endpoints for user registration, login, profile management, password reset, and related account actions.
    /// Supports JWT authentication and integrates with email services for password recovery.
    /// All endpoints return structured responses and handle errors gracefully.
    /// </summary>
    public class Accountcontroller : ControllerBase
    {
        private readonly UserManager<UserApp> userManager;
        private readonly SignInManager<UserApp> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenService tokenService;
        private readonly IEmailService emailService;

        public Accountcontroller(UserManager<UserApp> userManager,SignInManager<UserApp> signInManager,RoleManager<IdentityRole> roleManager,ITokenService tokenService,IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.tokenService = tokenService;
            this.emailService = emailService;
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="registerDTO">The registration data for the new user.</param>
        /// <returns>The created user data with JWT token, or an error if registration fails.</returns>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(ApiHandleError),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserDTO),StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiHandleError(400, "invalid Request"));
            var checkuser = await userManager.FindByEmailAsync(registerDTO.Email);
            if (checkuser != null)
                return BadRequest(new ApiHandleError(400, "Email already found"));
            var user = new UserApp()
            {
                FName = registerDTO.Fname,
                LName = registerDTO.Lname,
                UserName = $"{registerDTO.Fname}_{registerDTO.Lname}",
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                Country = registerDTO.Country,
                City=registerDTO.City,

            };
            var result=await userManager.CreateAsync(user,registerDTO.Password);
            if (result.Succeeded)
                return Ok(new UserDTO()
                {
                    DisplayName = user.UserName,
                    Email = user.Email,
                    Token = await tokenService.GetToken(user, userManager)
                });

            return BadRequest(new ApiHandleError(400, "Faild to create account"));
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="loginDto">The login credentials (email and password).</param>
        /// <returns>User data with JWT token, or an error if authentication fails.</returns>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(ApiHandleError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDTO>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiHandleError(400, "Invalid Request"));
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return BadRequest(new ApiHandleError(400, "Email not found"));

            var checkpassword = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!checkpassword)
                return BadRequest(new ApiHandleError(400, "Invalid password"));

            var result = await signInManager.PasswordSignInAsync(user, loginDto.Password, true, false);
            if (!result.Succeeded)
                return BadRequest(new ApiHandleError(400, "Failed to login"));
            return Ok(new UserDTO()
            {
                DisplayName = user.UserName,
                Email = user.Email,
                Token = await tokenService.GetToken(user, userManager)
            });
        }       

        /// <summary>
        /// Retrieves the currently authenticated user's information.
        /// </summary>
        /// <returns>The current user's data with a fresh JWT token, or an error if not authenticated.</returns>
        [HttpGet("CurrentUser")]
        [ProducesResponseType(typeof(ApiValidationError), StatusCodes.Status400BadRequest)]
        [Authorize]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiHandleError), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> CurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return BadRequest(new ApiValidationError()
                {
                    Errors = new List<string>() { "Please log in to continue" }
                });
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound(new ApiHandleError(404, "User not found"));

            return Ok(new UserDTO()
            {
                DisplayName = user.UserName,
                Email = user.Email,
                Token = await tokenService.GetToken(user, userManager)
            });

        }

        /// <summary>
        /// Updates the profile information of the currently authenticated user.
        /// </summary>
        /// <param name="updateUserDTO">The new profile data for the user.</param>
        /// <returns>The updated user data, or an error if the update fails.</returns>
        [Authorize]
        [HttpPost("UpdateUserProfile")]
        [ProducesResponseType(typeof(ApiValidationError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UpdateUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiHandleError), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpdateUserDTO>> UpdateUserProfile(UpdateUserDTO updateUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiHandleError(400, "Invalid model state"));
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return BadRequest(new ApiValidationError()
                {
                    Errors = new List<string>() { "Please log in to continue " }
                });
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound(new ApiHandleError(404, "User not found"));

            user.FName = updateUserDTO.Fname;
            user.LName = updateUserDTO.Lname;
            user.UserName = $"{updateUserDTO.Fname}_{updateUserDTO.Lname}";
            user.Email = updateUserDTO.Email;
            user.Country = updateUserDTO.Country;
            user.City = updateUserDTO.City;
            user.PhoneNumber = updateUserDTO.Phone;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiValidationError()
                {
                    Errors = result.Errors.Select(e => e.Description).ToList()
                });
            return Ok(updateUserDTO);
        }

        /// <summary>
        /// Sends a password reset link to the user's email address.
        /// </summary>
        /// <param name="forgetPassword">The email address for which to send the reset link.</param>
        /// <returns>A message indicating whether the reset link was sent.</returns>
        [HttpPost("ForgetPassword")]
        [ProducesResponseType(typeof(ApiHandleError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> ForgetPassword(ForgetPassword forgetPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiValidationError()
                {
                    Errors = new List<string>() { "Invalid model state" }
                });

            var user = await userManager.FindByEmailAsync(forgetPassword.Email);
            if (user == null)
                return NotFound(new ApiHandleError(404, "Email not found"));
            var token=await userManager.GeneratePasswordResetTokenAsync(user);
            var urllink = Url.Action("ResetPassword", "Account", new {token, Email = forgetPassword.Email }, Request.Scheme);
            await emailService.SendMailAsync(user.Email, $"click here \n {urllink}", "Reset Password");
            return Ok(new
            {
                message = "If the email is valid, a reset link will be sent to your email. Please check your inbox.",
                token = token
            });
        }

        /// <summary>
        /// Resets the user's password using the provided token and new password.
        /// </summary>
        /// <param name="resetPassword">The reset token, email, and new password data.</param>
        /// <returns>A message indicating whether the password was reset successfully.</returns>
        [HttpPost("ResetPassword")]
        [ProducesResponseType(typeof(ApiHandleError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> ResetPassword(ResetPassword resetPassword)
        {
           
            var user = await userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
                return NotFound(new ApiHandleError(404, "Email not found"));
            var result = await userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
            if (!result.Succeeded)
                return BadRequest(new ApiValidationError()
                {
                    Errors = result.Errors.Select(e => e.Description).ToList()
                });
            if (User.Identity.IsAuthenticated)
                await signInManager.SignOutAsync();


            return Ok("reset password successfully");
        }
    }
}
