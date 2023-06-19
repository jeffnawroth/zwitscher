using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using iva_grp7_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace iva_grp7_backend.Controllers;

[Route("api/[controller]")] // api/authentication
[ApiController]
// The [ApiController] attribute adds some default behavior for Web API controllers.
public class AuthenticationController : ControllerBase
{
    // These are the dependencies needed by the controller. They are passed in through the constructor.
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly RoleManager<IdentityRole> roleManager;

    private readonly ApiDbContext _context;

    // The constructor takes in the dependencies and assigns them to the private fields.
    public AuthenticationController(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        ApiDbContext context,
        TokenValidationParameters tokenValidationParameters,
        RoleManager<IdentityRole> roleManager
        )
    {
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
        _tokenValidationParameters = tokenValidationParameters;
        this.roleManager = roleManager;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="requestDto">The user registration request data transfer object (DTO).</param>
    /// <returns>A 200 OK response with the user's first name, last name, email, and JWT token if the registration is successful, or a 400 Bad Request error with the validation errors if the request is invalid.</returns>
    /// <response code="200">If the registration is successful.</response>
    /// <response code="400">If the request is invalid or the email is already registered.</response>
    /// <response code="500">If an exception occurs while registering the user.</response>
    [HttpPost]
        [Route("Register")]
        [ProducesResponseType(200, Type = typeof(AuthResult))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
        {
            // Validate the incoming request
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var user_exist = await _userManager.FindByEmailAsync(requestDto.Email);

                if (user_exist != null)
                {
                    return BadRequest("Email existiert bereits. Bitte einloggen");
                }

                // Create a user
                var newApplicationUser = new ApplicationUser()
                {
                    Email = requestDto.Email,
                    UserName = requestDto.Username,
                    Name = requestDto.Name,
                    Role = Role.User,
                    Gender = null,
                    BirthDate = null,
                    Followers = new List<UserFollower>(),
                    Following = new List<UserFollowing>(),
                    Interests = new List<UserInterest>()
                };

                var is_created = await _userManager.CreateAsync(newApplicationUser, requestDto.Password);

                if (!is_created.Succeeded)
                {
                    // Get the errors from the result
                    var errors = is_created.Errors.Select(e => e.Description);
                    // Return a BadRequest response with the errors
                    return BadRequest(errors);
                }


                if (is_created.Succeeded)
                {

                    // Adding role
                    if (!await roleManager.RoleExistsAsync(Role.User.ToString()))
                        await roleManager.CreateAsync(new IdentityRole(Role.User.ToString()));
                    if (await roleManager.RoleExistsAsync(Role.User.ToString()))
                        await _userManager.AddToRoleAsync(newApplicationUser, Role.User.ToString());
                    // Generate the token
                    var token = await GenerateJwtToken(newApplicationUser);
                    return Ok(token);
                }
                
            }

            return BadRequest();
        }
    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="loginRequest">The user login request data transfer object (DTO).</param>
    /// <returns>A 200 OK response with the user's first name, last name, email JWT token and refresh token if the login is successful, or a 400 Bad Request error with the validation errors if the request is invalid.</returns>
    /// <response code="200">If the login is successful.</response>
    /// <response code="400">If the request is invalid or the email is not registered or the password is incorrect.</response>
    /// <response code="500">If an exception occurs while logging in the user.</response>
    [Route("Login")]
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(AuthResult))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequest)
    {
        if (ModelState.IsValid)
        {
            // Check if user exists
            var existing_user = await _userManager.Users
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .Include(u => u.Interests)
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email);


            if (existing_user == null)
                // Return bad request with error message if user doesn't exist
                return BadRequest("Benutzer existiert nicht.");
            var isCorrect = await _userManager.CheckPasswordAsync(existing_user, loginRequest.Password);
            if (!isCorrect)
                // Return bad request with error message if email and password don't match
                return BadRequest("Email und Passwort stimmen nicht überein.");

            if (existing_user.Locked == true)
                return Forbid();
                
            // Generate JWT token and return it
            var jwtToken = await GenerateJwtToken(existing_user);

            return Ok(jwtToken);
        }
        // Return bad request with error message if model state is invalid
        // Get the error messages from the ModelState
        var errorMessages = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
    
        // Return a BadRequest response with the error messages
        return BadRequest(new { Errors = errorMessages });


    }

    /// <summary>
    /// Refreshes the access token.
    /// </summary>
    /// <param name="TokenRequest">The token request data transfer object (DTO).</param>
    /// <returns>A 200 OK response with the user's first name, last name, email, a new updatet JWT token and a refresh token if the request is successful</returns>
    /// <response code="200">If the request is successful.</response>
    [Route("RefreshToken")]
    [HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
    {
        if (ModelState.IsValid)
        {
            // Verify and generate new token
            var result = await VerifyAndGenerateToken(tokenRequest);

            if (result == null)
            {
                // Return bad request if token is invalid
                return BadRequest("Token ist ungültig.");
            }
            // Return new token if request is valid
            return Ok(result);

        }
        // Return bad request if request is invalid
        return BadRequest("Ungültige Parameter.");
    }

    private async Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        // Clone the validation parameters to avoid modifying the original instance
        var tokenValidationParameters = _tokenValidationParameters.Clone();
        // Disable lifetime validation as the token will have already expired for this operation
        tokenValidationParameters.ValidateLifetime = false;

        // Validate the token
        var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, tokenValidationParameters, out var validedToken);

        if (validedToken is JwtSecurityToken jwtSecurityToken)
        {
            // Check if the algorithm used to sign the token is HmacSha256
            var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

            if (result == false)
                return null;

        }
        // Extract the expiry date from the token
        var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

        // Check if the access token has expired
        if (expiryDate > DateTime.UtcNow)
        {
            var errorObject = new { Error = "Token ist noch nicht abgelaufen." };
            return new AuthResult()
            {
            };
        }


        // Retrieve the stored refresh token
        var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

        // Check if stored refresh token is valid
        if (storedToken == null)
        {
            // If the stored token is null, return an AuthResult with an error message
            return new AuthResult()
            {
            };


        }

        if (storedToken.IsUsed)
        {
            // If the stored token has already been used, return an AuthResult with an error message
            return new AuthResult()
            {
            };
        }

        if (storedToken.IsRevoked)
        {
            // If the stored token has been revoked, return an AuthResult with an error message
            return new AuthResult()
            {
            };
        }

        // Retrieve the "Jti" claim from the token being verified
        var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        // If the stored token's "JwtId" does not match the "Jti" claim, return an AuthResult with an error message
        if (storedToken.JwtId != jti)
        {
            return new AuthResult()
            {
            };
        }

        // If the stored token has expired, return an AuthResult with an error message
        if (storedToken.ExpiryDate < DateTime.UtcNow)
        {
            return new AuthResult()
            {
            };
        }


        // Mark the stored token as used,
        // update it in the database,
        // and generate a new JWT token for the user
        storedToken.IsUsed = true;
        _context.RefreshTokens.Update(storedToken);
        await _context.SaveChangesAsync();

        var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
        return await GenerateJwtToken(dbUser);




    }
    // This method converts a Unix timestamp to a DateTime object
    private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        // Create a DateTime object with the Unix epoch time (1970-01-01 00:00:00 UTC)
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        // Add the number of seconds since the Unix epoch time to the DateTime object
        dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

        // Return the resulting DateTime object
        return dateTimeVal;
    }

    // This method generates a JWT token for the given ApplicationUser object
    private async Task<AuthResult> GenerateJwtToken(ApplicationUser user)
    {
        // Create a JwtSecurityTokenHandler object to handle JWT tokens
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        // Get the secret key from the configuration
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

        // Get the roles of the user from the UserManager
        var userRoles = await _userManager.GetRolesAsync(user);

        // Set the token descriptor with the information needed for the token
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = _configuration.GetSection("JwtConfig:Issuer").Value,
            Audience = _configuration.GetSection("JwtConfig:Audience").Value,
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, value: user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUniversalTime().ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())

            }),


            // Set expire time for token
            Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("JwtConfig:ExpireTimeFrame").Value)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)


        };


        // Tokenhandler to create Token based on the tokenDescriptor information
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        // Generate a refresh token and set its properties
        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            Token = RandomStringGeneration(23), // Generate a refresh token
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            IsUsed = false,
            UserId = user.Id
        };

        // Add the refresh token to the context and save the changes
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        // Return the authentication result with the user's information, the JWT token and refresh token

        // Check if the lists are null before executing the Select-Method
        var followerIds = user.Followers != null 
            ? user.Followers.Select(f => f.FollowerId).ToList() 
            : new List<string>();

        var followingIds = user.Following != null 
            ? user.Following.Select(f => f.FollowingId).ToList() 
            : new List<string>();

        
        var interests = user.Interests != null 
            ? user.Interests.Select(f => f.Interest).ToList() 
            : new List<string>();
            

        
        return new AuthResult()
        {
            Email = user.Email,
            RefreshToken = refreshToken.Token,
            Token = jwtToken,
            Role = user.Role,
            Username = user.UserName,
            Name = user.Name,
            Gender = user.Gender,
            BirthDate = user.BirthDate,
            CreatedAt = user.CreatedAt,
            Bio = user.Bio,
            Locked = user.Locked,
            Avatar = user.Avatar,
            Id = user.Id,
            Following = followingIds,
            Followers = followerIds,
            Interests = interests
        };

    }

    // Generate a random string of a specified length
    private string RandomStringGeneration(int length)
    {
        // Create a new instance of the Random class
        var random = new Random();

        // Define the characters that can be used in the random string
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz_";

        // Select a random character from the string 's' using the Random class instance,
        // and finally, convert the resulting sequence to an array of characters and create a new string
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
    /*
    [Route("DBTest")]
    [HttpGet]
    public IActionResult TestConnection()
    {
        try
        {
            // Versuchen Sie, auf die Datenbank zuzugreifen
            _context.Database.CanConnect();

            // Wenn die Verbindung erfolgreich ist, geben Sie eine Erfolgsmeldung zurück
            return Ok("Database connection successful");
        }
        catch (Exception ex)
        {
            // Wenn die Verbindung fehlschlägt, geben Sie eine Fehlermeldung zurück
            return StatusCode(500, $"Database connection error: {ex.Message}");
        }
    }
    */
}


    

