using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Energy.Plugin
{
   public class Plugin
    {
        private BindingFlags _flags = BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Public;
        private string _name;
        public string Name
        {
            set
            {
                if (string.IsNullOrEmpty(this.Name) || this._name == base.GetType().Name) this._name = value;
            }
            get
            {
                return this._name;
            }
        }
        public string Author;
        public string Title;
        public string Desc;
        public string Version;
        public Plugin() 
        {
            Name = GetType().Name;
            Author = "Artemka";
            Title = "default plugin";
            Desc = "test";
            Version = "1.0.0";
        }         
        public void GetInfoPlugin()
        {
            var CustomInfo = GetType().GetCustomAttributes(typeof(CustomAttributes.Info), true);
            if (CustomInfo.Length != 0)
            {
                var Info = CustomInfo[0] as CustomAttributes.Info;
                Author = Info.Author;
                Title = Info.Title;
                Version = Info.Version;
                var CustomDescription = GetType().GetCustomAttributes(typeof(CustomAttributes.Description), true);
                if (CustomDescription.Length != 0)
                {
                    var Description = CustomDescription[0] as CustomAttributes.Description;
                    Desc = Description.Desc;
                }
            }         
        }
        public object CallHook(string hook, params object[] args) =>  GetType().GetMethod(hook, _flags).Invoke(this, args);     
    }
}
