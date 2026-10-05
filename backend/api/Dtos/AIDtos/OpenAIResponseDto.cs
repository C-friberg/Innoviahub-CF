using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.AIDtos
{
    public class OpenAIResponseDto
    {
        public List<OpenAiOutputDto> Output {get; set;} = new(); 
    }

    public class OpenAiOutputDto
    {
        public List<OpenAiContentDto> Content {get; set;} = new(); 
    }

    public class OpenAiContentDto
    {
        public string Text {get; set;} = string.Empty; 
    }
}