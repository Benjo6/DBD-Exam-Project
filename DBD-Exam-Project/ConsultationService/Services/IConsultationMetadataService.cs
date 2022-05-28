using lib.DTO;
namespace ConsultationService.Services
{
    public interface IConsultationMetadataService
    {
        public Task<ConsultationMetadataDto> GetLatestMetadataAsync();
        public Task<ConsultationMetadataDto> AddMetadataAsync(ConsultationMetadataDto consultationMetadata);
    }
}