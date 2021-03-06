﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EnglishForKidAPI.Models;
using EnglishForKidAPI.Models.ViewModels;

namespace EnglishForKidAPI.Controllers
{
    public class ViewsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("api/Views/SetViewCount")]
        [HttpGet]
        public IHttpActionResult SetViewCount()
        {
            DateTime today = DateTime.Now;
            View view = db.Views.Where(x => x.Year == today.Year
            && x.Month == today.Month
            && x.Day == today.Day)?.FirstOrDefault();

            if (view != null)
            {
                view.PageView++;
                db.SaveChanges();
                return Ok(GetViewCount());
            }
            else
            {
                db.Views.Add(new View
                {
                    ID = Guid.NewGuid(),
                    PageView = 1,
                    Month = today.Month,
                    Year = today.Year,
                    Day = today.Day

                });
                db.SaveChanges();
                return Ok(GetViewCount());
            }
        }

        public ViewCountViewModel GetViewCount()
        {
            DateTime today = DateTime.Now;
            View view = db.Views.SingleOrDefault(x => x.Day == today.Day
            && x.Month == today.Month
            && x.Year == today.Year);

            ViewCountViewModel viewCountViewModel = new ViewCountViewModel();
            viewCountViewModel.Month = db.Views.Where(x => x.Year == today.Year
            && x.Month == today.Month).Sum(x => x.PageView);
            viewCountViewModel.Year = db.Views.Where(x => x.Year == today.Year).Sum(x => x.PageView);
            viewCountViewModel.Day = view.PageView;
            return viewCountViewModel;
        }

        // GET: api/Views
        public IQueryable<View> GetViews()
        {
            return db.Views;
        }

        // GET: api/Views/5
        [ResponseType(typeof(View))]
        public IHttpActionResult GetView(Guid id)
        {
            View view = db.Views.Find(id);
            if (view == null)
            {
                return NotFound();
            }

            return Ok(view);
        }

        // PUT: api/Views/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutView(Guid id, View view)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != view.ID)
            {
                return BadRequest();
            }

            db.Entry(view).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Views
        [ResponseType(typeof(View))]
        public IHttpActionResult PostView(View view)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Views.Add(view);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ViewExists(view.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = view.ID }, view);
        }

        // DELETE: api/Views/5
        [ResponseType(typeof(View))]
        public IHttpActionResult DeleteView(Guid id)
        {
            View view = db.Views.Find(id);
            if (view == null)
            {
                return NotFound();
            }

            db.Views.Remove(view);
            db.SaveChanges();

            return Ok(view);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ViewExists(Guid id)
        {
            return db.Views.Count(e => e.ID == id) > 0;
        }
    }
}