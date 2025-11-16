using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases
{
    public class CreateTeachingMaterialUseCase : ICreateTeachingMaterialUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTeachingMaterialUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse> ExecuteAsync(CreateTeachingMaterialDto dto)
        {
            // 1. Validate input
            if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Url))
            {
                return GenericResponse.FailureResponse(
                    "You must provide both a title and a URL for the teaching material."
                );
            }

            // TODO: Získat aktuálního uživatele a jeho UserId
            // Zde je potřeba získat userId, např. z kontextu nebo parametru
            int userId = 0; // TODO: Zde nastavte správné UserId aktuálního uživatele

            // Nejdříve načteme uživatele z databáze
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return GenericResponse.FailureResponse("User not found.");
            }

            // 2. Create entity
            var newMaterial = new TeachingMaterial
            {
                Title = dto.Title.Trim(),
                Url = dto.Url.Trim(),
                UserId = userId,
                User = user // Použijeme načteného uživatele místo vytváření nové instance
            };

            // 3. Save to database
            _unitOfWork.TeachingMaterials.Add(newMaterial);
            await _unitOfWork.CommitAsync();

            // 4. Return response
            return GenericResponse.SuccessResponse(
                "The teaching material has been successfully created.",
                new TeachingMaterialDetailsDto
                {
                    Id = newMaterial.Id,
                    PublicId = newMaterial.PublicId,
                    Title = newMaterial.Title,
                    Url = newMaterial.Url
                }
            );
        }
    }
}
