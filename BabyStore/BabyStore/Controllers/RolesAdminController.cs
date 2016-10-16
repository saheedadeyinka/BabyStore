﻿using BabyStore.Models;
using BabyStore.ViewModels.AdminViewModel;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BabyStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesAdminController : Controller
    {
        public RolesAdminController()
        {

        }
        public RolesAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set { _userManager = value; }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            set { _roleManager = value; }
        }
        // GET: RolesAdmin
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        // GET: RolesAdmin/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var role = await RoleManager.FindByIdAsync(id);

            //Get the list of Users in this Role
            var users = new List<ApplicationUser>();
            foreach (var user in UserManager.Users.ToList())
            {
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                    users.Add(user);
            }

            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();

            return View(role);
        }

        // GET: RolesAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesAdmin/Create
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(roleViewModel.Name);
                var roleResult = await RoleManager.CreateAsync(role);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("", roleResult.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: RolesAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RolesAdmin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RolesAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RolesAdmin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}