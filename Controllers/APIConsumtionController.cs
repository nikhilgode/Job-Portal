using JobPortal_New.Data;
using JobPortal_New.Dto;
using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Modeles.Entites;
using JobPortal_New.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal_New.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIConsumtionController : ControllerBase
    {

        private readonly IUserRepository _iUser;
        private readonly ITokenRepository _iToken;
        private readonly MyDbContext _context;
        private readonly IApiConsumptionRepository _apiConsumptionRepository;
        public APIConsumtionController(IUserRepository iUser, ITokenRepository iToken, MyDbContext context, IApiConsumptionRepository apiconsumtionservice)
        {
            _iUser = iUser;
            _iToken = iToken;
            _context = context;
            _apiConsumptionRepository = apiconsumtionservice;
        }

       
        [HttpGet("apiconsumtion")]
        public async Task<List<User>> apiconsumtion()
        {
            string methodName = "getCandidates";
            var api = _context.aPIOptimizes.Where(a => a.ApiName == methodName).FirstOrDefault();

            bool flag = api.IsOptimised;

            List<User> result = new List<User>();
            if (flag)
            {
                result = await _apiConsumptionRepository.getCandidatesByApi();
            }
            else
            {
                result = await _apiConsumptionRepository.getCandidatesByMvc();
            }

           return result;
        }
    }
}
