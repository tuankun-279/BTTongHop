using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WinStore.Models;

namespace WinStore.Controllers
{
    public class AssignTasksController : Controller
    {
        private Model_Project_Context db = new Model_Project_Context();

        // GET: AssignTasks
        public ActionResult Index(string SearchString, string Department,string Company,string Date,string SortOrder, int? Page_No)
        {
            ViewBag.SortingByEmployeeName = String.IsNullOrEmpty(SearchString) ? "EmployeeName_Sort" : "";
            ViewBag.SortingByClientName = String.IsNullOrEmpty(SearchString) ? "ClientName_Sort" : "";
            ViewBag.SortingByProjectName = String.IsNullOrEmpty(SearchString) ? "ProjectName_Sort" : "";

            var assignTasks = from a in db.AssignTasks
                              join e in db.Employees
                              on a.EmployeeId equals e.EmployeeId
                              join c in db.Clients
                              on a.ClientId equals c.ClientId
                              join p in db.Projects
                              on a.ProjectId equals p.ProjectId
                              select a;
            //dropdown Department
            var DepartmentLst = new List<String>();
            var DepartmentQry = from d in db.Employees
                                orderby d.EmployeeId
                                select d.EmployeeDepartment;
            DepartmentLst.AddRange(DepartmentQry.Distinct());
            ViewBag.Department = new SelectList(DepartmentLst);
            //dropdown Company
            var CompanyLst = new List<String>();
            var CompanyQry = from c in db.Clients
                                orderby c.ClientId
                                select c.ClientCompany;
            CompanyLst.AddRange(CompanyQry.Distinct());
            ViewBag.Company = new SelectList(CompanyLst);

            var DateLst = new List<String>() { "Đang thực hiện", "Đã hoàn thành" };
            ViewBag.Date = new SelectList(DateLst);
            //
            if (!String.IsNullOrEmpty(SearchString))
            {
                assignTasks = assignTasks.Where(a => a.Employee.EmployeeName.Contains(SearchString)
                || a.Client.ClientName.Contains(SearchString)
                || a.Project.ProjectName.Contains(SearchString));
            }
            if (!String.IsNullOrEmpty(Department))
            {
                assignTasks = assignTasks.Where(a => a.Employee.EmployeeDepartment.Contains(Department));
            }
            if (!String.IsNullOrEmpty(Company))
            {
                assignTasks = assignTasks.Where(a => a.Client.ClientCompany.Contains(Company));
            }

            switch (Date)
            {
                case "Đang thực hiện":
                    assignTasks = assignTasks.Where(a => a.Project.ProjectEnd == null
                    || a.Project.ProjectEnd > DateTime.Now);
                    break;
                case "Đã hoàn thành":
                    assignTasks = assignTasks.Where(a => a.Project.ProjectEnd != null
                    && a.Project.ProjectEnd < DateTime.Now);
                    break;
            }
            switch (SortOrder)
            {
                case "EmployeeName_Sort":
                    assignTasks = assignTasks.OrderByDescending(a => a.Employee.EmployeeName);
                    break;
                case "ClientName_Sort":
                    assignTasks = assignTasks.OrderBy(a => a.Client.ClientName);
                    break;
                case "ProjectName_Sort":
                    assignTasks = assignTasks.OrderBy(a => a.Project.ProjectName);
                    break;
                default:
                    assignTasks = assignTasks.OrderBy(a => a.Employee.EmployeeName);
                    break;
            }
            int Page_Size = 4;
            int No_Of_Page = (Page_No ?? 1);
            //var assignTasks = db.AssignTasks.Include(a => a.Client).Include(a => a.Employee).Include(a => a.Project);
            return View(assignTasks.ToPagedList(No_Of_Page,Page_Size));
        }

        // GET: AssignTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignTask assignTask = db.AssignTasks.Find(id);
            if (assignTask == null)
            {
                return HttpNotFound();
            }
            return View(assignTask);
        }

        // GET: AssignTasks/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName");
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName");
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName");
            return View();
        }

        // POST: AssignTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssignTaskId,EmployeeId,ClientId,ProjectId,Task,Note")] AssignTask assignTask)
        {
            if (ModelState.IsValid)
            {
                db.AssignTasks.Add(assignTask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName", assignTask.ClientId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName", assignTask.EmployeeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName", assignTask.ProjectId);
            return View(assignTask);
        }

        // GET: AssignTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignTask assignTask = db.AssignTasks.Find(id);
            if (assignTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName", assignTask.ClientId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName", assignTask.EmployeeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName", assignTask.ProjectId);
            return View(assignTask);
        }

        // POST: AssignTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssignTaskId,EmployeeId,ClientId,ProjectId,Task,Note")] AssignTask assignTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName", assignTask.ClientId);
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "EmployeeName", assignTask.EmployeeId);
            ViewBag.ProjectId = new SelectList(db.Projects, "ProjectId", "ProjectName", assignTask.ProjectId);
            return View(assignTask);
        }

        // GET: AssignTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssignTask assignTask = db.AssignTasks.Find(id);
            if (assignTask == null)
            {
                return HttpNotFound();
            }
            return View(assignTask);
        }

        // POST: AssignTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssignTask assignTask = db.AssignTasks.Find(id);
            db.AssignTasks.Remove(assignTask);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
