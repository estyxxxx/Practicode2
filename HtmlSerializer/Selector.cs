using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Children { get; set; }
        public Selector(string htmlQuery, Selector parent)
        {
            int indexId = htmlQuery.IndexOf('#');
            int indexClass = htmlQuery.IndexOf('.');
            //Assuming that the query is according to the convention that the id precedes the class
            if (htmlQuery[0] != '#' && htmlQuery[0] != '.')
            {
                string tagName;
                if (indexId >= 0)
                    tagName = htmlQuery.Substring(0, indexId);
                else if (indexClass > 0)
                    tagName = htmlQuery.Substring(0, indexClass);
                else
                    tagName = htmlQuery;
                if (HtmlHelper.Instance.TagsInHtml.Contains(tagName))
                    this.TagName = tagName;
            }
            if (indexId >= 0)
            {
                if (indexClass >= 0)
                    Id = htmlQuery.Substring(indexId + 1, (indexClass - (indexId + 1)));
                else
                    Id = htmlQuery.Substring(indexId + 1, htmlQuery.Length - indexId); ;
            }
            if (indexClass >= 0)
                Classes = htmlQuery.Substring(indexClass + 1).Split('.').ToList();
            this.Parent = parent;
        }
        //public Selector(string htmlQuery, Selector parent)
        //{
        //    int indexId = htmlQuery.IndexOf('#');
        //    int indexClass = htmlQuery.IndexOf('.');
        //    if (htmlQuery[0] != '#' && htmlQuery[0] != '.')
        //    {
        //        int minIndex;
        //        if (indexId >= 0)
        //        {
        //            if (indexClass >= 0)
        //                minIndex = Math.Min(indexId, indexClass);
        //            else
        //                minIndex = indexId;
        //        }
        //        else
        //            minIndex = indexClass;
        //        string tagName;
        //        if (minIndex >= 0)
        //            tagName = htmlQuery.Substring(0, minIndex);
        //        else
        //            tagName = htmlQuery.Substring(0, htmlQuery.Length);
        //        if (HtmlHelper.Instance.TagsInHtml.Contains(tagName))
        //            this.TagName = tagName;
        //    }
        //    if (indexId >= 0)
        //    {
        //        if (indexClass >= 0)
        //            Id = htmlQuery.Substring(indexId + 1, (indexClass - (indexId + 1)));
        //        else
        //            Id = htmlQuery.Substring(indexId + 1, htmlQuery.Length - indexId);
        //    }
        //    if(indexClass >= 0)
        //        Classes = htmlQuery.Substring(indexClass + 1).Split('.').ToList();
        //    this.Parent = parent;
        //}
        public static Selector ConvertQueryToObject(string htmlQuery)
        {
            string[] queryParts = htmlQuery.Split(' ');
            Selector fatherSelector = new Selector(queryParts[0], null);
            Selector motherSelector = fatherSelector;
            queryParts = queryParts.ToList().GetRange(1, queryParts.Length - 1).ToArray();
            foreach (string part in queryParts)
            {
                motherSelector.Children = new Selector(part, motherSelector);
                motherSelector = motherSelector.Children;
            }
            return fatherSelector;
        }
        public override bool Equals(object? obj)
        {
            if (obj is HtmlElement)
            {
                HtmlElement element = (HtmlElement)obj;
                if ((this.Id == null || element.Id == this.Id) && (this.TagName == null || element.Name == this.TagName))
                {
                    if (this.Classes != null)
                    {
                        if (element.Classes == null)
                            return false;
                        foreach (var c in this.Classes)
                        {
                            if (!element.Classes.Contains(c))
                                return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}

