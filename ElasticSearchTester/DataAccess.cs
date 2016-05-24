using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ElasticSearchTester
{
    public class DataAccess
    {
        public List<Dictionary<string, object>> GetData()
        {
            List<Dictionary<string, object>> documents = new List<Dictionary<string, object>>();

            List<string> headers = new List<string>();
            string[] dataLines = File.ReadAllLines(@"..\\..\\Data\\proceedingsofzoo19221zool.txt");
            IEnumerable<string> files = Directory.EnumerateFiles(@"..\\..\\Data\proceedingsofzoo19221zool");
            List<string> fileList = files.ToList();
            fileList.Sort();

            bool readHeaders = true;
            foreach (string data in dataLines)
            {
                if (readHeaders)
                {
                    string[] headerLabels = data.Split('\t');
                    foreach (string label in headerLabels)
                    {
                        headers.Add(label);
                    }
                    readHeaders = false;
                }
                else
                {
                    Dictionary<string, object> document = new Dictionary<string, object>();
                    string[] values = data.Split('\t');
                    int index = 0;
                    foreach (string value in values)
                    {
                        string[] allValues = value.Split('|');
                        document.Add(headers[index], allValues);
                        index++;
                    }

                    //document.Add("_id", document["PageID"]);  // This causes "duplicate field definition" error
                    document.Add("PageText", File.ReadAllText(fileList[documents.Count]));

                    documents.Add(document);
                }
            }

            return documents;
        }
    }
}
