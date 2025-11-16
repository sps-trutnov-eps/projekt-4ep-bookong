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
            // 1. Find the material
            var material = await _unitOfWork.TeachingMaterials.GetByIdAsync(id);
            
            if (material == null)
            {
                return GenericResponse.FailureResponse("The teaching material was not found.");
            }

            // 2. Delete the material
            _unitOfWork.TeachingMaterials.Delete(material);
            await _unitOfWork.CommitAsync();

            // 3. Return success
            return GenericResponse.SuccessResponse("The teaching material has been successfully deleted.");
        }
    }
}
