using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using FlutterCinemaAPI.Models;
using Microsoft.IdentityModel.Tokens;
using MailKit.Net.Smtp;
using MimeKit;
using FlutterCinemaAPI.Models.Utils;

namespace FlutterCinemaAPI.Controllers.WebAPI
{
    public class KhachHangController : ApiController
    {
        private cinemaAPIEntities db = new cinemaAPIEntities();

        // POST: api/KhachHang/login
        [HttpPost]
        [Route("api/KhachHang/login")]
        public async Task<IHttpActionResult> Login(string email, string password)
        {
            var khachHang = await db.KhachHangs.FirstOrDefaultAsync(k => k.Email == email && k.MatKhau == password);

            if (khachHang == null)
            {
                return BadRequest("Sai email hoặc mật khẩu");
            }

            // Tạo một đối tượng ViewModel để trả về thông tin khách hàng
            var viewModel = new
            {
                status = "success",
                MaKH = khachHang.MaKH,
                HoTen = khachHang.HoTen,
                Email = khachHang.Email,
                MatKhau = khachHang.MatKhau,
                SDT = khachHang.STD,
                AnhDaiDien = khachHang.AnhDaiDien
            };

            return Ok(viewModel);
        }

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
        public async Task<IHttpActionResult> Get(int id)
        {
            KhachHang khachHang = await db.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return Ok(khachHang);
        }

        [HttpGet]
        [Route("api/KhachHang/GetPassword")]
        public async Task<IHttpActionResult> GetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email không được để trống");
            }

            var khachHang = await db.KhachHangs.FirstOrDefaultAsync(k => k.Email == email);

            if (khachHang == null)
            {
                return NotFound();
            }

            return Ok(new { MatKhau = khachHang.MatKhau });
        }

        // PUT: api/KhachHang/5
        [ResponseType(typeof(KhachHang))]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, KhachHang khachHang)
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

        //Gửi OTP đến email khách hàng
        // POST: api/KhachHang/postOTP
        [HttpPost]
        [Route("api/KhachHang/postOTP")]
        public IHttpActionResult PostOTP(string email)
        {
            var khachHang = db.KhachHangs.FirstOrDefault(k => k.Email == email);
            if (khachHang == null)
            {
                return BadRequest("Email không tồn tại");
            }

            string otp = GenerateOTP();
            Session.Add("OTP", otp, TimeSpan.FromMinutes(5));

            string emailBody = $"Mã OTP của bạn là: {otp}";

            bool isEmailSent = GoogleSMTP.SendEmail(email, emailBody);
            if (isEmailSent)
            {
                return Ok(new { status = "success", message = "OTP đã được gửi đến email của bạn", OTP = otp });
            }
            else
            {
                return BadRequest("Gửi email thất bại");
            }
        }

        // GET: api/KhachHang/getOTP
        [HttpGet]
        [Route("api/KhachHang/getOTP")]
        public IHttpActionResult GetOTP()
        {
            var otp = Session.Get("OTP");
            if (otp == null)
            {
                return NotFound();
            }

            return Ok(new { OTP = otp.ToString() });
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        // POST: api/KhachHang
        [ResponseType(typeof(KhachHang))]
        [HttpPost]
        public async Task<IHttpActionResult> Post(KhachHang khachHang)
        {
            // Kiểm tra xem email đã có tài khoản nào đăng ký chưa
            var existingKhachHang = await db.KhachHangs.FirstOrDefaultAsync(k => k.Email == khachHang.Email);
            if (existingKhachHang != null)
            {
                ModelState.AddModelError("Email", "Email đã được đăng ký");
                return BadRequest(ModelState);
            }

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
        public async Task<IHttpActionResult> Delete(int id)
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

        private bool KhachHangExists(int id)
        {
            return db.KhachHangs.Count(e => e.MaKH == id) > 0;
        }
    }
}