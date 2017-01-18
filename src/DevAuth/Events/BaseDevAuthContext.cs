// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNet.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication.DevAuth
{
    /// <summary>
    /// Base class for other DevAuth contexts.
    /// </summary>
    public class BaseDevAuthContext : BaseContext
    {
        /// <summary>
        /// Initializes a <see cref="BaseDevAuthContext"/>
        /// </summary>
        /// <param name="context">The HTTP environment</param>
        /// <param name="options">The options for DevAuth</param>
        public BaseDevAuthContext(HttpContext context, DevAuthOptions options)
            : base(context)
        {
            Options = options;
        }

        public DevAuthOptions Options { get; }
    }
}
