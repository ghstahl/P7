using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace P7.IdentityServer4.Common
{
    public class ClientModel :
        AbstractClientModel<
            List<ClaimModel>,
            List<Secret>,
            List<string>
        >
    {
        public ClientModel()
            : base()
        {
        }

        public ClientModel(Client client) : base(client)
        {
        }

        public override List<string> Serialize(List<string> stringList)
        {
            return stringList;
        }

        public override async Task<List<Claim>> DeserializeClaimsAsync(List<ClaimModel> obj)
        {
            return await Task.FromResult(obj == null ? null : obj.ToClaims());
        }

        public override List<ClaimModel> Serialize(List<Claim> claims)
        {
            return claims == null ? null : claims.ToClaimTypeRecords();
        }

        public override List<Secret> Serialize(List<Secret> secrets)
        {
            return secrets;
        }

        public override async Task<List<string>> DeserializeStringsAsync(List<string> obj)
        {
            return await Task.FromResult(obj);
        }

        public override async Task<List<Secret>> DeserializeSecretsAsync(List<Secret> obj)
        {
            return await Task.FromResult(obj);
        }

        public override bool Equals(object obj)
        {
            var other = obj as ClientModel;
            if (other == null)
            {
                return false;
            }

            var differenceAllowedScopes = AllowedScopes.Except(other.AllowedScopes);
            var equalsAllowedScopes = !differenceAllowedScopes.Any();
            var differenceClaims = Claims.Except(other.Claims);
            var equalsClaims = !differenceClaims.Any();
            var differenceClientSecrets = ClientSecrets.Except(other.ClientSecrets);
            var equalsClientSecrets = !differenceClientSecrets.Any();
            var differenceIdentityProviderRestrictions = IdentityProviderRestrictions.Except(other.IdentityProviderRestrictions);
            var equalsIdentityProviderRestrictions = !differenceIdentityProviderRestrictions.Any();
            var differencePostLogoutRedirectUris = PostLogoutRedirectUris.Except(other.PostLogoutRedirectUris);
            var equalsPostLogoutRedirectUris = !differencePostLogoutRedirectUris.Any();
            var differenceAllowedGrantTypes = AllowedGrantTypes.Except(other.AllowedGrantTypes);
            var equalsAllowedGrantTypes = !differenceAllowedGrantTypes.Any();
            var differenceAllowedCorsOrigins = AllowedCorsOrigins.Except(other.AllowedCorsOrigins);
            var equalsAllowedCorsOrigins = !differenceAllowedCorsOrigins.Any();
            var differenceRedirectUris = RedirectUris.Except(other.RedirectUris);
            var equalsRedirectUris = !differenceRedirectUris.Any();
            

            var result = 
                      AbsoluteRefreshTokenLifetime.Equals(other.AbsoluteRefreshTokenLifetime)
                   && AccessTokenLifetime.Equals(other.AccessTokenLifetime)
                   && AccessTokenType.Equals(other.AccessTokenType)
                   && AllowAccessTokensViaBrowser.Equals(other.AllowAccessTokensViaBrowser)
                   && equalsAllowedCorsOrigins
                   && equalsAllowedGrantTypes
                   && equalsAllowedScopes
                   && AllowOfflineAccess.Equals(other.AllowOfflineAccess)
                   && AllowPlainTextPkce.Equals(other.AllowPlainTextPkce)
                   && AllowRememberConsent.Equals(other.AllowRememberConsent)
                   && AlwaysSendClientClaims.Equals(other.AlwaysSendClientClaims)
                   && AuthorizationCodeLifetime.Equals(other.AuthorizationCodeLifetime)
                   && equalsClaims
                   && ClientId.Equals(other.ClientId)
                   && ClientName.Equals(other.ClientName)
                   && equalsClientSecrets
                   && ClientUri.Equals(other.ClientUri)
                   && Enabled.Equals(other.Enabled)
                   && EnableLocalLogin.Equals(other.EnableLocalLogin)
                   && equalsIdentityProviderRestrictions
                   && IdentityTokenLifetime.Equals(other.IdentityTokenLifetime)
                   && IncludeJwtId.Equals(other.IncludeJwtId)
                   && LogoUri.Equals(other.LogoUri)
                   && LogoutSessionRequired.Equals(other.LogoutSessionRequired)
                   && LogoutUri.Equals(other.LogoutUri)
                   && equalsPostLogoutRedirectUris
                   && PrefixClientClaims.Equals(other.PrefixClientClaims)
                   && ProtocolType.Equals(other.ProtocolType)
                   && equalsRedirectUris
                   && RefreshTokenExpiration.Equals(other.RefreshTokenExpiration)
                   && RefreshTokenUsage.Equals(other.RefreshTokenUsage)
                   && RequireClientSecret.Equals(other.RequireClientSecret)
                   && RequireConsent.Equals(other.RequireConsent)
                   && RequirePkce.Equals(other.RequirePkce)
                   && SlidingRefreshTokenLifetime.Equals(other.SlidingRefreshTokenLifetime)
                  && UpdateAccessTokenClaimsOnRefresh.Equals(other.UpdateAccessTokenClaimsOnRefresh);
            return result;
        }

        public override int GetHashCode()
        {
            var code = ClientId.GetHashCode();
            return code;
        }
    }
}