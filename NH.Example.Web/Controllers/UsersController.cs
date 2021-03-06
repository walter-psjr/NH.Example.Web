using Microsoft.AspNetCore.Mvc;
using NH.Example.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;

namespace NH.Example.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISession _session;

        public UsersController(ISession session)
        {
            _session = session;
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetAll()
        {
            var users = await _session.Query<User>().ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(Guid id)
        {
            var user = await _session.GetAsync<User>(id);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var transaction = _session.BeginTransaction();

            try
            {
                await _session.SaveAsync(user);

                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetById), new { id = user.Id }, null);
            }
            catch (Exception ex)
            {
                await _session.GetCurrentTransaction().RollbackAsync();

                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, User user)
        {
            var transaction = _session.BeginTransaction();

            try
            {
                await _session.UpdateAsync(user);

                await transaction.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _session.GetCurrentTransaction().RollbackAsync();

                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var transaction = _session.BeginTransaction();

            try
            {
                var user = await _session.GetAsync<User>(id);

                if (user == null)
                    return NotFound("User not found");

                await _session.DeleteAsync(user);

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