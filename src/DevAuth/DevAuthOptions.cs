// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.DevAuth;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Options for the DevAuth authentication middleware.
    /// </summary>
    public class DevAuthOptions : RemoteAuthenticationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DevAuthOptions"/> class.
        /// </summary>
        public DevAuthOptions()
        {
            AuthenticationScheme = DevAuthDefaults.AuthenticationScheme;
            DisplayName = AuthenticationScheme;
            CallbackPath = new PathString("/signin-devauth");
            BackchannelTimeout = TimeSpan.FromSeconds(60);
            Events = new DevAuthEvents();
        }

        /// <summary>
        /// Gets or sets the consumer key used to communicate with DevAuth.
        /// </summary>
        /// <value>The consumer key used to communicate with DevAuth.</value>
        public string ConsumerKey { get; set; }

        /// <summary>
        /// Gets or sets the consumer secret used to sign requests to DevAuth.
        /// </summary>
        /// <value>The consumer secret used to sign requests to DevAuth.</value>
        public string ConsumerSecret { get; set; }

        /// <summary>
        /// Enables the retrieval user details during the authentication process, including
        /// e-mail addresses. Retrieving e-mail addresses requires special permissions
        /// from DevAuth Support on a per application basis. The default is false.
        /// See https://dev.DevAuth.com/rest/reference/get/account/verify_credentials
        /// </summary>
        public bool RetrieveUserDetails { get; set; }

        /// <summary>
        /// Gets or sets the type used to secure data handled by the middleware.
        /// </summary>
        public ISecureDataFormat<RequestToken> StateDataFormat { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IDevAuthEvents"/> used to handle authentication events.
        /// </summary>
        public new IDevAuthEvents Events
        {
            get { return (IDevAuthEvents)base.Events; }
            set { base.Events = value; }
        }
    }
}
