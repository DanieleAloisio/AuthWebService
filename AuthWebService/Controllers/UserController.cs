using AuthWebService.Dto;
using AuthWebService.Dto.ErrorDto;
using AuthWebService.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService _userRepository;
        private readonly IMapper mapper;

        public UserController(IUserService userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            this.mapper = mapper;
        }

        [HttpPost("auth")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<JwtTokenDto>))]
        [ProducesResponseType(400, Type = typeof(ErrMsg))]
        public async Task<ActionResult<JwtTokenDto>> Authenticate([FromBody] AuthDto userParam)
        {
            string tokenJWT = "";

            bool IsOk = await _userRepository.Authenticate(userParam.UserId, userParam.Password);

            if (!IsOk)
            {
                return BadRequest(new ErrMsg(string.Format($"User Id e/o Passoword non corrette!"),
                    this.HttpContext.Response.StatusCode));
            }
            else
            {
                tokenJWT = await _userRepository.GetToken(userParam.UserId);
            }

            return Ok(new JwtTokenDto(tokenJWT));
        }
    }
}
