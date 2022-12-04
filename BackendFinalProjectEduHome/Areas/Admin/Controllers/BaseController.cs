using BackendFinalProjectEduHome.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendFinalProjectEduHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =Constants.AdminRole)]
    public class BaseController : Controller
    {
    }
}
