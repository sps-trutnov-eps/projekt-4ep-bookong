using Bookong.Domain.Interfaces;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases
{
    public class DeleteTeachingMaterialUseCase : IDeleteTeachingMaterialUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTeachingMaterialUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse> ExecuteAsync(int id)
        {
            // 1. Najdi materiál
            var material = await _unitOfWork.TeachingMaterials.GetByIdAsync(id);
            
            if (material == null)
            {
                return GenericResponse.FailureResponse("Výukový materiál nebyl nalezen.");
            }

            // 2. Smaž materiál
            _unitOfWork.TeachingMaterials.Delete(material);
            await _unitOfWork.CommitAsync();

            // 3. Vrať úspěch
            return GenericResponse.SuccessResponse("Výukový materiál byl úspěšně smazán.");
        }
    }
}