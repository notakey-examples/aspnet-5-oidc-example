
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace NotakeyOidcDemo
{
    public class JsonKeyArrayClaimAction : ClaimAction

    {
        /// <summary>
        /// Creates a new JsonKeyArrayClaimAction.
        /// </summary>
        /// <param name="claimType">The value to use for Claim.Type when creating a Claim.</param>
        /// <param name="valueType">The value to use for Claim.ValueType when creating a Claim.</param>
        /// <param name="jsonKey">The top level key to look for in the json user data.</param>
        public JsonKeyArrayClaimAction(string claimType, string valueType, string jsonKey) : base(claimType, valueType)
        {
            _jsonKey = jsonKey;
        }

        /// <summary>
        /// The top level key to look for in the json user data.
        /// </summary>
        private string _jsonKey { get; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Run(JsonElement userData, ClaimsIdentity identity, string issuer)
        {
            JsonElement jsonGroups;
            userData.TryGetProperty(_jsonKey, out jsonGroups);

            foreach (var value in jsonGroups.EnumerateArray())
            {
                identity.AddClaim(new Claim(ClaimType, value.ToString(), ValueType, issuer));
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
