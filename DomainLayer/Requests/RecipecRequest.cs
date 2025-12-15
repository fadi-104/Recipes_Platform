using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;


namespace DomainLayer.Requests
{
    public class RecipecRequest
    {
        public int? Id { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int ChefId { get; set; }
        [Required]
        [MaxLength(60)]
        public string Description { get; set; }
        [Required]
        public List<string> Ingredients { get; set; }
        [Required]
        public string Steps { get; set; }
        [Required]
        public int CookTime { get; set; }
        [Required]
        public IFormFile BaseImage { get; set; }
        [Required]
        public bool IsPublished { get; set; }
    }

    public class RecipesValidator : AbstractValidator<RecipecRequest>
    {
        public RecipesValidator(IStringLocalizer<RecipecRequest> localizer)
        {
            RuleFor(x => x.BaseImage).Must((model, file) =>
            {
                if (file == null)
                    return false;

                var size = file.Length / 1024;
                if (size > 500)
                    return false;

                return true;
            }).WithMessage("Image size must be less than 500KB");

        }
    }
}
