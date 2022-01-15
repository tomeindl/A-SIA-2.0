using Microsoft.AspNetCore.Mvc;

namespace A_SIA2WebAPI.BL.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelationController : ControllerBase
    {
        [HttpPost("Person/{fromPersonId}/Influences/{toPersonId}")]
        public void AddRelation([FromRoute] long fromPersonId, [FromRoute] long toPersonId)
        {

        }

        [HttpDelete("Person/{fromPersonId}/Influences/{toPersonId}")]
        public void DeleteRelation([FromRoute] long fromPersonId, [FromRoute] long toPersonId)
        {

        }

        [HttpPut("Person/{fromPersonId}/Influences/{toPersonId}")]
        public void UpdateRelation([FromRoute] long fromPersonId, [FromRoute] long toPersonId)
        {

        }
    }
}
