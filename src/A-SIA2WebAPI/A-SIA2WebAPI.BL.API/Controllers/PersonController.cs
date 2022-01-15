using Microsoft.AspNetCore.Mvc;
using A_SIA2WebAPI.Models;

namespace A_SIA2WebAPI.BL.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpPost("{networkId}")]
        public void CreatePerson([FromRoute] long networkId, [FromBody] Person person)
        {

        }

        [HttpPut("{personId}")]
        public void UpdateNode([FromRoute] long personId, [FromBody] Person person)
        {

        }

        [HttpDelete("{personId}")]
        public void DeleteNode([FromRoute] long personId)
        {

        }

        // -------------------------
        //          GROUPS
        // -------------------------

        [HttpPost("{personId}/Group/{groupId}")]
        public void SetGroup([FromRoute] long personId, [FromRoute] long groupId)
        {

        }

        [HttpDelete("{personId}/Group/{groupId}")]
        public void DetachPersonFromGroup([FromRoute] long personId, [FromRoute] long groupId)
        {

        }

        [HttpPatch("{personId}/Group/{groupId}")]
        public void ChangeGroupOfPerson([FromRoute] long personId, [FromRoute] long groupId)
        {

        }
    }
}
