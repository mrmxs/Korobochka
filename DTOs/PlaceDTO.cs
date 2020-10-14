using System.Collections.Generic;
using Korobochka.Models;

namespace Korobochka.DTOs
{
    public class PlaceDTO : BaseDTO
    {    
        public string Name { get; set; }
        public IEnumerable<int> Owner { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Order { get; set; }

        public static explicit operator PlaceDTO(Place model)
        {
            return new PlaceDTO
            {
                Id = model.Id,
                Name = model.Name,
                Owner = model.Owner,
                Address = model.Address,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Order = model.Order,
                GSheetRange = model.GSheetRange,
            };
        }
        public static explicit operator Place(PlaceDTO model)
        {
            return new Place
            {
                Name = model.Name,
                Owner = model.Owner,
                Address = model.Address,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Order = model.Order,
                GSheetRange = model.GSheetRange,
            };
        }
    }
}
