using ConsultationService.Entities;
using lib.DTO;
using MongoDB.Bson;

namespace ConsultationService.Util
{
    public static class ConsultationMapper
    {
        public static ConsultationDto ToDto(ConsultationEntity entity){
            var dto = new ConsultationDto(){
                Id = entity.Id.ToString(),
                DoctorId = entity.DoctorId,
                PatientId = entity.DoctorId,
                Regarding = entity.Regarding

            };
            return dto;
        }

        public static ConsultationEntity FromDto(ConsultationDto dto){
            var entity = new ConsultationEntity(){
                DoctorId = dto.DoctorId,
                Id = new BsonObjectId(new ObjectId(dto.Id)),
                PatientId = dto.PatientId,
                Regarding = dto.Regarding
            };
            return entity;
        }

        public static ConsultationEntity FromDto(ConsultationBookingRequestDto dto){
            var entity = new ConsultationEntity(){
                Id = new BsonObjectId(new ObjectId(dto.Id)),
                PatientId = dto.PatientId,
                Regarding = dto.Regarding
            };
            return entity;
        }

        public static ConsultationEntity FromDto(ConsultationCreationDto dto){
            var entity = new ConsultationEntity(){
                ConsultationStartUtc = dto.ConsultationStartUtc,
                DoctorId = dto.DoctorId
            };
            return entity;
        }
    }
}