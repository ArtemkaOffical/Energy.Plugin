using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Plugin
{
    public class CustomAttributes
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class Info : Attribute
        {
            public string Author { get; }
            public string Title { get; }
            public string Version { get; }
            public Info(string Author, string Title,string Version)
            {
                this.Author = Author;
                this.Title = Title;
                this.Version = Version;
            }
        }
        [AttributeUsage(AttributeTargets.Class)]
        public class Description : Attribute
        {
            public string Desc { get; }
            public Description(string Desc)
            {
                this.Desc = Desc;
            }
        }
    }
}
