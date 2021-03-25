using CorpusSearchModule.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using CorpusSearchModule.Core.Services.TextCorpusSystemDriver.Models;
using Newtonsoft.Json;
using System.IO;

namespace CorpusSearchModule.Core.Services.TextCorpusSystemDriver
{
    public class TextCorpusClient
    {
        private const string root = "https://localhost:44313/ApiQuery/";
        private const string exactSearchPath = "/ExactFormSearch";
        private const string lemmaSearchPath = "/LemmaPairSearch";
        private const string tagSearchPath = "/TagPairSearch";
        public List<QueryResult> SendQuery(Query query)
        {
            if (query is ExactQuery)
            {
                return SendExactQuery((ExactQuery)query);
            }
            else
            {
                return SendPairQuery((PairQuery)query);
            }    
        }
      
        private List<QueryResult> SendExactQuery(ExactQuery query)
        {
            var url = new StringBuilder(root);
            url.Append(exactSearchPath);
            
            string parameters = "?exactForm=" + HttpUtility.UrlEncode(query.ExactForm);
            url.Append(parameters);

            WebRequest request = WebRequest.Create(url.ToString());
            request.Method = "GET";

            WebResponse response = request.GetResponse();
            string result;
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                result = reader.ReadToEnd();
            }

            response.Close();

            return ParseResponse(result);           
        }

        private List<QueryResult> SendPairQuery(PairQuery query)
        {
            string url = DefinePairSearchPath(query.Type);
                     
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            string postData = JsonConvert.SerializeObject(query);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            WebResponse response = request.GetResponse();
            string result;
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                result = reader.ReadToEnd();
            }

            response.Close();

            return ParseResponse(result);
        }

        private string DefinePairSearchPath(PairQueryType queryType)
        {
            var url = new StringBuilder(root);
            if (queryType == PairQueryType.LemmaPair)
            {
                url.Append(lemmaSearchPath);
            }
            else
            {
                url.Append(tagSearchPath);
            }
            return url.ToString();
        }

        private List<QueryResult> ParseResponse(string response)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<QueryResult>>(response);               
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
