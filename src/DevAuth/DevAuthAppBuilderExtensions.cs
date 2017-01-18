// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Authentication.DevAuth;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods to add DevAuth authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class DevAuthAppBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="DevAuthMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables DevAuth authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseDevAuthAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<DevAuthMiddleware>();
        }

        /// <summary>
        /// Adds the <see cref="DevAuthMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables DevAuth authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">An action delegate to configure the provided <see cref="DevAuthOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseDevAuthAuthentication(this IApplicationBuilder app, DevAuthOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<DevAuthMiddleware>(Options.Create(options));
        }
    }
}
