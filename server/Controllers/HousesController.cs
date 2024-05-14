using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http.HttpResults;

namespace csharp_gregslist_api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class HousesController : ControllerBase
{
    private readonly HousesService _housesService;
    private readonly Auth0Provider _auth0Provider;

    public HousesController(HousesService housesService, Auth0Provider auth0Provider)
    {
        _housesService = housesService;
        _auth0Provider = auth0Provider;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<House>> CreateHouse([FromBody] House houseData)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);

            houseData.CreatorId = userInfo.Id;

            House house = _housesService.CreateHouse(houseData);
            return Ok(house);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet]

    public ActionResult<List<House>> GetHouses()
    {
        try
        {
            List<House> houses = _housesService.GetHouses();
            return Ok(houses);
        }
        catch (Exception exception)
        {

            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{houseId}")]

    public ActionResult<House> GetHouseById(int houseId)
    {
        try
        {
            House house = _housesService.GetHouseById(houseId);
            return Ok(house);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("{houseId}")]
    [Authorize]
    public async Task<ActionResult<House>> UpdateHouse(int houseId, [FromBody] House houseData)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            House house = _housesService.UpdateHouse(houseId, userInfo.Id, houseData);
            return Ok(house);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("{houseId}")]
    [Authorize]

    public async Task<ActionResult<string>> TrashHouse(int houseId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            string message = _housesService.TrashHouse(houseId, userInfo.Id);
            return Ok(message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}