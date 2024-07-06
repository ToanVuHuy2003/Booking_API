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
    public class LoaiVeController : ApiController
    {
        private cinemaEntities db = new cinemaEntities();

        // GET: api/LoaiVe
        [HttpGet]
        public IQueryable<LoaiVe> Get()
        {
            return db.LoaiVes;
        }

        // GET: api/LoaiVe/5
        [ResponseType(typeof(LoaiVe))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string id)
        {
            LoaiVe loaiVe = await db.LoaiVes.FindAsync(id);
            if (loaiVe == null)
            {
                return NotFound();
            }

            return Ok(loaiVe);
        }

        // PUT: api/LoaiVe/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutLoaiVe(string id, LoaiVe loaiVe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != loaiVe.MaVe)
            {
                return BadRequest();
            }

            db.Entry(loaiVe).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiVeExists(id))
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

        // POST: api/LoaiVe
        [ResponseType(typeof(LoaiVe))]
        [HttpPost]
        public async Task<IHttpActionResult> PostLoaiVe(LoaiVe loaiVe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LoaiVes.Add(loaiVe);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LoaiVeExists(loaiVe.MaVe))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = loaiVe.MaVe }, loaiVe);
        }

        // DELETE: api/LoaiVe/5
        [ResponseType(typeof(LoaiVe))]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteLoaiVe(string id)
        {
            LoaiVe loaiVe = await db.LoaiVes.FindAsync(id);
            if (loaiVe == null)
            {
                return NotFound();
            }

            db.LoaiVes.Remove(loaiVe);
            await db.SaveChangesAsync();

            return Ok(loaiVe);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LoaiVeExists(string id)
        {
            return db.LoaiVes.Count(e => e.MaVe == id) > 0;
        }
    }
}