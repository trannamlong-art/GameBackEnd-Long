using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RoleController : Controller
    {
        // Controller actions would go here 
        private ApplicationDbContext _context;
        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var roles = _context.Roles.ToList();
            return View(roles);
        }

        public ActionResult Create()
        {
            return View();
        }
        
        public ActionResult Deleted()
        {
            return View();
        }

        public ActionResult ChangeRoleName()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
             var role = _context.Roles.Find(id);
             if (role == null) return NotFound();
              return View(role);
        }

         [HttpPost]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Roles.Add(role);                   
                 _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        [HttpPost]
        public ActionResult Edit(Role role)
        {
            if (!ModelState.IsValid) return View(role);

            var r = _context.Roles.Find(role.roleId);
            if (r != null)
            {
                r.Name = role.Name;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var role = _context.Roles.Find(id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int roleId)
        {
            var role = _context.Roles.Find(roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }


    }

}