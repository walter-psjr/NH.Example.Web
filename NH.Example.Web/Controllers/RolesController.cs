using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NH.Example.Web.Models;
using NHibernate;
using NHibernate.Linq;

namespace NH.Example.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ISession _session;

        public RolesController(ISession session)
        {
            _session = session;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Role>>> GetAll()
        {
            var roles = await _session.Query<Role>().ToListAsync();

            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetById(Guid id)
        {
            var role = await _session.GetAsync<Role>(id);

            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Role role)
        {
            var transaction = _session.BeginTransaction();

            try
            {
                await _session.SaveAsync(role);

                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetById), new {id = role.Id}, null);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpPut("~/api/users/{userId}/roles")]
        public async Task<IActionResult> AddToUser(Guid userId, Role role)
        {
            var transaction = _session.BeginTransaction();

            try
            {
                var user = await _session.GetAsync<User>(userId);

                if (user == null)
                    return NotFound();

                user.Roles.Add(role);

                await _session.UpdateAsync(user);

                await transaction.CommitAsync();

                return NoContent();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}