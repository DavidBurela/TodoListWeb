using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TodoListWeb.Models;

namespace TodoListWeb.Data
{
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));


            // Requirement #1: login with the following credentials Username: manager / Password: password
            const string userName = "manager";
            const string roleName = "manager";
            const string password = @"password";

            //Create the Role "manager" if it does not exist
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }

            //Create User with role="manager"
            var user = new ApplicationUser { UserName = userName };
            var userResult = userManager.Create(user, password);

            //Add User to Role
            if (userResult.Succeeded)
            {
                var result = userManager.AddToRole(user.Id, roleName);
            }
            base.Seed(context);
        }
    }
}