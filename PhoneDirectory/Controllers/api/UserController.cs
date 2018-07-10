using PhoneDirectory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PhoneDirectory.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            IList<UserViewModel> users = null;

            using (var ctx = new PhoneDirectoryDBEntities())
            {
                users = ctx.UserDetails.Select(s => new UserViewModel()
                {
                    Id = s.UserId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Status = s.Status,
                    Phone = s.Phone
                }).ToList<UserViewModel>();
            }

            if (users.Count == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpPost]
        public IHttpActionResult CreatNewUser(UserViewModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new PhoneDirectoryDBEntities())
            {
                ctx.UserDetails.Add(new UserDetail()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Status = true
                });

                ctx.SaveChanges();
            }

            return Ok();
        }


        public IHttpActionResult GetUser(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            UserViewModel users = null;

            using (var ctx = new PhoneDirectoryDBEntities())
            {
                users = ctx.UserDetails.Where(s => s.UserId == id)
                .Select(s => new UserViewModel()
                {
                    Id = s.UserId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Phone = s.Phone,
                    Status = s.Status
                }).FirstOrDefault<UserViewModel>();
            }

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpPut]
        public IHttpActionResult UpdateUser(UserViewModel user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");



            using (var ctx = new PhoneDirectoryDBEntities())
            {
                var existingUser = ctx.UserDetails.Where(s => s.UserId == user.Id)
                                                        .FirstOrDefault<UserDetail>();

                if (existingUser != null)
                {
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Email = user.Email;
                    existingUser.Phone = user.Phone;
                    existingUser.Status = user.Status;

                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new PhoneDirectoryDBEntities())
            {
                var user = ctx.UserDetails
                    .Where(s => s.UserId == id)
                    .FirstOrDefault();

                ctx.UserDetails.Remove(user);

                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
