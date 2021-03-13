using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Interfaces
{
    public interface ITranslateService
    {
        string TranslateText(string text, string sourceLanguage = "en", string targetLanguage = "tr");
    }
}
