﻿using Microsoft.AspNetCore.Identity;
using ToDoWebAPI.Data.Models;

namespace ToDoWebAPI.Dtos.Seed
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = new[] { RoleTypes.SuperAdmin, RoleTypes.Admin, RoleTypes.Employee };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string superAdminEmail = "superadmin1@example.com";
            string superAdminPassword = "SuperAdmin123!";

            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);
            if (superAdmin == null)
            {
                superAdmin = new User
                {
                    UserName = superAdminEmail,
                    Email = superAdminEmail,
                    FirstName = "System",
                    LastName = "SuperAdmin",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(superAdmin, superAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, RoleTypes.SuperAdmin);
                }
            }
        }
    }
}
