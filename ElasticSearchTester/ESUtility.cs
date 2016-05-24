using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchTester
{
    class ESUtility
    {
        private string _indexName = "estest";
        private string _elasticSearchUrl = string.Empty;
        private ElasticClient _esClient = null;

        public ESUtility(string elasticSearchUrl)
        {
            _elasticSearchUrl = elasticSearchUrl;

            ConnectionSettings connectionSettings = new ConnectionSettings(new Uri(elasticSearchUrl));
            connectionSettings.DefaultIndex(_indexName);  // Set the default ElasticSearch index
            _esClient = new ElasticClient(connectionSettings);
        }

        public void Index(List<Dictionary<string, object>> documents)
        {
            var result = _esClient.IndexMany(documents);

            if (!result.IsValid)
            {
                StringBuilder errSb = new StringBuilder();
                foreach (var item in result.ItemsWithErrors)
                    errSb.AppendFormat("Failed to index document {0}: {1}]\n", item.Id, item.Error);

                if (result.OriginalException != null) errSb.AppendLine(result.OriginalException.Message);

                throw new Exception(errSb.ToString());
            }
        }

        public void DropIndex()
        {
            var result = _esClient.DeleteIndex(_indexName);
            if (!result.IsValid) throw new Exception(result.OriginalException.Message);
        }

        public ISearchResponse<Dictionary<string, object>> Query(string query)
        {
            ISearchResponse<Dictionary<string, object>> results = _esClient.Search<Dictionary<string, object>>(s => s
                .Index(_indexName)
                .Size(1000) // Max number of results to return
                .Query(q => q
                    .QueryString(qs => qs
                        .Query(query))));

            return results;
        }

        public void Delete(string query)
        {
            // With a base ElasticSearch installation, this operation returns:
            //      "No handler found for uri [/estest/_query?q=%2A] and method [DELETE]".
            // "DELETE" functionality is apparently an ElasticSearch Plug-in that needs to be added to a base installation.
            // Install "DELETE" with: 
            //      sudo bin/plugin install delete-by-query

            var result = _esClient.DeleteByQuery<Dictionary<string, object>>(_indexName, null, d => d
                 .Query(q => q
                     .QueryString(qs => qs
                         .Query(query))));

            if (!result.IsValid) throw new Exception(result.OriginalException.Message);
        }
    }
}
