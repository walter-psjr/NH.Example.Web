using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NH.Example.Web.Models;
using NHibernate;

namespace NH.Example.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonesController : ControllerBase
    {
        private readonly ISession _session;

        public PhonesController(ISession session)
        {
            _session = session;
        }

        [HttpGet("~/api/users/{userId}/phones")]
        public async Task<ActionResult<IList<Phone>>> GetById(Guid userId)
        {
            var user = await _session.GetAsync<User>(userId);

            if (user == null)
                return NotFound();

            var phones = user.Phones;

            return Ok(phones);
        }

        [HttpGet("~/api/users/{userId}/phones/{phoneId}")]
        public async Task<ActionResult<IList<Phone>>> GetById(Guid userId, Guid phoneId)
        {
            var phone = await _session.GetAsync<Phone>(phoneId);

            if (phone == null)
                return NotFound();

            return Ok(phone);
        }

        [HttpPost("~/api/users/{userId}/phones")]
        public async Task<IActionResult> Create(Guid userId, Phone phone)
        {
            var transaction = _session.BeginTransaction();

            try
            {
                var user = await _session.GetAsync<User>(userId);

                if (user == null)
                    return NotFound();

                phone.User = user;
                user.Phones.Add(phone);

                await _session.UpdateAsync(user);

                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetById), new { userId = user.Id, phoneId = phone.Id }, null);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpPut("~/api/users/{userId}/phones/{phoneId}")]
        public async Task<IActionResult> Put(Guid userId, Guid phoneId, Phone phone)
        {
            var transaction = _session.BeginTransaction();

            try
            {
                var user = await _session.GetAsync<User>(userId);

                if (user == null)
                    return NotFound();

                var phoneToUpdate = user.Phones.FirstOrDefault(x => x.Id == phoneId);
                phoneToUpdate.Number = phone.Number;

                await _session.UpdateAsync(user);

                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetById), new { userId = user.Id, phoneId = phone.Id }, null);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpDelete("~/api/users/{userId}/phones/{phoneId}")]
        public async Task<IActionResult> Delete(Guid userId, Guid phoneId)
        {
            var transaction = _session.BeginTransaction();

            try
            {
                var user = await _session.GetAsync<User>(userId);

                if (user == null)
                    return NotFound("User not found");

                var phone = user.Phones.FirstOrDefault(x => x.Id == phoneId);

                if (phone == null)
                    return NotFound("Phone not found");

                user.Phones.Remove(phone);

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