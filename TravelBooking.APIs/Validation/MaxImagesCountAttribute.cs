using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBooking.APIs.DTOS.Rooms;

namespace TravelBooking.Core.Validations
{
    public class MaxImagesCountAttribute: ValidationAttribute
    {
        private readonly int _maxCount;

        public MaxImagesCountAttribute(int maxCount)
        {
            _maxCount = maxCount;
        }

        public override bool IsValid(object value)
        {
            if (value is List<RoomImageCreateDTO> images)
                return images.Count <= _maxCount;

            return false;
        }
    }
}
