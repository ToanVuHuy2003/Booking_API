using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using FlutterCinemaAPI.Models;

namespace FlutterCinemaAPI.Controllers.WebAPI
{
    public class SuatChieuController : ApiController
    {
        private cinemaAPIEntities db = new cinemaAPIEntities();

        // GET: api/SuatChieu
        [HttpGet]
        public async Task<List<SuatChieu>> Get()
        {
            List<SuatChieu> suatChieu = await db.SuatChieux.ToListAsync();
            return suatChieu;
        }

        // GET: api/SuatChieu/5
        [ResponseType(typeof(SuatChieu))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            SuatChieu suatChieu = await db.SuatChieux.FindAsync(id);
            if (suatChieu == null)
            {
                return NotFound();
            }

            return Ok(suatChieu);
        }

        // PUT: api/SuatChieu/5
        [ResponseType(typeof(SuatChieu))]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, SuatChieu suatChieu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != suatChieu.MaSC)
            {
                return BadRequest();
            }

            db.Entry(suatChieu).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuatChieuExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(suatChieu);
        }

        // POST: api/SuatChieu
        [ResponseType(typeof(SuatChieu))]
        [HttpPost]
        public async Task<IHttpActionResult> Post(SuatChieu suatChieu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SuatChieux.Add(suatChieu);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SuatChieuExists(suatChieu.MaSC))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = suatChieu.MaSC }, suatChieu);
        }

        // DELETE: api/SuatChieu/5
        [ResponseType(typeof(SuatChieu))]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            SuatChieu suatChieu = await db.SuatChieux.FindAsync(id);
            if (suatChieu == null)
            {
                return NotFound();
            }

            db.SuatChieux.Remove(suatChieu);
            await db.SaveChangesAsync();

            return Ok(suatChieu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SuatChieuExists(int id)
        {
            return db.SuatChieux.Count(e => e.MaSC == id) > 0;
        }
    }
}