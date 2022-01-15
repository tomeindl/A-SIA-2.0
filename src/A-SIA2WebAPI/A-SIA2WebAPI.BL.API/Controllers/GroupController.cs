using Microsoft.AspNetCore.Mvc;
using A_SIA2WebAPI.Models;

namespace A_SIA2WebAPI.BL.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        [HttpPost("{networkId}")]
        public void CreatePerson([FromRoute] long networkId, [FromBody] Group group)
        {

        }

        [HttpDelete("{groupId}")]
        public void DetachPersonFromGroup([FromRoute] long groupId)
        {

        }

        [HttpPut("{groupId}")]
        public void ChangeGroupOfPerson([FromRoute] long groupId, [FromBody] Group group)
        {

        }
    }
}
