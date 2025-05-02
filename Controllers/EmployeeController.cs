public class CustomerController(DataContext db, UserManager<AppUser> usrMgr) : Controller
{
  private readonly DataContext _dataContext = db;
  private readonly UserManager<AppUser> _userManager = usrMgr;

  public IActionResult Orders() 
  {
    
  } 
}
