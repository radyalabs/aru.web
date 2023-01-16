using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.AspNet.Common.Models
{
    public class JsonAlert
    {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }

    public class JsonEntity
    {
        [JsonProperty(PropertyName = "error")]
        public bool Error { get; set; }

        [JsonProperty(PropertyName = "force")]
        public bool ForceLogout { get; set; }

        [JsonProperty(PropertyName = "alert")]
        public JsonAlert Alerts { get; set; }

        [JsonProperty(PropertyName = "data")]
        public Object Data { get; set; }

        public JsonEntity()
        {
            this.Alerts = new JsonAlert();
            this.Data = new Object();
        }

        public void SetError(bool error)
        {
            this.Error = error;
        }

        public bool GetError()
        {
            return this.Error;
        }

        public void AddAlert(int code, string message)
        {
            this.Alerts.Code = code;
            this.Alerts.Message = message;
        }

        public JsonAlert GetAlert()
        {
            return this.Alerts;
        }

        public void AddData(Object data)
        {
            this.Data = data;
        }

        public Object GetData()
        {
            return this.Data;
        }

        public void SetForceLogout(bool forceLogout)
        {
            this.ForceLogout = forceLogout;
        }

        public bool GetForceLogout()
        {
            return this.ForceLogout;
        }

        public void SetJsonProperty(bool error, bool forceLogout, int code, string message, object data = null)
        {
            this.Error = error;
            this.ForceLogout = forceLogout;
            this.Alerts.Code = code;
            this.Alerts.Message = message;
            this.Data = data;
        }
    }
}
