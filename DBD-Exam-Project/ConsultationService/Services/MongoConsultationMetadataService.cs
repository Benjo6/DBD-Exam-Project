using ConsultationService.Entities;
using ConsultationService.Util;
using lib.DTO;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ConsultationService.Services
{
    public class MongoConsultationMetadataService : IConsultationMetadataService
    {
        private MongoClient _client;
        private IMongoDatabase _database;
        private const string Collection = "consultations-metadata";

        public MongoConsultationMetadataService(IOptions<DatabaseSettings> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _client = new MongoClient(settings.Value.MongoConnectionString ?? throw new ArgumentNullException("MongoConnectionString"));
            _database = _client.GetDatabase("consultations");
        }
        public async Task<ConsultationMetadataDto> GetLatestMetadataAsync()
        {
            var consultationEntity = await _database.GetCollection<ConsultationMetadataEntity>(Collection).Find(x => true).SortByDescending(x => x.CreatedUtc).FirstOrDefaultAsync();
            return ConsultationMetadataMapper.ToDto(consultationEntity); ;
        }
        public async Task<ConsultationMetadataDto> AddMetadataAsync(ConsultationMetadataDto consultationMetadata)
        {
            if (consultationMetadata == null)
                throw new ArgumentNullException(nameof(consultationMetadata));

            var consulationEntity = ConsultationMetadataMapper.FromDto(consultationMetadata);
            await _database.GetCollection<ConsultationMetadataEntity>(Collection).InsertOneAsync(consulationEntity);
            return ConsultationMetadataMapper.ToDto(consulationEntity);
        }
    }
}