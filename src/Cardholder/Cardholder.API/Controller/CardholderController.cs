using Microsoft.AspNetCore.Mvc;

namespace Cardholder.API.Controller;

[ApiController]
[Route("api/cardholder")]
public class CardholderController
{
    [HttpGet]
    public ActionResult getProfilInfo()
    {
        return null;
    }
}