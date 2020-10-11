using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Korobochka.DTOs;
using Korobochka.Models;
using Korobochka.Services;

namespace Korobochka.Controllers
{
    public class PlacesController : BaseController<PlaceDTO>
    {
        private PlacesCRUDService _crudService;
        private readonly ILogger<PlacesController> _logger;

        public PlacesController(
            PlacesCRUDService crudService,
            ILogger<PlacesController> logger)
        {
            _crudService = crudService;
            _logger = logger;
        }

        public override ActionResult<IEnumerable<PlaceDTO>> Get()
        {
            try
            {
                return Ok(_crudService.Get().Select(item => (PlaceDTO)item));
            }
            catch (Exception e)
            {
                return CustomBadRequest(e.Message);
            }
        }

        public override ActionResult<PlaceDTO?> Get(int id)
        {
            try
            {
                var result = _crudService.Get(id);

                if (result != null) return Ok((PlaceDTO)result);
                return Ok(null);
            }
            catch (Exception e)
            {
                return CustomBadRequest(e.Message);
            }
        }

        public override ActionResult<PlaceDTO> Post([FromBody] PlaceDTO value)
        {
            try
            {
                // TODO if wrong data
                return Ok((PlaceDTO)(_crudService.Create((Place)value))); //spreadsheetinfo - range

            }
            catch (Exception e)
            {
                return CustomBadRequest(e.Message);
            }
        }

        public override ActionResult<PlaceDTO> Put(int id, [FromBody] PlaceDTO value)
        {
            try
            {
                // TODO if wrong data?
                var result = _crudService.Update(id, (Place)value);
                return Ok((PlaceDTO)result);
            }
            catch (Exception e)
            {
                return CustomBadRequest(e.Message);
            }
        }

        public override ActionResult<PlaceDTO> Delete(int id)
        {
            try
            {
                // TODO if wrong id
                // TODO check dpendencies, dont remove if stuff here
                _crudService.Remove(id);
                return Ok();
            }
            catch (Exception e) //TODO custom exceptions
            {
                return CustomBadRequest(e.Message);
            }
        }
    }
}
