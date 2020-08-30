using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using VkNet;
using VkNet.Model.RequestParams;
using VkNet.Model;

namespace Energy.Plugin
{
   public class Plugin
    {
        private BindingFlags _flags = BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Public;
        private string _name;
        public VkApi api = new VkApi();
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
        protected void SendMessage(long? PeerId, string Message)
        {
            try
            {
                api.Messages.Send(new MessagesSendParams()
                {
                    PeerId = PeerId,
                    Message = Message,
                    RandomId = new System.Random().Next()
                });
            }
            catch 
            {
                api.Messages.Send(new MessagesSendParams()
                {
                    PeerId = PeerId,
                    Message = "Ошибка. Скорее всего один из параметров не указан или указан не верно.",
                    RandomId = new System.Random().Next()
                });
              
            
            }
        }
        protected string GetMessageFromChat(Message message)
        {
            return message.Text;
        }
        public string GetFormat(Message message, string mess)
        {
            return string.Format(mess, GetMessageFromChat(message), GetUserInfo(message.PeerId.Value, message.FromId));
        }
        protected string GetUserInfo(long PeerId, long? UserId)
        {
            try
            {
                foreach (var item in api.Messages.GetConversationMembers(PeerId, new List<string>() { "LastName", "FirstName", "Id" }).Profiles)
                {

                    if (UserId == item.Id)
                    {
                        return string.Format($"{item.FirstName} {item.LastName} [id {item.Id}]");
                    }
                }
            }
            catch
            {
                SendMessage(PeerId, $"error GetUserInfo {UserId.Value}");
            }
            return string.Empty;
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
