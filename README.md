# ElasticSearchTester
Simple C# WPF app that exercises the basic functionality of NEST and ElasticSearch.NET.

To be able to delete individual records from an ElasticSearch index, an ElasticSearch Plug-in needs to be added to the base installation.  Use the following command to add the plug-in:

  bin/plugin install delete-by-query

If using a Docker container to host ElasticSearch, use this instead:

  docker exec -it <container_id> bin/plugin install delete-by-query

The four functions provided by this application are as follows:

  Add All Data = indexes the contents of the Data folder in your ElasticSearch instance (do this first)
  
  Delete All Data = drops the ElasticSearch index
  
  Submit Query = submits a query against the ElasticSearch instance
  
  Delete = deletes the data specified in the "Query" text box from the ElasticSearch index
