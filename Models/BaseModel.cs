using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Korobochka.Models
{

    public abstract class BaseModel
    {
        [Required]
        public int Id { get; set; }
        // public DateTime CreatedAt { get; set; }
        // public DateTime? EditedAt { get; set; }

#nullable enable
        public virtual bool MemberwiseEquals(object? obj)
        {
            if (null == obj || obj.GetType() != this.GetType()) return false;

            var thisTypeObj = obj as BaseModel;
            if (thisTypeObj.Id != this.Id) return false;
            // if (thisTypeObj.CreatedAt != this.CreatedAt) return false;
            // if (thisTypeObj.EditedAt != this.EditedAt) return false;

            return true;
        }
#nullable disable

        public abstract T Merge<T>(T itemIn) where T : BaseModel;
        public virtual T FromValues<T>(IList<string> values) where T : BaseModel
        {
            this.Id = int.Parse(values[0]);

            return this as T;
        }
    }
}

