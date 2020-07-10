/* 
 * Azure Game Day - RPSLS API
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = AzureGameDay.SDK.Client.OpenAPIDateConverter;

namespace AzureGameDay.SDK.Model
{
    /// <summary>
    /// MatchSetupRequest
    /// </summary>
    [DataContract]
    public partial class MatchSetupRequest :  IEquatable<MatchSetupRequest>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchSetupRequest" /> class.
        /// </summary>
        /// <param name="challengerId">challengerId.</param>
        public MatchSetupRequest(string challengerId = default(string))
        {
            this.ChallengerId = challengerId;
        }
        
        /// <summary>
        /// Gets or Sets ChallengerId
        /// </summary>
        [DataMember(Name="challengerId", EmitDefaultValue=false)]
        public string ChallengerId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class MatchSetupRequest {\n");
            sb.Append("  ChallengerId: ").Append(ChallengerId).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as MatchSetupRequest);
        }

        /// <summary>
        /// Returns true if MatchSetupRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of MatchSetupRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(MatchSetupRequest input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.ChallengerId == input.ChallengerId ||
                    (this.ChallengerId != null &&
                    this.ChallengerId.Equals(input.ChallengerId))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.ChallengerId != null)
                    hashCode = hashCode * 59 + this.ChallengerId.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}