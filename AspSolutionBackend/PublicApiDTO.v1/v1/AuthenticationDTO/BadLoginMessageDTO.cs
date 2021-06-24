using System.Collections.Generic;

namespace PublicApiDTO.v1.v1.AuthenticationDTO
{
    public class BadLoginMessageDTO
    {
        public IList<string> Messages { get; set; } = new List<string>();


        public BadLoginMessageDTO()
        {
        }


        public BadLoginMessageDTO(params string[] messages)
        {
            Messages = messages;
        }
    }
}