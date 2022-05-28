using lib.DTO;
namespace ConsultationService.Services
{
    public class MongoConsultationMetadataService : IConsultationMetadataService
    {
        public MongoConsultationMetadataService()
        {

        }
        public Task<ConsultationMetadataDto> GetLatestMetadataAsync()
        {
            throw new NotImplementedException();
        }
        public Task<ConsultationMetadataDto> AddMetadataAsync(ConsultationMetadataDto consultationMetadata)
        {
            throw new NotImplementedException();
        }
    }
}