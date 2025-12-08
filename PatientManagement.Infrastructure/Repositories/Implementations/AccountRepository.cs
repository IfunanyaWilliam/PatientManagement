
namespace PatientManagement.Infrastructure.Repositories.Implementations
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using DbContexts;
    using Microsoft.AspNetCore.Http;
    using Domain.ApplicationUser;
    using Application.Utilities;
    using Application.Interfaces.Repositories;
    using PatientManagement.Domain.Account;

    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Entities.ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ILogger<AccountRepository> _logger;

        public AccountRepository(
            AppDbContext context,
            UserManager<Entities.ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            ILogger<AccountRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }


        public async Task<CreateUserResultDto> CreateUserAsync(
            string email,
            string password,
            UserRole role,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                throw new CustomException($"user with email {email} already exist", StatusCodes.Status400BadRequest);
            }

            bool roleExists = await _roleManager.RoleExistsAsync(role.ToString());

            if (!roleExists)
            {
                throw new CustomException($"UserRole {role} does not exist", StatusCodes.Status400BadRequest);
            }

            var newUser = new Entities.ApplicationUser
            {
                Email = email,
                UserName = email,
                Role = role
            };

            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, role.ToString());
            }


            return new CreateUserResultDto(
                userId: newUser.Id,
                email: email,
                userRole: role.ToString(),
                created: newUser.DateCreated);
        }

        //Add update user method



        //Add a method to insert user Login when user logs in normally



        public async Task<bool> InsertFacebookLoginAsync(Guid userId, string facebookId, CancellationToken cancellationToken = default)
        {
            var userLogin = new Entities.ExternalLogin
            {
                ApplicationUserId = userId,
                Provider = "Facebook",
                ProviderUserId = facebookId,
                LinkedAt = DateTime.UtcNow
            };

            await _context.ExternalLogins.AddAsync(userLogin, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }



        //Add a method to insert user Login when user logs with google
    }
}
