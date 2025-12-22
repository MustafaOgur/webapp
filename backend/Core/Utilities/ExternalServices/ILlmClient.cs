using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.ExternalServices
{
    public interface ILlmClient
    {
        Task<string> SendMessageAsync(string prompt, string systemPrompt = null, string model = "openai/gpt-oss-20b:free", Dictionary<string, object> extraBody = null);

    }
}
