using Newtonsoft.Json;
using PhoneDirectory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace PhoneDirectory.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// This action method will get all the contact information 
        /// call web api GetAllUsers()
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public ActionResult Index(string sortOrder, string searchString)
        {
            if (String.IsNullOrEmpty(sortOrder))
                ViewBag.FNameSortParm = "fname";

            ViewBag.FNameSortParm = sortOrder == "fname" ? "fname_desc" : "fname";

            ViewBag.LNameSortParm = sortOrder == "lname" ? "lname_desc" : "lname";

            IEnumerable<UserViewModel> users = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:18054/api/");
                //HTTP GET
                var responseTask = client.GetAsync("user");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<UserViewModel>>();
                    readTask.Wait();

                    users = readTask.Result;

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        users = users.Where(s => s.FirstName.ToLower().Contains(searchString.Trim())
                                               || s.LastName.ToLower().Contains(searchString.Trim()));
                    }

                    if (sortOrder != null)
                    {
                        switch (sortOrder)
                        {
                            case "fname_desc":
                                users = users.OrderByDescending(s => s.FirstName);
                                break;
                            case "lname_desc":
                                users = users.OrderByDescending(s => s.LastName);
                                break;
                            case "fname":
                                users = users.OrderBy(s => s.FirstName);
                                break;
                            case "lname":
                                users = users.OrderBy(s => s.LastName);
                                break;
                        }
                    }
                }
                else
                {
                    users = Enumerable.Empty<UserViewModel>();
                    ModelState.AddModelError(string.Empty, result.ReasonPhrase);
                }
            }


            return View(users);
        }

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Pass UserViewModel object to api and add record to database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(UserViewModel user)
        {
            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:18054/api/user");

                    //HTTP POST
                    var createContact = client.PostAsJsonAsync("user", user);
                    createContact.Wait();

                    var result = createContact.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }

                ModelState.AddModelError(string.Empty, "Error Occured!");

            }
            return View(user);
        }

        /// <summary>
        /// Used to edit contact information.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            UserViewModel student = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:18054/api/");
                //HTTP GET
                var getContact = client.GetAsync("user/?id=" + id.ToString());
                getContact.Wait();

                var result = getContact.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<UserViewModel>();
                    readTask.Wait();

                    student = readTask.Result;
                }
            }
            return View(student);
        }

        /// <summary>
        /// Used to updated information to database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(UserViewModel user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:18054/api/user");

                //HTTP POST
                var updateContact = client.PutAsJsonAsync<UserViewModel>("user", user);
                updateContact.Wait();

                var result = updateContact.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        /// <summary>
        /// Delete contact information of specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:18054/api/");
                //HTTP GET
                var deleteContact = client.DeleteAsync("user/" + id.ToString());
                deleteContact.Wait();

                var result = deleteContact.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}