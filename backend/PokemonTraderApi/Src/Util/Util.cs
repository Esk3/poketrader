namespace PokemonTraderApi.Util;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
public class MyControllerBase : ControllerBase { }
