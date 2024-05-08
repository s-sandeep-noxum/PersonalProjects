using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json; // Install Newtonsoft.Json from Nuget

namespace DevOpsWorkItems
{
    class Program
    {
        public class WorkItemsList
        {
            public Workitem[] workItems { get; set; }
        }

        public class Workitem
        {
            public object rel { get; set; }
            public object source { get; set; }
            public Target target { get; set; }
        }

        public class Target
        {
            public int id { get; set; }
        }

        static class Azure
        {
            public const string BASE = "https://noxum.visualstudio.com/";
            public const string PAT = "3ljqhiwlkowddiqscp2xd6nd3rwfs2g6vs5yfaeg2kcdzsgi264a";
            public const string ORG = "OpenTechGuides";
            public const string API = "api-version=5.1-preview";
            public const string PROJECT = "OTGRESTDemo";
            public const string PROJECT_TEAM = "OTGRESTDemoTeam";
            public const string BACKLOG_ID = "Microsoft.RequirementCategory";
        }

        static void Sample(string[] args)
        {
            // Create and initialize HttpClient instance.      
            HttpClient client = new HttpClient();

            // Set Media Type of Response.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Generate base64 encoded authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Azure.PAT))));

            // Get all work item ids in backlog
            string BackLogWorkItemsURI = String.Join("?", String.Join("/", Azure.BASE, Azure.ORG, Azure.PROJECT, Azure.PROJECT_TEAM, "_apis/work/backlogs", Azure.BACKLOG_ID, "/workItems"), Azure.API);
            Console.WriteLine(BackLogWorkItemsURI);
            string result = SendRequest(client, BackLogWorkItemsURI).Result;
            WorkItemsList workitemlist = JsonConvert.DeserializeObject<WorkItemsList>(result);

            // Combine workitem ids as a comma separated list.
            List<int> ids = new List<int>();
            foreach (Workitem wit in workitemlist.workItems)
            {
                ids.Add(wit.target.id);
            }

            // Build URI to get details of all workitems
            string workItemListURI = String.Join("/", Azure.BASE, Azure.ORG, Azure.PROJECT, "_apis/wit/workitems?ids=");
            workItemListURI += String.Join("&", String.Join(",", ids), Azure.API);
            Console.WriteLine(workItemListURI);
            result = SendRequest(client, workItemListURI).Result;

            // Pretty print the JSON
            dynamic witem = JsonConvert.DeserializeObject<object>(result);
            Console.WriteLine(JsonConvert.SerializeObject(witem, Formatting.Indented));

            Console.ReadLine();
            client.Dispose();

        } // End of Main

        // Send HTTP GET Request and get response
        public static async Task<string> SendRequest(HttpClient client, string uri)
        {
            try
            {
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    return (await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        } // End of SendRequest method


    }
}