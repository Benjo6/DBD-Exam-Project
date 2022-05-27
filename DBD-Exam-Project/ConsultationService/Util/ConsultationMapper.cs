using ConsultationService.Entities;
using lib.DTO;
using MongoDB.Driver.GeoJsonObjectModel;

namespace ConsultationService.Util
{
    public static class ConsultationMapper
    {
        public static ConsultationDto ToDto(ConsultationEntity entity){
            if (entity == null)
                return null;

            var point = entity.Location != null ? new GeoPointDto(entity.Location.Coordinates.X, entity.Location.Coordinates.Y) : null;

            var dto = new ConsultationDto() {
                Id = entity.ConsultationId,
                DoctorId = entity.DoctorId,
                PatientId = entity.PatientId,
                Regarding = entity.Regarding,
                ConsultationStartUtc = entity.ConsultationStartUtc,
                CreatedUtc = entity.CreatedUtc,
                GeoPoint = point
            };
            return dto;
        }

        public static ConsultationEntity FromDto(ConsultationDto dto){
            var entity = new ConsultationEntity(){
                DoctorId = dto.DoctorId,
                ConsultationId = dto.Id,
                PatientId = dto.PatientId,
                Regarding = dto.Regarding,
                ConsultationStartUtc = dto.ConsultationStartUtc,
                CreatedUtc = dto.CreatedUtc
            };
            return entity;
        }

        public static ConsultationEntity FromDto(ConsultationBookingRequestDto dto){
            var entity = new ConsultationEntity(){
                ConsultationId = dto.Id,
                PatientId = dto.PatientId,
                Regarding = dto.Regarding
            };
            return entity;
        }

        public static ConsultationEntity FromDto(ConsultationCreationDto dto){
            var entity = new ConsultationEntity(){
                ConsultationStartUtc = dto.ConsultationStartUtc,
                DoctorId = dto.DoctorId,
                CreatedUtc = DateTime.UtcNow,
                Location = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(dto.GeoPoint.Longitude, dto.GeoPoint.Latitude))
            };
            return entity;
        }
    }
}