using HtmlSerializer;
using System.Text.RegularExpressions;

var html = await Load("https://learn.malkabruk.co.il/");

var cleanHtml = new Regex("\\s").Replace(html, " ");

var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(i => i.Length > 0 && i[0] != ' ').ToList();

HtmlElement root = new HtmlElement("html", htmlLines[1].Substring(htmlLines[1].IndexOf(" ") + 1), null);
HtmlElement currentElement = root;

htmlLines = htmlLines.GetRange(2, htmlLines.Count() - 2);

foreach (string line in htmlLines)
{
    if (line[0] == '/')
        if (line.Substring(1) == currentElement.Name)
            currentElement = currentElement.Parent;
        else
        {
            if (currentElement.InnerHtml == null)
                currentElement.InnerHtml = "";
            currentElement.InnerHtml += line;
        }
    else
    {
        var spaceIndex = line.IndexOf(" ");
        string tagName;
        if (spaceIndex >= 0)
            tagName = line.Substring(0, spaceIndex);
        else
            tagName = line.Substring(0, line.Length);
        if (HtmlHelper.Instance.TagsInHtml.Contains(tagName))
        {
            HtmlElement newElement = new HtmlElement(tagName, line.Substring(line.IndexOf(" ") + 1), currentElement);
            if (currentElement.Children == null)
                currentElement.Children = new List<HtmlElement>();
            currentElement.Children.Add(newElement);
            if (!HtmlHelper.Instance.DoNotRequireClosingTags.Contains(tagName))
                currentElement = newElement;
        }
        else
        {
            if (currentElement.InnerHtml == null)
                currentElement.InnerHtml = "";
            currentElement.InnerHtml += line;
        }
    }
}

Selector selector = Selector.ConvertQueryToObject("div img");
var result = root.SearchFitElements(selector);

static async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

foreach (var ancestor in result.First().Ancestors())
{
    Console.WriteLine(ancestor.Name);
}
