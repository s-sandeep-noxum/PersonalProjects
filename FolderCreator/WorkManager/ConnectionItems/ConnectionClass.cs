using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;

namespace WorkManager.ConnectionItems
{
	public static class ConnectionClass
	{
		private const String c_collectionUri = @"https://noxum.visualstudio.com/";		
		private static VssConnection connection = null;

		public static VssConnection Connection
		{
			get
			{
				if (connection == null)
				{
					ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
					IConfiguration configuration = configurationBuilder
																					.AddUserSecrets("011cf0df-752c-481d-96cc-728b1efb92fe")
																					.Build();

					string azureToken = configuration.GetSection("Azure")["cToken"];

					Uri Uri = new Uri(c_collectionUri);
					connection = new(Uri, new VssBasicCredential(string.Empty, azureToken));
					return connection;
				}
				return connection;
			}
		}

		/*
		private void GetWorkItemsAssignedTome(List<WorkItemDetails> list)
		{
				try
				{
						const String c_collectionUri = @"https://noxum.visualstudio.com/";
						const String c_projectName = "Noxum.PS5";
						const String c_token = "";

						Uri orgUrl = new Uri(c_collectionUri);

						// Connect to Azure DevOps Services
						VssConnection connection = new VssConnection(orgUrl, new VssBasicCredential(string.Empty, c_token));

						//// Get a GitHttpClient to talk to the Git endpoints
						//using (GitHttpClient gitClient = connection.GetClient<GitHttpClient>())
						//{
						//}

						// Create instance of WorkItemTrackingHttpClient using VssConnection
						WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

						// Get 2 levels of query hierarchy items
						List<QueryHierarchyItem> queryHierarchyItems = witClient.GetQueriesAsync(c_projectName, depth: 2).Result;

						// Search for 'My Queries' folder
						QueryHierarchyItem myQueriesFolder = queryHierarchyItems.FirstOrDefault(qhi => qhi.Name.Equals("My Queries"));
						if (myQueriesFolder != null)
						{
								string queryName = "Open WI";

								// See if our 'REST Sample' query already exists under 'My Queries' folder.
								QueryHierarchyItem newBugsQuery = null;
								if (myQueriesFolder.Children != null)
								{
										newBugsQuery = myQueriesFolder.Children.FirstOrDefault(qhi => qhi.Name.Equals(queryName));
								}
								if (newBugsQuery == null)
								{
										// if the 'REST Sample' query does not exist, create it.
										newBugsQuery = new QueryHierarchyItem()
										{
												Name = queryName,
												Wiql = "SELECT [System.Id],[System.WorkItemType],[System.Title],[System.AssignedTo],[System.State],[System.Tags] FROM WorkItems WHERE [System.TeamProject] = @project AND [System.WorkItemType] = 'User Story' AND [System.State] = 'New'",
												IsFolder = false
										};
										newBugsQuery = witClient.CreateQueryAsync(newBugsQuery, c_projectName, myQueriesFolder.Name).Result;
								}

								// run the 'REST Sample' query
								WorkItemQueryResult result = witClient.QueryByIdAsync(newBugsQuery.Id).Result;

								if (result.WorkItems.Any())
								{
										int skip = 0;
										const int batchSize = 100;
										IEnumerable<WorkItemReference> workItemRefs;
										do
										{
												workItemRefs = result.WorkItems.Skip(skip).Take(batchSize);
												if (workItemRefs.Any())
												{
														// get details for each work item in the batch
														List<WorkItem> workItems = witClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id)).Result;
														foreach (WorkItem workItem in workItems)
														{
																list.Add(new WorkItemDetails
																{
																		WiNumber = (int)workItem.Id,
																		Description = (string)workItem.Fields["System.Title"]
																});
														}
												}
												skip += batchSize;
										}
										while (workItemRefs.Count() == batchSize);
								}
								else
								{
										MessageBox.Show("No work Items Open.");
								}
						}
				}
				catch (Exception ee)
				{
						MessageBox.Show(ee.StackTrace, "Error");
				}
		}        */
	}
}