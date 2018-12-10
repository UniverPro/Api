using System.Collections.Generic;
using Newtonsoft.Json;

namespace Uni.Api.Shared.Responses
{
    [JsonObject]
    public class RoleResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public List<PermissionResponseModel> Permissions { get; set; }
    }
}