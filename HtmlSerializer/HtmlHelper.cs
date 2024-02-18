using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] TagsInHtml { get; set; }
        public string[] DoNotRequireClosingTags { get; set; }
        public HtmlHelper()
        {
            var content = File.ReadAllText("./files/html-tags.json");
            TagsInHtml = JsonSerializer.Deserialize<string[]>(content);
            content = File.ReadAllText("./files/html-tags-void.json");
            DoNotRequireClosingTags = JsonSerializer.Deserialize<string[]>(content);
        }
    }
}
