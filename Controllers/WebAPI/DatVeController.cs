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
    public class DatVeController : ApiController
    {
        private cinemaAPIEntities db = new cinemaAPIEntities();

        // GET: api/DatVe
        [HttpGet]
        public async Task<List<DatVe>> Get()
        {
            List<DatVe> datVe = await db.DatVes.ToListAsync();
            return datVe;
        }

        // GET: api/DatVe/5
        [ResponseType(typeof(DatVe))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string id)
        {
            DatVe datVe = await db.DatVes.FindAsync(id);
            if (datVe == null)
            {
                return NotFound();
            }

            return Ok(datVe);
        }

        // PUT: api/DatVe/5
        [ResponseType(typeof(DatVe))]
        [HttpPut]
        public async Task<IHttpActionResult> Put(string id, DatVe datVe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != datVe.MaDat)
            {
                return BadRequest();
            }

            db.Entry(datVe).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatVeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(datVe);
        }

        // POST: api/DatVe
        [ResponseType(typeof(DatVe))]
        [HttpPost]
        public async Task<IHttpActionResult> Post(DatVe datVe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Đặt giá tiền mặc định là 70.000 VND
            datVe.GiaTien = 70000;

            db.DatVes.Add(datVe);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DatVeExists(datVe.MaDat))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = datVe.MaDat }, datVe);
        }

        // DELETE: api/DatVe/5
        [ResponseType(typeof(DatVe))]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string id)
        {
            DatVe datVe = await db.DatVes.FindAsync(id);
            if (datVe == null)
            {
                return NotFound();
            }

            db.DatVes.Remove(datVe);
            await db.SaveChangesAsync();

            return Ok(datVe);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DatVeExists(string id)
        {
            return db.DatVes.Count(e => e.MaDat == id) > 0;
        }
    }
}