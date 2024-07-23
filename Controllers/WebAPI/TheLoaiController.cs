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
    public class TheLoaiController : ApiController
    {
        private cinemaAPIEntities db = new cinemaAPIEntities();

        // GET: api/TheLoai
        [HttpGet]
        public async Task<List<TheLoai>> Get()
        {
            List<TheLoai> theLoai = await db.TheLoais.ToListAsync();
            return theLoai;
        }

        // GET: api/TheLoai/5
        [ResponseType(typeof(TheLoai))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            TheLoai theLoai = await db.TheLoais.FindAsync(id);
            if (theLoai == null)
            {
                return NotFound();
            }

            return Ok(theLoai);
        }

        // PUT: api/TheLoai/5
        [ResponseType(typeof(TheLoai))]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, TheLoai theLoai)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != theLoai.MaTL)
            {
                return BadRequest();
            }

            db.Entry(theLoai).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TheLoaiExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(theLoai);
        }

        // POST: api/TheLoai
        [ResponseType(typeof(TheLoai))]
        [HttpPost]
        public async Task<IHttpActionResult> Post(TheLoai theLoai)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TheLoais.Add(theLoai);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TheLoaiExists(theLoai.MaTL))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = theLoai.MaTL }, theLoai);
        }

        // DELETE: api/TheLoai/5
        [ResponseType(typeof(TheLoai))]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            TheLoai theLoai = await db.TheLoais.FindAsync(id);
            if (theLoai == null)
            {
                return NotFound();
            }

            db.TheLoais.Remove(theLoai);
            await db.SaveChangesAsync();

            return Ok(theLoai);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TheLoaiExists(int id)
        {
            return db.TheLoais.Count(e => e.MaTL == id) > 0;
        }
    }
}