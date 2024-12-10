using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Ecommerce_Shop_NDNB.Repository.Validation
{
	public class FileExtensionAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is IFormFile file)
			{
				var extension = Path.GetExtension(file.FileName);
				string[] allowedExtensions = { "jpg", "png", "jpeg" };
				bool result = allowedExtensions.Any(x => extension.EndsWith(x));

                if (!result)
                {
                    return new ValidationResult("Chỉ cho phép file có định dạng .jpg, .jpeg hoặc .png.");
                }
            }
			return ValidationResult.Success;
		}
	}
}
