using JobPortal_New.Data;
using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using JobPortal_New.Dto;
using JobPortal_New.HelperMethods;
using System.Security.Authentication;
using JobPortal_New.Services;
using JobPortal_New.HelperMethods;
using Microsoft.AspNetCore.Builder;
using System;
namespace JobPortal_New.Repositories
{
    public class UserService : IUserRepository
    {
        private readonly MyDbContext _context;
        private readonly  ITokenRepository _tokenRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IOtpRepository _otpRepository ;
        public UserService(MyDbContext context, ITokenRepository tokenRepository,IEmailRepository emailRepository,IOtpRepository otpRepository)
        {
            _context = context;
            _tokenRepository = tokenRepository;
            _emailRepository = emailRepository;
            _otpRepository = otpRepository;
        }

        
        public async Task<IActionResult> AddUser(UserDto model)
        {
            try
            {
                var user = new User();
                if(IsPasswordValid(model.Password))
                {
                    string hashedPassword = PasswordHasher.HashPassword(model.Password);
                    user.Password =  hashedPassword;
                }
                else 
                {
                    return new ObjectResult(new { message = "Password is in incorrect format" });
                }
                user.UserName = model.Name; 
                user.UserEmail = model.Email;
                if (model.role == 2 || model.role == 3)
                {
                    var roleEntity = _context.Roles.FirstOrDefault(r => r.RoleId == model.role);
                    user.roles = roleEntity;
                }
                else
                {
                    return new ObjectResult(new { message = "Invalid User Role" });
                }
                user.CreatedDate = model.CreatedDate;
                user.IsActive = true;
                user.Gender = model.Gender;
                user.ContactNumber = model.ContactNumber;
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return new OkObjectResult(new { message = "User Created Successfully" });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred while updating the recruiter", error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }

        public async Task<IActionResult> DeleteUser(int userId)
        {


            try
            {
                var user = _context.Users.Find(userId);
                if (user != null)
                {
                    var userApplications = _context.Applicationes.Where(ja => ja.UserId == userId).ToList();
                    _context.Applicationes.RemoveRange(userApplications);
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    return new OkObjectResult(new { message = "User Deleted Successfully" });
                }
                else
                {
                    return new ObjectResult(new { message = "User not found" });
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred while updating the recruiter", error = ex.Message })
                {
                    StatusCode = 500
                };
            }


        }


        public async Task<IActionResult> UpdateUser(int userId, UserDto updatedUser)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user != null)
                {
                    // Update user properties
                    user.UserName = updatedUser.Name;
                    user.UserEmail = updatedUser.Email;
                    user.Password = user.Password;
                    user.CreatedDate = updatedUser.CreatedDate ?? user.CreatedDate;
                    user.IsActive = true;
                    user.Gender = updatedUser.Gender;
                    var role = _context.Roles.Find(userId);
                    user.roles = role;

                    // Save changes to the database
                    _context.SaveChanges();
                    return new OkObjectResult(new { message = "User Updated Successfully" });
                }
                else
                {
                    return new ObjectResult(new { message = "User not found" });
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred while updating the recruiter", error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }



        public async Task<IActionResult> AddAdmin(UserDto model)
        {
            try
            {
                var user = new User();
                if (IsPasswordValid(model.Password))
                {
                    string hashedPassword = PasswordHasher.HashPassword(model.Password);
                    user.Password = hashedPassword;
                }
                else
                {
                    return new ObjectResult(new { message = "Password is in incorrect format" });
                }
                user.UserName = model.Name;
                user.UserEmail = model.Email;
                user.roles = _context.Roles.FirstOrDefault(r => r.RoleId == model.role);
                user.CreatedDate = model.CreatedDate;
                user.IsActive = true;
                user.Gender = model.Gender;
                user.ContactNumber = model.ContactNumber;
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return new OkObjectResult(new { message = "Admin Created Successfully" });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred while updating the recruiter", error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }


        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, int userId)
        {
            try 
            {
                var user = _context.Users.Find(userId);
                if (user != null)
                {

                    string hashedPassword = PasswordHasher.HashPassword(oldPassword);

                    if (hashedPassword == user.Password)
                    {
                        if (IsPasswordValid(newPassword))
                        {
                            string hashedPassword1 = PasswordHasher.HashPassword(newPassword);
                            user.Password = hashedPassword1;
                        }
                        else
                        {
                            return new ObjectResult(new { message = "Password is in incorrect format" });
                        }
                    }
                    else 
                    {
                        return new ObjectResult(new { message = "Your old password is wrong" });
                    }

                    // Save changes to the database
                    _context.SaveChanges();
                    return new OkObjectResult(new { message = "User Updated Successfully" });
                }
                else
                {
                    return new ObjectResult(new { message = "User not found" });
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred while updating the user", error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }


        public async Task<IActionResult> Login(UserLoginDto user)
        {
            try
            {
                var user1 = await AuthenticateUserAsync(user.UserEmail,user.Password);

                if (user != null)
                {
                    var tokenString = _tokenRepository.GenerateToken(user1);
                    return new OkObjectResult(new { token = tokenString });
                }

                return new UnauthorizedObjectResult(new { message = "Unauthorized" });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred while logging in", error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }



         private Task<User> AuthenticateUserAsync(string email, string password)
            {

                 string hashedPassword = PasswordHasher.HashPassword(password);
                 var user = _context.Users.Include(r => r.roles).FirstOrDefault(u => email == u.UserEmail   &&  hashedPassword==u.Password );

                if (user == null)
                {
                throw new AuthenticationException("Invalid username or password");
            }
                else
                {
                   return Task.FromResult(user);
                }
            }


        public bool IsPasswordValid(string? password)
        {

            if (password == null)
            {
                return false;
            }

            string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$";

            return Regex.IsMatch(password, pattern);
        }


        public async Task<String> ExportAllCandidates()
        {
            var candidates = await _context.Users
                                           .Where(u => u.roles.RoleId == 3)
                                           .Select(u => new UserDto
                                           {   
                                              Name = u.UserName,
                                              Gender = u.Gender,
                                              Email = u.UserEmail,

                                           })
                                           .ToListAsync();

            string filePath = ExcelFiles.ExportToExcel(candidates,"Candidates");
            return filePath;
        }


        public async Task<String> ExportAllRecruiters()
        {
            var candidates = await _context.Users
                                           .Where(u => u.roles.RoleId == 2)
                                           .Select(u => new UserDto
                                           {
                                               Name = u.UserName,
                                               Gender = u.Gender,
                                               Email = u.UserEmail,

                                           })
                                           .ToListAsync();

            string filePath = ExcelFiles.ExportToExcel(candidates, "Recruiters");
            return filePath;
        }

        public async Task <List<User>> getAllCandidates()
        {
            var user = _context.Users.Include(r => r.roles).ToListAsync();
            return await user;
        }


        public async Task<List<UserDto>> getActivatedUsers()


        {
            var activeUsers = _context.Users
                                      .Where(u => u.IsActive==true)
                                      .Select(u => new UserDto
                                      {
                                          Name = u.UserName,
                                          Gender = u.Gender,
                                          Email = u.UserEmail,

                                      })
                                      .ToListAsync();     
            return await activeUsers;
        }


        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var user = _context.Users
                                   .Where(u => u.UserEmail == email)
                                   .FirstOrDefault();

                if (user == null)
                {
                    return new ObjectResult(new { message = "User with this email does not exist." });
                }

                var otp = GenerateOTP.GenerateOtp();
                var userEmail = user.UserEmail; // Assuming you have an Email property in the User entity
                var subject = "OTP ";
                var message = $"Dear {user.UserName},\n\nThis is your one time password is {otp}.\n\nBest regards,\nJob Portal  Team";
                await _emailRepository.SendEmailAsync(userEmail, subject, message);

                var otpModel = new OtpDto();
                otpModel.email = email;
                otpModel.otp = otp;

                var isExistinfOtp = _context.Otps.Where(o => o.email == email).FirstOrDefault();

                if(isExistinfOtp != null)
                {
                    _context.Otps.Remove(isExistinfOtp);
                }

                await _otpRepository.AddOtp(otpModel);
                return new OkObjectResult(new { message = "OTP Sent Successfully" });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred while  Sending OTP", error = ex.Message })
                {
                    StatusCode = 500
                };
            }
            
        }


        public async Task<IActionResult> ResetPassword(OtpDto model,string newPassword)
        {
            var otpModel = _context.Otps.FirstOrDefault(u => u.email == model.email);

            if (otpModel.otp != model.otp)
            {
                return new ObjectResult(new { message = "OTP NOT MATCHING" });
            }
            else
            {

                DateTime datetimeNow = DateTime.Now;
                DateTime registerDateTime = otpModel.otpSentTime;

                TimeSpan difference = datetimeNow - registerDateTime;
                double minutes = difference.TotalMinutes;
                if (minutes <= 7)
                {
                    if (IsPasswordValid(newPassword))
                    {
                        var user = _context.Users.Where(u => u.UserEmail == model.email).FirstOrDefault();
                        string hashedPassword = PasswordHasher.HashPassword(newPassword);
                        user.Password = hashedPassword;
                        _context.SaveChangesAsync();


                        return new OkObjectResult(new { message = "Password Recovered successfully..!" });
                    }
                    else
                    {
                        return new ObjectResult(new { message = "New Password is not valid" });
                    }
                }
                else
                {
                    _context.Otps.Remove(otpModel);
                    _context.SaveChangesAsync();
                    return new ObjectResult(new { message = "Time Limit for this OTP is expired please generate New OTP" });
                }
            }
        }


        public async Task<IActionResult> ActivateUser(int userId)
        {
            try
            {
                var user = _context.Users.Where(u => u.UserId == userId).FirstOrDefault();

                if(user == null)
                {
                    return new ObjectResult(new { message = "No such user...!" });
                }

                 user.IsActive = true;
                _context.Update(user);
               _context.SaveChanges();

                return new OkObjectResult(new { message = "User activated sucessfully...!" });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred while  Sending OTP", error = ex.Message })
                {
                    StatusCode = 500
                };
            }

        }

        public async Task<IActionResult> DeactivateUser(int userId)
        {
            try
            {
                var user = _context.Users.Where(u => u.UserId == userId).FirstOrDefault();

                if (user == null)
                {
                    return new ObjectResult(new { message = "No such user...!" });
                }

                user.IsActive = false;
                _context.Update(user);
                _context.SaveChanges();

                return new OkObjectResult(new { message = "User activated sucessfully...!" });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred while  Sending OTP", error = ex.Message })
                {
                    StatusCode = 500
                };
            }

        }

    }
}
