using JobPortal_New.Data;
using JobPortal_New.Dto;
using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal_New.Services
{
    public class OtpService : IOtpRepository
    {

        private readonly MyDbContext _context;

        public OtpService(MyDbContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> AddOtp(OtpDto model)
        {
            var otpToDelete = _context.Otps.Where( o => o.email == model.email).FirstOrDefault();

            if (otpToDelete != null)
            {
                _context.Otps.Remove(otpToDelete);
                await _context.SaveChangesAsync();
            }


            var otp = new Otp();
            otp.email = model.email;
            otp.otp = model.otp;
            otp.otpSentTime = DateTime.Now;
            await _context.Otps.AddAsync(otp);
            await _context.SaveChangesAsync();
            return new OkObjectResult(new { message = "Otp added Successfully" });
        }

        public async Task<IActionResult> DeleteOtp(OtpDto model)
        {
            var otp = _context.Otps.Where(o => o.email == model.email).FirstOrDefault();
            _context.Otps.Remove(otp);
            _context.SaveChanges();
            return new OkObjectResult(new { message = "OTP Deleted Successfully" });
        }
    }
}
