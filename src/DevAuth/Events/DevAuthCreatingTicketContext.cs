// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Authentication.DevAuth
{
    /// <summary>
    /// Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.
    /// </summary>
    public class DevAuthCreatingTicketContext : BaseDevAuthContext
    {
        /// <summary>
        /// Initializes a <see cref="DevAuthCreatingTicketContext"/>
        /// </summary>
        /// <param name="context">The HTTP environment</param>
        /// <param name="options">The options for DevAuth</param>
        /// <param name="userId">DevAuth user ID</param>
        /// <param name="screenName">DevAuth screen name</param>
        /// <param name="accessToken">DevAuth access token</param>
        /// <param name="accessTokenSecret">DevAuth access token secret</param>
        /// <param name="user">User details</param>
        public DevAuthCreatingTicketContext(
            HttpContext context,
            DevAuthOptions options,
            string userId,
            string screenName,
            string accessToken,
            string accessTokenSecret,
            JObject user)
            : base(context, options)
        {
            UserId = userId;
            ScreenName = screenName;
            AccessToken = accessToken;
            AccessTokenSecret = accessTokenSecret;
            User = user ?? new JObject();
        }

        /// <summary>
        /// Gets the DevAuth user ID
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Gets the DevAuth screen name
        /// </summary>
        public string ScreenName { get; }

        /// <summary>
        /// Gets the DevAuth access token
        /// </summary>
        public string AccessToken { get; }

        /// <summary>
        /// Gets the DevAuth access token secret
        /// </summary>
        public string AccessTokenSecret { get; }

        /// <summary>
        /// Gets the JSON-serialized user or an empty
        /// <see cref="JObject"/> if it is not available.
        /// </summary>
        public JObject User { get; }

        /// <summary>
        /// Gets the <see cref="ClaimsPrincipal"/> representing the user
        /// </summary>
        public ClaimsPrincipal Principal { get; set; }

        /// <summary>
        /// Gets or sets a property bag for common authentication properties
        /// </summary>
        public AuthenticationProperties Properties { get; set; }
    }
}
