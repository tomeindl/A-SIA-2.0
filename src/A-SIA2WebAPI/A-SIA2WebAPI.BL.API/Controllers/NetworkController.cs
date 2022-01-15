using Microsoft.AspNetCore.Mvc;
using A_SIA2WebAPI.Models;
using System;

namespace A_SIA2WebAPI.BL.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NetworkController : ControllerBase
    {
        [HttpPost]
        public void CreateNetwork([FromBody] Network network)
        {

        }

        [HttpPut("{networkId}")]
        public void UpdateNetwork([FromRoute] Guid networkId, [FromBody] Network network)
        {

        }

        [HttpDelete("{networkId}")]
        public void DeleteNetwork([FromRoute] Guid networkId)
        {

        }
    }
}
