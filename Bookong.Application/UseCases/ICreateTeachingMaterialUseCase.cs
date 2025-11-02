using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases
{
    internal class ICreateTeachingMaterialUseCase : Interfaces.ICreateTeachingMaterialUseCase
    {
        private readonly ITeachingMaterialRepository _teachingMaterialRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ICreateTeachingMaterialUseCase(
            ITeachingMaterialRepository teachingMaterialRepository,
            IUnitOfWork unitOfWork)
        {
            _teachingMaterialRepository = teachingMaterialRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse> ExecuteAsync(CreateTeachingMaterialDto dto)
        {
            //  1. Validace vstupu
            if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Url))
            {
                return GenericResponse.FailureResponse(
                    "Musíte zadat titulek i URL výukového materiálu."
                );
            }

            //  2. Vytvoření entity
            var newMaterial = new TeachingMaterial
            {
                Title = dto.Title.Trim(),       
                Url = dto.Url.Trim()
            };

            //  3. Uložení do databáze
            _teachingMaterialRepository.Add(newMaterial);
            await _unitOfWork.CommitAsync();

            //  4. Vrácení odpovědi
            return GenericResponse.SuccessResponse(
                "Výukový materiál byl úspěšně vytvořen.",
                new
                {
                    newMaterial.Id,
                    newMaterial.PublicId,
                    newMaterial.Title,
                    newMaterial.Url
                }
            );
        }
    }
}
