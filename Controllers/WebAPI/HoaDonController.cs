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
    public class HoaDonController : ApiController
    {
        private cinemaAPIEntities db = new cinemaAPIEntities();

        // GET: api/HoaDon
        [HttpGet]
        public async Task<List<HoaDon>> Get()
        {
            List<HoaDon> hoaDon = await db.HoaDons.ToListAsync();
            return hoaDon;
        }

        // GET: api/HoaDon/GetByIDOrCustomerID
        [HttpGet]
        [Route("api/HoaDon/GetByID")]
        public async Task<IHttpActionResult> GetById(int? id = null, int? MaKH = null)
        {
            if (id.HasValue)
            {
                // Tìm kiếm hóa đơn theo ID
                HoaDon hoaDon = await db.HoaDons.FindAsync(id);
                if (hoaDon == null)
                {
                    return NotFound();
                }
                return Ok(hoaDon);
            }
            else if (MaKH.HasValue)
            {
                // Tìm kiếm hóa đơn theo mã khách hàng
                var hoaDons = await db.HoaDons.Where(h => h.MaKH == MaKH.Value).ToListAsync();
                if (hoaDons == null || !hoaDons.Any())
                {
                    return NotFound();
                }
                return Ok(hoaDons);
            }
            else
            {
                // Không có ID hoặc mã khách hàng được cung cấp
                return BadRequest("Vui lòng cung cấp ID hóa đơn hoặc mã khách hàng.");
            }
        }

        // PUT: api/HoaDon/5
        [ResponseType(typeof(HoaDon))]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, HoaDon hoaDon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hoaDon.MaHD)
            {
                return BadRequest();
            }

            db.Entry(hoaDon).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HoaDonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(hoaDon);
        }

        // POST: api/HoaDon
        [ResponseType(typeof(HoaDon))]
        [HttpPost]
        public async Task<IHttpActionResult> Post(HoaDon hoaDon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.HoaDons.Add(hoaDon);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HoaDonExists(hoaDon.MaHD))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = hoaDon.MaHD }, hoaDon);
        }

        // DELETE: api/HoaDon/5
        [ResponseType(typeof(HoaDon))]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            HoaDon hoaDon = await db.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            db.HoaDons.Remove(hoaDon);
            await db.SaveChangesAsync();

            return Ok(hoaDon);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HoaDonExists(int id)
        {
            return db.HoaDons.Count(e => e.MaHD == id) > 0;
        }
    }
}