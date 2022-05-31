using MongoDB.Driver;
using lib.DTO;
using ConsultationService.Entities;
using ConsultationService.Util;
using Microsoft.Extensions.Options;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Bson;

namespace ConsultationService.Services
{
    public class MongoConsultationService : IConsultationService
    {
        private MongoClient _client;
        private IMongoDatabase _database;

        public MongoConsultationService(IOptions<DatabaseSettings> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _client = new MongoClient(settings.Value.MongoConnectionString ?? throw new ArgumentNullException("MongoConnectionString"));
            _database = _client.GetDatabase("consultations");
        }

        public ConsultationDto BookConsultation(ConsultationBookingRequestDto consultationDto)
        {
            return BookConsultationAsync(consultationDto).Result;
        }

        public async Task<ConsultationDto> BookConsultationAsync(ConsultationBookingRequestDto consultationDto)
        {
            var consultationEntity = ConsultationMapper.FromDto(consultationDto);
            var builder = Builders<ConsultationEntity>
            .Update.Set(x => x.PatientId, consultationDto.PatientId)
            .Set(x => x.Regarding, consultationDto.Regarding);

            var options = new UpdateOptions { IsUpsert = false };

            var result = await _database.GetCollection<ConsultationEntity>("consultations").UpdateOneAsync(x =>
            x.ConsultationId == consultationDto.Id && x.PatientId == null
            , builder, options);
            return result.ModifiedCount > 0 ? ConsultationMapper.ToDto(consultationEntity) : null;
        }

        public ConsultationDto CreateConsultation(ConsultationCreationDto consultationDto)
        {
            return CreateConsultationAsync(consultationDto).Result;
        }

        public async Task<ConsultationDto> CreateConsultationAsync(ConsultationCreationDto consultationDto)
        {
            if (consultationDto.DoctorId == null || consultationDto.GeoPoint == null)
                throw new ArgumentException(nameof(consultationDto));

            var consulationEntity = ConsultationMapper.FromDto(consultationDto);
            await _database.GetCollection<ConsultationEntity>("consultations").InsertOneAsync(consulationEntity);
            return ConsultationMapper.ToDto(consulationEntity);
        }

        public bool DeleteConsultation(string id)
        {
            return DeleteConsultationAsync(id).Result;
        }

        public async Task<bool> DeleteConsultationAsync(string id)
        {
            var result = await _database.GetCollection<ConsultationEntity>("consultations").DeleteOneAsync(x => x.ConsultationId == id);
            return result.DeletedCount == 1;
        }

        public ConsultationDto GetConsultation(string id)
        {
            return GetConsultationAsync(id).Result;
        }

        public async Task<ConsultationDto> GetConsultationAsync(string id)
        {
            var consultationEntity = await _database.GetCollection<ConsultationEntity>("consultations").Find(x => x.ConsultationId == id).FirstOrDefaultAsync();
            return ConsultationMapper.ToDto(consultationEntity);
        }

        public IEnumerable<ConsultationDto> GetConsultations(int skip = 100, int take = 0)
        {
            return GetConsultationsAsync(take, skip).Result;
        }

        public async Task<IEnumerable<ConsultationDto>> GetConsultationsAsync(int skip = 100, int take = 0)
        {
            var consultationEntity = await _database.GetCollection<ConsultationEntity>("consultations").Find(x => true).Skip(skip).Limit(take).ToListAsync();
            return consultationEntity.Select(ConsultationMapper.ToDto);
        }

        public IEnumerable<ConsultationDto> GetConsultationsForDoctor(string doctorId)
        {
            return GetConsultationsForDoctorAsync(doctorId).Result;
        }

        public async Task<IEnumerable<ConsultationDto>> GetConsultationsForDoctorAsync(string doctorId)
        {
            var consultationEntity = await _database.GetCollection<ConsultationEntity>("consultations").Find(x => x.DoctorId == doctorId).ToListAsync();
            return consultationEntity.Select(ConsultationMapper.ToDto);
        }

        public IEnumerable<ConsultationDto> GetConsultationsForPatient(string patientId)
        {
            return GetConsultationsForPatientAsync(patientId).Result;
        }

        public async Task<IEnumerable<ConsultationDto>> GetConsultationsForPatientAsync(string patientId)
        {
            var consultationEntity = await _database.GetCollection<ConsultationEntity>("consultations").Find(x => x.PatientId == patientId).ToListAsync();
            return consultationEntity.Select(ConsultationMapper.ToDto);
        }

        public IEnumerable<ConsultationDto> GetConsultationsOpenForBooking(GeoPointDto position, int distanceMeters)
        {
            return GetConsultationsOpenForBookingAsync(position, distanceMeters).Result;
        }

        public async Task<IEnumerable<ConsultationDto>> GetConsultationsOpenForBookingAsync(GeoPointDto position, int distanceMeters)
        {
            var builder = Builders<ConsultationEntity>.Filter;
            var point = GeoJson.Point(GeoJson.Position(position.Longitude, position.Latitude));
            var filter = builder.Near(x => x.Location, point, maxDistance: distanceMeters) & Builders<ConsultationEntity>.Filter.Eq(x => x.PatientId, null) & Builders<ConsultationEntity>.Filter.Exists(x => x.ConsultationStartUtc);

            var consultationEntity = await _database.GetCollection<ConsultationEntity>("consultations").Find(filter).ToListAsync();
            return consultationEntity.Select(ConsultationMapper.ToDto);
        }

        public ConsultationDto UpdateConsultation(ConsultationDto consultationDto)
        {
            return UpdateConsultationAsync(consultationDto).Result;
        }

        public async Task<ConsultationDto> UpdateConsultationAsync(ConsultationDto consultationDto)
        {
            if (string.IsNullOrWhiteSpace(consultationDto.Id))
                throw new ArgumentException("Consultation is missing ID");

            var filter = Builders<ConsultationEntity>.Filter.Eq("_id", consultationDto.Id);
            var consultationEntity = await _database.GetCollection<ConsultationEntity>("consultations").Find(filter).FirstOrDefaultAsync();
            return ConsultationMapper.ToDto(consultationEntity);
        }
    }
}