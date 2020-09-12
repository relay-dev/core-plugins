using System.Text;
using AutoMapper;

namespace Core.Plugins.AutoMapper.Mappers
{
    public class PrimitiveMappers : Profile
    {
        public PrimitiveMappers()
        {
            // These facilitate an auto-mapper between the byte[] RowVersion on entities and the string RowVersion that is exposed on the DTO
            CreateMap<byte[], string>().ConvertUsing(bytes => bytes == null ? null : new UTF8Encoding().GetString(bytes, 0, bytes.Length));
            CreateMap<string, byte[]>().ConvertUsing(str => string.IsNullOrEmpty(str) ? new byte[0] : new UTF8Encoding().GetBytes(str));
        }
    }
}
