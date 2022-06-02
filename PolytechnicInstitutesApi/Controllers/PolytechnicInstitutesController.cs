using Microsoft.AspNetCore.Mvc;

namespace PolytechnicInstitutesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolytechnicInstitutesController : ControllerBase
    {
        IEnumerable<PolytechnicInstitutesDto> institutes = new List<PolytechnicInstitutesDto>();

        private async Task<IEnumerable<PolytechnicInstitutesDto>> GetDataFromApiAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://universities.hipolabs.com/");
                var result = await client.GetAsync("search?country=portugal");
                if (result.IsSuccessStatusCode)
                {
                    institutes = await result.Content.ReadAsAsync<IEnumerable<PolytechnicInstitutesDto>>();
                }
                else
                {
                    institutes = Enumerable.Empty<PolytechnicInstitutesDto>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return institutes;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PolytechnicInstitutes>>> MappingDataFromApi()
        {
            await GetDataFromApiAsync();
            var institutesResult = institutes.Select(item => new PolytechnicInstitutes
            {
                Name = item.Name,
                Webpage = item.Web_pages[0]
            }).ToList();
            return Ok(institutesResult);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> NumberOfELements()
        {
            await GetDataFromApiAsync();
            var numberOfElements = institutes.Select(item => new PolytechnicInstitutes
            {
                Name = item.Name,
                Webpage = item.Web_pages[0]
            }).Count();
            return Ok(numberOfElements);
        }

        [HttpGet("ascendingOrder")]
        public async Task<ActionResult<IEnumerable<PolytechnicInstitutes>>> AscendingOrder()
        {
            await GetDataFromApiAsync();
            var institutesResult = institutes.Select(item => new PolytechnicInstitutes
            {
                Name = item.Name,
                Webpage = item.Web_pages[0]
            }).OrderBy(i => i.Name).ToList();
            return Ok(institutesResult);
        }

        [HttpGet("descendingOrder")]
        public async Task<ActionResult<IEnumerable<PolytechnicInstitutes>>> DescendingOrder()
        {
            await GetDataFromApiAsync();
            var institutesResult = institutes.Select(item => new PolytechnicInstitutes
            {
                Name = item.Name,
                Webpage = item.Web_pages[0]
            }).OrderByDescending(i => i.Name).ToList();
            return Ok(institutesResult);
        }
    }
}