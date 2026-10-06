using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.AIDtos;

namespace api.Services
{
    public interface IAIService
    {
        Task<BookingIntentDto> AskAsync(string question); 
    }
}