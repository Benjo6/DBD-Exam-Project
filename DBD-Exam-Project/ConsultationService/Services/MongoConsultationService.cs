using MongoDB.Driver;
using lib;
using lib.DTO;
using ConsultationService.Entities;
using ConsultationService.Util;

namespace ConsultationService.Services
{
    public class MongoConsultationService: IConsultationService
    {
        private MongoClient _client;

        public MongoConsultationService(string connectionstring)
        {
            _client = new MongoClient(connectionstring ?? throw new ArgumentNullException(nameof(connectionstring)));
        }

        public ConsultationDto GetConsultation(string id)
        {
            return GetConsultationAsync(id).Result;
        }

        public async Task<ConsultationDto> GetConsultationAsync(string id)
        {
            var filter = Builders<ConsultationEntity>.Filter.Eq("_id", id);
            var consultationEntity = await _client.GetDatabase("consultations").GetCollection<ConsultationEntity>("consultations").Find(filter).FirstOrDefaultAsync();
            return ConsultationMapper.ToDto(consultationEntity);
        }

        public IEnumerable<ConsultationDto> GetConsultationsForDoctor(string doctorId)
        {
            return GetConsultationsForDoctorAsync(doctorId).Result;
        }

        public async Task<IEnumerable<ConsultationDto>> GetConsultationsForDoctorAsync(string doctorId)
        {
            var filter = Builders<ConsultationEntity>.Filter.Eq("doctorId", doctorId);
            var consultationEntity = await _client.GetDatabase("consultations").GetCollection<ConsultationEntity>("consultations").Find(filter).ToListAsync();
            return consultationEntity.Select(entity => ConsultationMapper.ToDto(entity));
        }

        public IEnumerable<ConsultationDto> GetConsultationsForPatient(string patientId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConsultationDto>> GetConsultationsForPatientAsync(string patientId)
        {
            throw new NotImplementedException();
        }
    }
}