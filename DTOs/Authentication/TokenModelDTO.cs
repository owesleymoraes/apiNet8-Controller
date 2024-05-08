using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.DTOs
{
    public class TokenModelDTO
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

    }
}