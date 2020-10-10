﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using Korobochka.DTOs;
using Korobochka.Models;
using Korobochka.Services;

namespace Korobochka.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleSheetTestController : ControllerBase
    {
        private readonly ILogger<GoogleSheetTestController> _logger;
        private PlacesService _placesService;

        public GoogleSheetTestController(
            ILogger<GoogleSheetTestController> logger,
            PlacesService placesService)
        {
            _logger = logger;
            _placesService = placesService;
        }

        [HttpGet]
        public /* TODO override*/ ActionResult<IEnumerable<PlaceDTO>> Get()
        {
            try
            {
                return Ok(_placesService.Get().Select(item => (PlaceDTO)item));
            }
            catch (Exception e)
            {
                return CustomBadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public /*override*/ ActionResult<PlaceDTO?> Get(int id)
        {
            try
            {
                var result = _placesService.Get(id);

                if (result != null) return Ok((PlaceDTO)result);
                return Ok(null);
            }
            catch (Exception e)
            {
                return CustomBadRequest(e.Message);
            }
        }

        [HttpPost]
        public /*override*/ ActionResult<PlaceDTO> Post([FromBody] PlaceDTO value)
        {
            try
            {
                // TODO if wrong data
                return Ok((PlaceDTO)(_placesService.Create((Place)value))); //spreadsheetinfo - range

            }
            catch (Exception e)
            {
                return CustomBadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public /*override*/ ActionResult<PlaceDTO> Put(int id, [FromBody] PlaceDTO value)
        {
            try
            {
                // TODO if wrong data?
                var result = _placesService.Update(id, (Place)value);
                return Ok((PlaceDTO)result);
            }
            catch (Exception e)
            {
                return CustomBadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public /*override*/ ActionResult<PlaceDTO> Delete(int id)
        {
            try
            {
                // TODO if wrong id
                // TODO check dpendencies, dont remove if stuff here
                _placesService.Remove(id);
                return Ok();
            }
            catch (Exception e) //TODO custom exceptions
            {
                return CustomBadRequest(e.Message);
            }
        }

        //TODO to BaseController
        protected BadRequestObjectResult CustomBadRequest(object error)
        {
            return BadRequest(new { Error = error });
        }
    }
}
