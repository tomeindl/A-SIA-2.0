using Microsoft.AspNetCore.Mvc;
using A_SIA2WebAPI.Models;

namespace A_SIA2WebAPI.BL.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NetworkStructureController : ControllerBase
    {
        [HttpGet("{networkId}")]
        public NetworkStructure GetNetworkStructure([FromRoute] long networkId)
        {
            return null;
        }

        [HttpPut("{networkId}")]
        public void UpdateNetworkStructure([FromRoute] long networkId)
        {
        }
    }
}
