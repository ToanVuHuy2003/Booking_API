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
    public class KhachHangController : ApiController
    {
        private cinemaAPIEntities db = new cinemaAPIEntities();

        // GET: api/KhachHang
        [HttpGet]
        public async Task<List<KhachHang>> Get()
        {
            List<KhachHang> khachHang = await db.KhachHangs.ToListAsync();
            return khachHang;
        }

        // GET: api/KhachHang/5
        [ResponseType(typeof(KhachHang))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string id)
        {
            KhachHang khachHang = await db.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return Ok(khachHang);
        }

        // PUT: api/KhachHang/5
        [ResponseType(typeof(KhachHang))]
        [HttpPut]
        public async Task<IHttpActionResult> Put(string id, KhachHang khachHang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != khachHang.MaKH)
            {
                return BadRequest();
            }

            db.Entry(khachHang).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KhachHangExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(khachHang);
        }

        // POST: api/KhachHang
        [ResponseType(typeof(KhachHang))]
        [HttpPost]
        public async Task<IHttpActionResult> Post(KhachHang khachHang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.KhachHangs.Add(khachHang);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (KhachHangExists(khachHang.MaKH))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = khachHang.MaKH }, khachHang);
        }

        // DELETE: api/KhachHang/5
        [ResponseType(typeof(KhachHang))]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string id)
        {
            KhachHang khachHang = await db.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }

            db.KhachHangs.Remove(khachHang);
            await db.SaveChangesAsync();

            return Ok(khachHang);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KhachHangExists(string id)
        {
            return db.KhachHangs.Count(e => e.MaKH == id) > 0;
        }
    }
}