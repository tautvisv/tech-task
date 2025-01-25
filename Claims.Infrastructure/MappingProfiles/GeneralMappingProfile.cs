using AutoMapper;
using Claims.Domain.Models;

namespace Claims.Infrastructure.MappingProfiles
{
    public class GeneralMappingProfile : Profile
    {
        public GeneralMappingProfile()
        {
            CreateMap<DateOnly, DateTime>().ConvertUsing<DateOnlyToDateTimeConverter>();
            CreateMap<DateTime, DateOnly>().ConvertUsing<DateTimeToDateOnlyConverter>();
        }

        internal class DateOnlyToDateTimeConverter : ITypeConverter<DateOnly, DateTime>
        {
            public DateTime Convert(DateOnly source, DateTime destination, ResolutionContext context)
            {
                return source.ToDateTime(TimeOnly.MinValue);
            }
        }

        internal class DateTimeToDateOnlyConverter : ITypeConverter<DateTime, DateOnly>
        {
            public DateOnly Convert(DateTime source, DateOnly destination, ResolutionContext context)
            {
                return DateOnly.FromDateTime(source);
            }
        }
    }
}
