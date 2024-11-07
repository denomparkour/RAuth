using RAuth.Application.Constants;

namespace RAuth.Application.DTO.ResponseDTO
{
    public static class ResponseBuilder
    {
        public static Response Build(object payload)
        {
            Response response = new Response();
            response.Status = GlobalConstants.SUCCESS;
            response.Payload = payload;
            return response;
        }
    }
}
