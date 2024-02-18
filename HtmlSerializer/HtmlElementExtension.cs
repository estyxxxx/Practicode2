using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    internal static class HtmlElementExtension
    {
        public static HashSet<HtmlElement> SearchFitElements(this HtmlElement htmlElement, Selector selector) 
        {
            HashSet<HtmlElement> hashSet = new HashSet<HtmlElement>();
            if (selector.Equals(htmlElement))
                htmlElement.SearchFitElements(selector.Children, hashSet);
            htmlElement.SearchFitElements(selector, hashSet); 
            return hashSet;
        }
        public static void SearchFitElements(this HtmlElement htmlElement, Selector selector, HashSet<HtmlElement> hashSet)
        {
            foreach (var child in htmlElement.Descendants())
                if (selector.Equals(child))
                    if (selector.Children == null)
                        hashSet.Add(child);
                    else
                        child.SearchFitElements(selector.Children, hashSet);
        }
    }
}
