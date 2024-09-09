using Microsoft.Extensions.Configuration;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using ResponsiveWorkManager.DataObjects;
using System.Windows;

namespace ResponsiveWorkManager.Connection
{
	public static class VSSConnection
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

		public static List<string> GetProjects()
		{
			ProjectHttpClient projectClient = VSSConnection.Connection.GetClient<ProjectHttpClient>();
			IEnumerable<TeamProjectReference> projectsInAzure = projectClient.GetProjects().Result;
			List<string> projectNames = projectsInAzure.Select(p => p.Name).ToList();
			return projectNames;
		}

		public static List<string> GetPullRequests()
		{
			List<string> pullRequestTitles = new List<string>();

			// Create an instance of GitHttpClient using VssConnection
			GitHttpClient gitClient = VSSConnection.Connection.GetClient<GitHttpClient>();

			ProjectHttpClient projectClient = VSSConnection.Connection.GetClient<ProjectHttpClient>();
			IEnumerable<TeamProjectReference> projectsInAzure = projectClient.GetProjects().Result;
			List<string> projectNames = projectsInAzure.Select(p => p.Name).ToList();


			foreach (var project in projectNames)
			{
				// Get all repositories in the project
				List<GitRepository> repositories = gitClient.GetRepositoriesAsync(project).Result;

				// Get all pull requests
				foreach (var repository in repositories)
				{
					GitPullRequestSearchCriteria searchCriteria = new GitPullRequestSearchCriteria()
					{
						Status = PullRequestStatus.Active,
						TargetRefName = "refs/heads/master",
						SourceRefName = "refs/heads/develop",
						RepositoryId = repository.Id,
						CreatorId = Guid.Empty
					};

					List<GitPullRequest> pullRequests = gitClient.GetPullRequestsAsync(project, repository.Id, searchCriteria).Result;

					// Extract the titles of the pull requests
					foreach (var pullRequest in pullRequests)
					{
						pullRequestTitles.Add(pullRequest.Title);
					}
				}
			}

			return pullRequestTitles;
		}

		private static WorkItemTrackingHttpClient GetMyQueriesFromProject(string ProjectName, out QueryHierarchyItem myQueriesFolder)
		{
			// Create instance of WorkItemTrackingHttpClient using VssConnection
			WorkItemTrackingHttpClient witClient = VSSConnection.Connection.GetClient<WorkItemTrackingHttpClient>();

			// Get 2 levels of query hierarchy items
			List<QueryHierarchyItem> queryHierarchyItems = witClient.GetQueriesAsync(ProjectName, depth: 2).Result;

			// Search for 'My Queries' folder
			myQueriesFolder = queryHierarchyItems.FirstOrDefault(qhi => qhi.Name.Equals("My Queries"));

			return witClient;
		}

		public static List<Queries> GetQueries(string ProjectName)
		{
			List<Queries> savedQueries = new List<Queries>();
			QueryHierarchyItem myQueriesFolder;
			GetMyQueriesFromProject(ProjectName, out myQueriesFolder);

			if (myQueriesFolder != null)
			{
				if (myQueriesFolder.Children != null)
				{
					foreach (var child in myQueriesFolder.Children.OrderBy(x => x.Name))
					{
						savedQueries.Add(new Queries { QueryText = child.Name });
					}
				}
			}
			if (savedQueries.Count > 0)
			{
				return savedQueries.OrderBy(x => x.QueryText).ToList();
			}
			return null;
		}

		public static List<DataObjects.WorkItemDetails> GetWorkItems(string selectedQuery, string projectText)
		{
			try
			{
				if (string.IsNullOrEmpty(selectedQuery)) return null;
				List<DataObjects.WorkItemDetails> workItemDetails = new List<DataObjects.WorkItemDetails>();

				QueryHierarchyItem myQueriesFolder;
				WorkItemTrackingHttpClient witClient = GetMyQueriesFromProject(projectText, out myQueriesFolder);

				if (myQueriesFolder != null)
				{
					// See if our 'Open WI' query already exists under 'My Queries' folder.
					QueryHierarchyItem newBugsQuery = null;
					if (myQueriesFolder.Children != null)
					{
						newBugsQuery = myQueriesFolder.Children.FirstOrDefault(qhi => qhi.Name.Equals(selectedQuery));
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
								List<Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem> workItems = witClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id)).Result;
								foreach (var workItem in workItems)
								{
									workItemDetails.Add(new DataObjects.WorkItemDetails
									{
										WiNumber = (int)workItem.Id,
										Description = (string)workItem.Fields["System.Title"],
										State = (string)workItem.Fields["System.State"],
										AssignedTo = GetAssignedTo(workItem),
										Tags = GetTagValues(workItem),
										WiType = GetWorkItemType(workItem)
									});
								}
							}
							skip += batchSize;
						}
						while (workItemRefs.Count() == batchSize);

						return workItemDetails;
					}
					else
					{
						return null;
					}
				}

				return null;
			}
			catch (Exception ee)
			{
				MessageBox.Show(ee.StackTrace, "Error");
				return null;
			}
		}

		private static string GetAssignedTo(Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem workItem)
		{
			object identityOjbect;
			workItem.Fields.TryGetValue("System.AssignedTo", out identityOjbect);
			if (identityOjbect == null)
			{
				return "Unassigned";
			}

			return ((IdentityRef)identityOjbect).DisplayName;
		}

		private static string GetTagValues(Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem workItem)
		{
			object identityOjbect;
			workItem.Fields.TryGetValue("System.Tags", out identityOjbect);

			if (identityOjbect == null)
			{
				return string.Empty;
			}

			return (string)identityOjbect;
		}

		private static string GetWorkItemType(Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem workItem)
		{
			object identityOjbect;
			workItem.Fields.TryGetValue("System.WorkItemType", out identityOjbect);

			if (identityOjbect == null)
			{
				return string.Empty;
			}

			return (string)identityOjbect;
		}
	}
}
