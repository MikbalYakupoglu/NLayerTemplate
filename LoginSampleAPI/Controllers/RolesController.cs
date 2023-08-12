using Business.Abstract;
using Business.Utils;
using Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSampleAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = AuthorizationRoles.Admin)]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(Role role)
        {
            var result = await _roleService.CreateAsync(role);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid roleId)
        {
            var result = await _roleService.DeleteAsync(roleId);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        //[HttpPatch("update")]
        //public async Task<IActionResult> Update(int roleId, Role newRole)
        //{
        //    var result = await _roleService.UpdateAsync(roleId, newRole);

        //    if (!result.Success)
        //        return BadRequest(result);

        //    return Ok(result);
        //}

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAllAsync();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
