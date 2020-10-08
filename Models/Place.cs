using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Korobochka.Models
{
    public class Place : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public IEnumerable<int> Owner { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Order { get; set; }

#nullable enable
        public override bool MemberwiseEquals(object? obj)
        {

            if (null == obj || obj.GetType() != this.GetType()) return false;

            var thisTypeObj = obj as Place;
            if (thisTypeObj.Name != this.Name) return false;
            if (thisTypeObj.Owner != this.Owner) return false;
            if (thisTypeObj.Address != this.Address) return false;
            if (thisTypeObj.Latitude != this.Latitude) return false;
            if (thisTypeObj.Longitude != this.Longitude) return false;
            if (thisTypeObj.Order != this.Order) return false;

            return base.MemberwiseEquals(obj);
        }
#nullable disable

        public override T Merge<T>(T itemIn)
        {
            var newItem = itemIn as Place;
            var edited = (Place)this.MemberwiseClone();

            edited.Name = newItem.Name ?? this.Name;
            edited.Owner = newItem.Owner ?? this.Owner;
            edited.Address = newItem.Address ?? this.Address;
            edited.Latitude = newItem.Latitude ?? this.Latitude;
            edited.Longitude = newItem.Longitude ?? this.Longitude;
            edited.Order = newItem.Order ?? this.Order;

            return edited as T;
        }

        public override T FromValues<T>(IList<string> values)
        {
            try
            {
                this.Name = values[1];
                this.Owner = values[2]
                    .Split(",").Select(r => int.Parse(r));//.ToArray();
                this.Address = values[3];
                this.Latitude = "" == values[4] ? null :                
                    Double.Parse(
                        values[4].Replace(',', '.'),                        
                        CultureInfo.InvariantCulture);
                this.Longitude = "" == values[5] ? null : 
                    Double.Parse(
                        values[5].Replace(',', '.'),
                        CultureInfo.InvariantCulture);
                this.Order = int.Parse(values[6]);

                return base.FromValues<T>(values);
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}