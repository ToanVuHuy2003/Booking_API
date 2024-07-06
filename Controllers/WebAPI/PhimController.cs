using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using FlutterCinemaAPI.Models;

namespace FlutterCinemaAPI.Controllers.WebAPI
{
    public class PhimController : ApiController
    {
        private cinemaEntities db = new cinemaEntities();

        // GET: api/Phim
        [HttpGet]
        public async Task<List<Phim>> Get()
        {
            List<Phim> films = await db.Phims.ToListAsync();    

            return films;   
        }

        // GET: api/Phim/5
        [HttpGet]
        [ResponseType(typeof(Phim))]
        public async Task<IHttpActionResult> Get(string id)
        {
            Phim phim = await db.Phims.FindAsync(id);
            if (phim == null)
            {
                return NotFound();
            }

            return Ok(phim);
        }

        // PUT: api/Phim/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutPhim(string id, Phim phim)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != phim.MaPhim)
            {
                return BadRequest();
            }

            db.Entry(phim).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhimExists(id))
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

        // POST: api/Phim
        [ResponseType(typeof(Phim))]
        [HttpPost]
        public async Task<IHttpActionResult> PostPhim(Phim phim)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Phims.Add(phim);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PhimExists(phim.MaPhim))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = phim.MaPhim }, phim);
        }

        // DELETE: api/Phim/5
        [HttpDelete]
        [ResponseType(typeof(Phim))]
        public async Task<IHttpActionResult> DeletePhim(string id)
        {
            Phim phim = await db.Phims.FindAsync(id);
            if (phim == null)
            {
                return NotFound();
            }

            db.Phims.Remove(phim);
            await db.SaveChangesAsync();

            return Ok(phim);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhimExists(string id)
        {
            return db.Phims.Count(e => e.MaPhim == id) > 0;
        }
    }
}