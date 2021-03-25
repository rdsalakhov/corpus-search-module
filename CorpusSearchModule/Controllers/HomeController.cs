using CorpusSearchModule.Core.Models;
using CorpusSearchModule.Core.Models.Exceptions;
using CorpusSearchModule.Core.Services;
using CorpusSearchModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CorpusSearchModule.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDriverManager _driverManager;
        private readonly IQueryParser _queryParser;

        public HomeController()
        {
            _driverManager = new DriverManager();
            _queryParser = new QueryParser();
        }

        public ActionResult ExecuteQuery(string query, string driverName)
        {
            IntermediateQuery queryObj;
            try
            {
                queryObj = _queryParser.ParseQuery(query);
                List<QueryResult> results = _driverManager.ExecuteQuery(queryObj, driverName);
                return PartialView("QueryResultPartial", results);
            }
            catch (InvalidQuerySyntaxException e)
            {
                return PartialView("FailedQueryErrorMessage", $"Syntax error: {e.UnnesessarySyntax}");
            }
            catch (UnknownDriverException e)
            {
                return PartialView("FailedQueryErrorMessage", $"Unknown driver: {e.DriverName}");
            }
            catch (EmptyQueryException e)
            {
                return PartialView("FailedQueryErrorMessage", $"{e.Message}");
            }
            catch (Exception e)
            {
                return PartialView("FailedQueryErrorMessage", $"Failed to execute query: {e.Message}");
            }
        }

        public ActionResult Index()
        {
            var list = _driverManager.GetAvailableDrivers();
            SelectList driverNames = new SelectList(list);
            ViewBag.Drivers = driverNames;
            return View();
        }

        [ActionName("FillQuery")]
        public ActionResult FillQuery(string query)
        {
            var list = _driverManager.GetAvailableDrivers();
            SelectList driverNames = new SelectList(list);
            ViewBag.Drivers = driverNames;
            
            return View(viewName: "Index", model: query);
        }

        public void SaveQuery(string query)
        {
            SavedQuery.queries.Add(new SavedQuery
            {
                CreatedAt = DateTime.Now,
                Query = query
            });
        }

        public ActionResult QueryConstructor()
        {
            var list = _driverManager.GetAvailableDrivers();
            SelectList driverNames = new SelectList(list);
            ViewBag.Drivers = driverNames;
            return View();
        }

        public ActionResult SavedQueries()
        {
            
            return View(SavedQuery.queries);
        }       
    }
}