using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace wordmeister_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public string Index() => $"Welcome to WordMeister Api " +
            $"Version:{GetType().Assembly.GetName().Version.ToString()} " +
            $"© 2020 - {DateTime.Now.Year}";

        //Scaffold-DbContext -Connection "data source=localhost;initial catalog=WordMeister;User Id=sa;Password=!wordmeister123;MultipleActiveResultSets=True;App=EntityFramework" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir "Entities/" -Context "WordMeisterDbContext" –Force

    }
}
