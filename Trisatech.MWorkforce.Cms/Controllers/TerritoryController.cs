using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trisatech.MWorkforce.Business.Entities;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Cms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Trisatech.AspNet.Common.Helpers;


namespace Trisatech.MWorkforce.Cms.Controllers
{
	[Authorize(Roles = "SA")]
	public class TerritoryController : Controller
	{

		private readonly ITerritoryService territoryService;
		private readonly IUserManagementService userManagementService;
		public TerritoryController(ITerritoryService territoryService, IUserManagementService userManagementService)
		{
			this.userManagementService = userManagementService;
			this.territoryService = territoryService;
		}
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			ViewBag.SelectedMenu = "territory";

			base.OnActionExecuted(context);
		}

		public IActionResult Index()
		{
			return View();
		}


		public IActionResult Details(string id)
		{
			TerritoryModel territoryModel = new TerritoryModel();

			try
			{
				territoryModel = territoryService.Get(id);
			}
			catch (Exception ex)
			{
				return NotFound();
			}

			return View(territoryModel);
		}

		// GET: teritory/Create
		public IActionResult Create()
		{
			TeritoryViewModel model = new TeritoryViewModel();
			long totalSales = userManagementService.TotalUsers();
			var sales = userManagementService.Users("", (int)totalSales, 0).Where(x => x.RoleCode == "SALES").ToList();
			ViewBag.SalesList = sales;
			return View(model);
		}
		// POST: teritory/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create([FromForm]TeritoryViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

					TerritoryModel territory = new TerritoryModel();
					CopyProperty.CopyPropertiesTo(model, territory);

					territory.TerritoryId = Guid.NewGuid().ToString();

					
					territoryService.Add(territory, userAuth.UserId/*, false*/);


					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ViewBag.ErrorMessage = ex.Message;
				}
			}

			long totalSales = userManagementService.TotalUsers();
			var sales = userManagementService.Users("", (int)totalSales, 0).Where(x => x.RoleCode == "SALES").ToList();
			ViewBag.SalesList = sales;
			return View(model);
		}

		[HttpGet]
		public IActionResult Edit(string id)
		{
			TeritoryViewModel model = new TeritoryViewModel();

			try
			{
				if (string.IsNullOrEmpty(id))
					return NotFound();

				var territory = territoryService.Get(id);
				if (territory == null)
					return NotFound();

				CopyProperty.CopyPropertiesTo(territory, model);
			}
			catch
			{
				return NotFound();
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(string id, TeritoryViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);

					TerritoryModel territory = new TerritoryModel();
					CopyProperty.CopyPropertiesTo(model, territory);
					territory.TerritoryId = id;

					territoryService.Edit(territory, userAuth.UserId);
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ViewBag.ErrorMessage = ex.Message;
				}
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(string id)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var userAuth = Trisatech.MWorkforce.Cms.Helpers.AppCookieHelper.Get<UserAuthenticated>(this.HttpContext);
					territoryService.Delete(id, userAuth.UserId);
				}
				catch (Exception ex)
				{
					ViewBag.ErrorMessage = ex.Message;
				}
			}

			return RedirectToAction(nameof(Index));
		}
	}


}

