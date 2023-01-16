using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Trisatech.MWorkforce.Business.Entities
{
    public class UserAuthenticated
    {
        public UserAuthenticated(string accountId, string userId, string userCode, string roleCode, string roleName, string name, List<string> territory)
        {
            AccountId = accountId;
            UserId = userId;
            RoleCode = roleCode;
            RoleName = roleName;
            Name = name;
            UserCode = userCode;
            TerritoryId = territory;
        }

        [JsonProperty("account_id")]
        public string AccountId { get; private set; }
        [JsonProperty("user_id")]
        public string UserId { get; private set; }
        [JsonProperty("user_code")]
        public string UserCode { get; private set; }
        [JsonProperty("role_code")]
        public string RoleCode { get; private set; }
        [JsonProperty("role_name")]
        public string RoleName { get; private set; }
        [JsonProperty("name")]
        public string Name { get; private set; }
        [JsonProperty("territory_id")]
        public List<string> TerritoryId { get; private set; }
    }
}
