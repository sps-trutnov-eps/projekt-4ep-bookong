using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases
{
    internal class TeachingMaterialService : ICreateTeachingMaterialUseCase
    {
        private ITeachingMaterialRepository repository;
        private IUnitOfWork unitOfWork;

        public TeachingMaterialService(ITeachingMaterialRepository r, IUnitOfWork uow)
        {
            repository = r;
            unitOfWork = uow;
        }

        public async Task<GenericResponse> ExecuteAsync(CreateTeachingMaterialDto input)
        {
            // Validace vstupů pomocí pattern matching
            if (input is null || string.IsNullOrWhiteSpace(input.Title) || string.IsNullOrWhiteSpace(input.Url))
                return CreateResp(false, "Titulek i URL musí být vyplněné.");

            // Anonymous local function pro tvorbu entity
            TeachingMaterial VytvorMaterial(CreateTeachingMaterialDto d)
                => new TeachingMaterial { Title = d.Title.Trim(), Url = d.Url.Trim() };

            var material = VytvorMaterial(input);

            Action<TeachingMaterial> pridej = m => repository.Add(m);
            pridej(material);

            await unitOfWork.CommitAsync();

            // Response pomocí výrazu switch
            return material switch
            {
                { Id: var id, PublicId: var pid, Title: var t, Url: var u } =>
                    CreateResp(true, "Hotovo!",
                        new { Identifikator = id, Odkaz = u, Nazev = t, Guid = pid }),
                _ => CreateResp(false, "Neznámá chyba")
            };
        }

        // Helper
        private static GenericResponse CreateResp(bool ok, string msg, object? data = null)
            => new GenericResponse { Success = ok, Message = msg, Data = data };
    }
}
