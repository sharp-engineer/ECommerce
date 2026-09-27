using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ECommerceDbContext dbContext, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager, CancellationToken cancellationToken = default)
    {
        const string sellerRole = "Seller";
        const string adminRole = "Admin";

        if (!await roleManager.RoleExistsAsync(sellerRole))
        {
            var result = await roleManager.CreateAsync(new IdentityRole<Guid>(sellerRole));
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to seed role '{sellerRole}': {errors}");
            }
        }
        
        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            var result = await roleManager.CreateAsync(new IdentityRole<Guid>(adminRole));
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to seed role '{adminRole}': {errors}");
            }
        }
        
        if (!await dbContext.Brands.AnyAsync(cancellationToken))
        {
            dbContext.Brands.Add(Brand.Create("Apple"));
        }

        if (!await dbContext.Categories.AnyAsync(cancellationToken))
        {
            dbContext.Categories.Add(Category.Create("Mobile Phones"));
        }

        var seller = await userManager.FindByEmailAsync("seller@test.com");
        if (seller is null)
        {
            seller = new ApplicationUser
            {
                UserName = "seller@test.com",
                Email = "seller@test.com",
                EmailConfirmed = true,
                FirstName = "Test",
                LastName = "Seller"
            };

            var sellerResult = await userManager.CreateAsync(seller, "Password123!");
            if (!sellerResult.Succeeded)
            {
                var errors = string.Join("; ", sellerResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to seed seller user: {errors}");
            }
        }
        if (!await userManager.IsInRoleAsync(seller, sellerRole))
        {
            var roleResult = await userManager.AddToRoleAsync(seller, sellerRole);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join("; ", roleResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to assign seller role: {errors}");
            }
        }

        var admin = await userManager.FindByEmailAsync("admin@test.com");
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com",
                EmailConfirmed = true,
                FirstName = "Test",
                LastName = "Admin"
            };
            
            var adminResult = await userManager.CreateAsync(admin, "Password123!");
            if (!adminResult.Succeeded)
            {
                var errors = string.Join("; ", adminResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to seed admin user: {errors}");
            }
        }
        if (!await userManager.IsInRoleAsync(admin, adminRole))
        {
            var roleResult = await userManager.AddToRoleAsync(admin, adminRole);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join("; ", roleResult.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to assign admin role: {errors}");
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}