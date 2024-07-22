
using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace JobPortal_New.Services
{
    public class ApiConsumeService : IApiConsumptionRepository
    {
        private readonly HttpClient _httpClient;
      //  private readonly IApiConsumptionRepository _apiConsumptionRepository;

        public ApiConsumeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
         //   _apiConsumptionRepository = apiConsumptionRepository;
        }

        public async Task<List<User>> getCandidatesByApi()
        {
            var response = await _httpClient.GetAsync($"api/User/getAllCandidates");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<User>>();
            return result;
        }

        public async Task<List<User>> getCandidatesByMvc()
        {
            var response = await _httpClient.GetAsync($"api/User/getallUserView");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<User>>();
            return result;
            // return View(response);
        }

    }
}
