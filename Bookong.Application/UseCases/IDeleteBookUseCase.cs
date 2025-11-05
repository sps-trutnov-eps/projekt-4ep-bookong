using Bookong.Domain.Interfaces;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases
{
    public class DeleteTeachingMaterialUseCase : IDeleteTeachingMaterialUseCase
    {
        private readonly ITeachingMaterialRepository _teachingMaterialRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTeachingMaterialUseCase(
            ITeachingMaterialRepository teachingMaterialRepository,
            IUnitOfWork unitOfWork)
        {
            _teachingMaterialRepository = teachingMaterialRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse> ExecuteAsync(int id)
        {
            var material = await _teachingMaterialRepository.GetByIdAsync(id);
            if (material == null)
            {
                return GenericResponse.FailureResponse("Výukový materiál nebyl nalezen.");
            }
            _teachingMaterialRepository.Delete(material);
            await _unitOfWork.CommitAsync();
            return GenericResponse.SuccessResponse("Výukový materiál byl úspěšně smazán.");
        }
    }
}
