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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using AzureGameDay.SDK.Client;
using AzureGameDay.SDK.Model;

namespace AzureGameDay.SDK.Api
{

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IMatchSetupApiSync : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="AzureGameDay.SDK.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="matchSetupRequest"></param>
        /// <returns>MatchSetup</returns>
        MatchSetup MatchSetupPost (MatchSetupRequest matchSetupRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="AzureGameDay.SDK.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="matchSetupRequest"></param>
        /// <returns>ApiResponse of MatchSetup</returns>
        ApiResponse<MatchSetup> MatchSetupPostWithHttpInfo (MatchSetupRequest matchSetupRequest);
        #endregion Synchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IMatchSetupApiAsync : IApiAccessor
    {
        #region Asynchronous Operations
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="AzureGameDay.SDK.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="matchSetupRequest"></param>
        /// <returns>Task of MatchSetup</returns>
        System.Threading.Tasks.Task<MatchSetup> MatchSetupPostAsync (MatchSetupRequest matchSetupRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref="AzureGameDay.SDK.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="matchSetupRequest"></param>
        /// <returns>Task of ApiResponse (MatchSetup)</returns>
        System.Threading.Tasks.Task<ApiResponse<MatchSetup>> MatchSetupPostAsyncWithHttpInfo (MatchSetupRequest matchSetupRequest);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IMatchSetupApi : IMatchSetupApiSync, IMatchSetupApiAsync
    {

    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class MatchSetupApi : IMatchSetupApi
    {
        private AzureGameDay.SDK.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchSetupApi"/> class.
        /// </summary>
        /// <returns></returns>
        public MatchSetupApi() : this((string) null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchSetupApi"/> class.
        /// </summary>
        /// <returns></returns>
        public MatchSetupApi(String basePath)
        {
            this.Configuration = AzureGameDay.SDK.Client.Configuration.MergeConfigurations(
                AzureGameDay.SDK.Client.GlobalConfiguration.Instance,
                new AzureGameDay.SDK.Client.Configuration { BasePath = basePath }
            );
            this.Client = new AzureGameDay.SDK.Client.ApiClient(this.Configuration.BasePath);
            this.AsynchronousClient = new AzureGameDay.SDK.Client.ApiClient(this.Configuration.BasePath);
            this.ExceptionFactory = AzureGameDay.SDK.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchSetupApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public MatchSetupApi(AzureGameDay.SDK.Client.Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Configuration = AzureGameDay.SDK.Client.Configuration.MergeConfigurations(
                AzureGameDay.SDK.Client.GlobalConfiguration.Instance,
                configuration
            );
            this.Client = new AzureGameDay.SDK.Client.ApiClient(this.Configuration.BasePath);
            this.AsynchronousClient = new AzureGameDay.SDK.Client.ApiClient(this.Configuration.BasePath);
            ExceptionFactory = AzureGameDay.SDK.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchSetupApi"/> class
        /// using a Configuration object and client instance.
        /// </summary>
        /// <param name="client">The client interface for synchronous API access.</param>
        /// <param name="asyncClient">The client interface for asynchronous API access.</param>
        /// <param name="configuration">The configuration object.</param>
        public MatchSetupApi(AzureGameDay.SDK.Client.ISynchronousClient client,AzureGameDay.SDK.Client.IAsynchronousClient asyncClient, AzureGameDay.SDK.Client.IReadableConfiguration configuration)
        {
            if(client == null) throw new ArgumentNullException("client");
            if(asyncClient == null) throw new ArgumentNullException("asyncClient");
            if(configuration == null) throw new ArgumentNullException("configuration");

            this.Client = client;
            this.AsynchronousClient = asyncClient;
            this.Configuration = configuration;
            this.ExceptionFactory = AzureGameDay.SDK.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// The client for accessing this underlying API asynchronously.
        /// </summary>
        public AzureGameDay.SDK.Client.IAsynchronousClient AsynchronousClient { get; set; }

        /// <summary>
        /// The client for accessing this underlying API synchronously.
        /// </summary>
        public AzureGameDay.SDK.Client.ISynchronousClient Client { get; set; }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public String GetBasePath()
        {
            return this.Configuration.BasePath;
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public AzureGameDay.SDK.Client.IReadableConfiguration Configuration {get; set;}

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public AzureGameDay.SDK.Client.ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _exceptionFactory;
            }
            set { _exceptionFactory = value; }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="AzureGameDay.SDK.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="matchSetupRequest"></param>
        /// <returns>MatchSetup</returns>
        public MatchSetup MatchSetupPost (MatchSetupRequest matchSetupRequest)
        {
             AzureGameDay.SDK.Client.ApiResponse<MatchSetup> localVarResponse = MatchSetupPostWithHttpInfo(matchSetupRequest);
             return localVarResponse.Data;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="AzureGameDay.SDK.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="matchSetupRequest"></param>
        /// <returns>ApiResponse of MatchSetup</returns>
        public AzureGameDay.SDK.Client.ApiResponse< MatchSetup > MatchSetupPostWithHttpInfo (MatchSetupRequest matchSetupRequest)
        {
            // verify the required parameter 'matchSetupRequest' is set
            if (matchSetupRequest == null)
                throw new AzureGameDay.SDK.Client.ApiException(400, "Missing required parameter 'matchSetupRequest' when calling MatchSetupApi->MatchSetupPost");

            AzureGameDay.SDK.Client.RequestOptions localVarRequestOptions = new AzureGameDay.SDK.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };

            var localVarContentType = AzureGameDay.SDK.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

            var localVarAccept = AzureGameDay.SDK.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

            localVarRequestOptions.Data = matchSetupRequest;


            // make the HTTP request
            var localVarResponse = this.Client.Post< MatchSetup >("/MatchSetup", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("MatchSetupPost", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="AzureGameDay.SDK.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="matchSetupRequest"></param>
        /// <returns>Task of MatchSetup</returns>
        public async System.Threading.Tasks.Task<MatchSetup> MatchSetupPostAsync (MatchSetupRequest matchSetupRequest)
        {
             AzureGameDay.SDK.Client.ApiResponse<MatchSetup> localVarResponse = await MatchSetupPostAsyncWithHttpInfo(matchSetupRequest);
             return localVarResponse.Data;

        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref="AzureGameDay.SDK.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="matchSetupRequest"></param>
        /// <returns>Task of ApiResponse (MatchSetup)</returns>
        public async System.Threading.Tasks.Task<AzureGameDay.SDK.Client.ApiResponse<MatchSetup>> MatchSetupPostAsyncWithHttpInfo (MatchSetupRequest matchSetupRequest)
        {
            // verify the required parameter 'matchSetupRequest' is set
            if (matchSetupRequest == null)
                throw new AzureGameDay.SDK.Client.ApiException(400, "Missing required parameter 'matchSetupRequest' when calling MatchSetupApi->MatchSetupPost");


            AzureGameDay.SDK.Client.RequestOptions localVarRequestOptions = new AzureGameDay.SDK.Client.RequestOptions();

            String[] _contentTypes = new String[] {
                "application/json"
            };

            // to determine the Accept header
            String[] _accepts = new String[] {
                "application/json"
            };
            
            foreach (var _contentType in _contentTypes)
                localVarRequestOptions.HeaderParameters.Add("Content-Type", _contentType);
            
            foreach (var _accept in _accepts)
                localVarRequestOptions.HeaderParameters.Add("Accept", _accept);
            
            localVarRequestOptions.Data = matchSetupRequest;


            // make the HTTP request

            var localVarResponse = await this.AsynchronousClient.PostAsync<MatchSetup>("/MatchSetup", localVarRequestOptions, this.Configuration);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("MatchSetupPost", localVarResponse);
                if (_exception != null) throw _exception;
            }

            return localVarResponse;
        }

    }
}
