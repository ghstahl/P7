// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Authentication.DevAuth
{
    /// <summary>
    /// The DevAuth access token retrieved from the access token endpoint.
    /// </summary>
    public class AccessToken : RequestToken
    {
        /// <summary>
        /// Gets or sets the DevAuth User ID.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the DevAuth screen name.
        /// </summary>
        public string ScreenName { get; set; }
    }
}
