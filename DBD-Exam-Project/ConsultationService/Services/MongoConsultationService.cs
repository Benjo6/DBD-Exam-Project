using MongoDB.Driver;
using lib;
using lib.DTO;
using ConsultationService.Entities;
using ConsultationService.Util;
using Microsoft.Extensions.Options;

namespace ConsultationService.Services
{
    public class MongoConsultationService: IConsultationService
    {
        private MongoClient _client;
        private IMongoDatabase _database;

        public MongoConsultationService(IOptions<DatabaseSettings> settings)
        {
            _client = new MongoClient(settings.Value.MongoConnectionString ?? throw new ArgumentNullException("MongoConnectionString"));
            _database = _client.GetDatabase("consultations");
        }

        public ConsultationDto BookConsultation(ConsultationBookingRequestDto consultationDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ConsultationDto> BookConsultationAsync(ConsultationBookingRequestDto consultationDto)
        {
            var consulationEntity = Util.ConsultationMapper.FromDto(consultationDto);
            var filter = Builders<ConsultationEntity>.Filter.Eq("_id", consultationDto.Id);
            await _database.GetCollection<ConsultationEntity>("consultations").UpdateOneAsync(filter, consulationEntity, null, CancellationToken.None);
            return Util.ConsultationMapper.ToDto(consulationEntity);
        }

        public ConsultationDto CreateConsultation(ConsultationCreationDto consultationDto)
        {
            return CreateConsultationAsync(consultationDto).Result;
        }

        public async Task<ConsultationDto> CreateConsultationAsync(ConsultationCreationDto consultationDto)
        {
            var consulationEntity = Util.ConsultationMapper.FromDto(consultationDto);
            await _database.GetCollection<ConsultationEntity>("consultations").InsertOneAsync(consulationEntity);
            return Util.ConsultationMapper.ToDto(consulationEntity);
        }

        public ConsultationDto GetConsultation(string id)
        {
            return GetConsultationAsync(id).Result;
        }

        public async Task<ConsultationDto> GetConsultationAsync(string id)
        {
            var filter = Builders<ConsultationEntity>.Filter.Eq("_id", id);
            var consultationEntity = await _database.GetCollection<ConsultationEntity>("consultations").Find(filter).FirstOrDefaultAsync();
            return ConsultationMapper.ToDto(consultationEntity);
        }

        public IEnumerable<ConsultationDto> GetConsultationsForDoctor(string doctorId)
        {
            return GetConsultationsForDoctorAsync(doctorId).Result;
        }

        public async Task<IEnumerable<ConsultationDto>> GetConsultationsForDoctorAsync(string doctorId)
        {
            var filter = Builders<ConsultationEntity>.Filter.Eq("doctorId", doctorId);
            var consultationEntity = await _database.GetCollection<ConsultationEntity>("consultations").Find(filter).ToListAsync();
            return consultationEntity.Select(entity => ConsultationMapper.ToDto(entity));
        }

        public IEnumerable<ConsultationDto> GetConsultationsForPatient(string patientId)
        {
            return GetConsultationsForPatientAsync(patientId).Result;
        }

        public async Task<IEnumerable<ConsultationDto>> GetConsultationsForPatientAsync(string patientId)
        {
            var filter = Builders<ConsultationEntity>.Filter.Eq("patientId", patientId);
            var consultationEntity = await _database.GetCollection<ConsultationEntity>("consultations").Find(filter).ToListAsync();
            return consultationEntity.Select(entity => ConsultationMapper.ToDto(entity));
        }

        public ConsultationDto UpdateConsultation(ConsultationDto consultationDto)
        {
            return UpdateConsultationAsync(consultationDto).Result;
        }

        public async Task<ConsultationDto> UpdateConsultationAsync(ConsultationDto consultationDto)
        {
            if(string.IsNullOrWhiteSpace(consultationDto.Id))
                throw new ArgumentException("Consultation is missing ID");

            var filter = Builders<ConsultationEntity>.Filter.Eq("_id", consultationDto.Id);
            var consultationEntity = await _database.GetCollection<ConsultationEntity>("consultations").Find(filter).FirstOrDefaultAsync();
            return ConsultationMapper.ToDto(consultationEntity);
        }
    }
}