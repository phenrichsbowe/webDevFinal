using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

public class EmployeeController(DataContext db, UserManager<AppUser> usrMgr) : Controller
{
  private readonly DataContext _dataContext = db;
  private readonly UserManager<AppUser> _userManager = usrMgr;

 // public IActionResult Orders() => View();
  public IActionResult Orders() {
    if (User?.Identity?.IsAuthenticated ?? false)
    {
        if (User.IsInRole("northwind-employee"))
        {
            // Get the currently signed-in user's email
            string userEmail = _userManager.GetUserAsync(User).Result?.Email;

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Find the employee ID using the email
                int userId = _dataContext.Employees
                    .FirstOrDefault(e => e.Email == userEmail)?.EmployeeId ?? 0;
            if (userId > 0)
                {
                    // Fetch orders for the employee and pass them to the view
                    var orders = _dataContext.Orders
                        .Where(o => o.EmployeeId == userId)
                        .OrderByDescending(o => o.RequiredDate)
                        .ToList();

                    return View(orders);
                }
                else
                {
                    // Handle case where employee ID is not found
                    ModelState.AddModelError("", "Employee not found.");
                }
            }
            else
            {
                // Handle case where user email is not found
                ModelState.AddModelError("", "User email not found.");
            }
        }
        else
        {
            // Handle case where user is not in the employee role
            ModelState.AddModelError("", "You do not have permission to view this page.");
        }
    }
    else
    {
        // Handle case where user is not authenticated
        ModelState.AddModelError("", "You must be logged in to view this page.");
    }
    return View(new List<Order>()); // Return an empty list if no orders are found or user is not authenticated
  }
    
}
