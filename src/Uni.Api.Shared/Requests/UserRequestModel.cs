﻿using JetBrains.Annotations;

namespace Uni.Api.Shared.Requests
{
    [PublicAPI]
    public class UserRequestModel
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public int PersonId { get; set; }
    }
}
