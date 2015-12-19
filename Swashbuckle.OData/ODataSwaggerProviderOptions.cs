﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.OData.Descriptions;
using Swashbuckle.Swagger;

namespace Swashbuckle.OData
{
    internal class ODataSwaggerProviderOptions
    {
        public ODataSwaggerProviderOptions(SwaggerProviderOptions swaggerProviderOptions)
        {
            Contract.Requires(swaggerProviderOptions != null);

            VersionSupportResolver = swaggerProviderOptions.VersionSupportResolver;
            Schemes = swaggerProviderOptions.Schemes;
            SecurityDefinitions = swaggerProviderOptions.SecurityDefinitions;
            IgnoreObsoleteActions = swaggerProviderOptions.IgnoreObsoleteActions;
            GroupingKeySelector = swaggerProviderOptions.GroupingKeySelector ?? DefaultGroupingKeySelector;
            GroupingKeyComparer = swaggerProviderOptions.GroupingKeyComparer ?? Comparer<string>.Default;
            CustomSchemaMappings = swaggerProviderOptions.CustomSchemaMappings ?? new Dictionary<Type, Func<Schema>>();
            SchemaFilters = swaggerProviderOptions.SchemaFilters ?? new List<ISchemaFilter>();
            ModelFilters = swaggerProviderOptions.ModelFilters ?? new List<IModelFilter>();
            IgnoreObsoleteProperties = swaggerProviderOptions.IgnoreObsoleteProperties;
            SchemaIdSelector = swaggerProviderOptions.SchemaIdSelector ?? DefaultSchemaIdSelector;
            DescribeAllEnumsAsStrings = swaggerProviderOptions.DescribeAllEnumsAsStrings;
            DescribeStringEnumsInCamelCase = swaggerProviderOptions.DescribeStringEnumsInCamelCase;
            OperationFilters = swaggerProviderOptions.OperationFilters ?? new List<IOperationFilter>();
            DocumentFilters = swaggerProviderOptions.DocumentFilters ?? new List<IDocumentFilter>();
            ConflictingActionsResolver = swaggerProviderOptions.ConflictingActionsResolver ?? DefaultConflictingActionsResolver;
        }

        public Func<ApiDescription, string, bool> VersionSupportResolver { get; private set; }

        public IEnumerable<string> Schemes { get; private set; }

        public IDictionary<string, SecurityScheme> SecurityDefinitions { get; private set; }

        public bool IgnoreObsoleteActions { get; private set; }

        public Func<ApiDescription, string> GroupingKeySelector { get; private set; }

        public IComparer<string> GroupingKeyComparer { get; private set; }

        public IDictionary<Type, Func<Schema>> CustomSchemaMappings { get; private set; }

        public IEnumerable<ISchemaFilter> SchemaFilters { get; private set; }

        public IEnumerable<IModelFilter> ModelFilters { get; private set; }

        public bool IgnoreObsoleteProperties { get; private set; }

        public Func<Type, string> SchemaIdSelector { get; private set; }

        public bool DescribeAllEnumsAsStrings { get; private set; }

        public bool DescribeStringEnumsInCamelCase { get; private set; }

        public IEnumerable<IOperationFilter> OperationFilters { get; private set; }

        public IEnumerable<IDocumentFilter> DocumentFilters { get; private set; }

        public Func<IEnumerable<ApiDescription>, ApiDescription> ConflictingActionsResolver { get; private set; }

        private static string DefaultGroupingKeySelector(ApiDescription apiDescription)
        {
            Contract.Requires(apiDescription != null);

            return apiDescription.ActionDescriptor.ControllerDescriptor.ControllerName == "Restier"
                ? ((RestierHttpActionDescriptor)apiDescription.ActionDescriptor).EntitySetName
                : apiDescription.ActionDescriptor.ControllerDescriptor.ControllerName;
        }

        private static string DefaultSchemaIdSelector(Type type)
        {
            return type.FriendlyId();
        }

        private static ApiDescription DefaultConflictingActionsResolver(IEnumerable<ApiDescription> apiDescriptions)
        {
            Contract.Requires(apiDescriptions != null);

            var first = apiDescriptions.First();
            throw new NotSupportedException($"Not supported by Swagger 2.0: Multiple operations with path '{first.RelativePathSansQueryString()}' and method '{first.HttpMethod}'. " + "See the config setting - \"ResolveConflictingActions\" for a potential workaround");
        }
    }
}