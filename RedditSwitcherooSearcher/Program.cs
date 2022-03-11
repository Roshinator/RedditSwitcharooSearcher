using Reddit;
using Reddit.Controllers;
using Reddit.AuthTokenRetriever;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.Generic;
using Reddit.Models;
using Reddit.Things;

class Program
{
	private const string APP_ID = "-8s9e3GlJbYKMMx3y7_2Dw";
	static RedditClient Reddit = new RedditClient();
	
	static void Main(string[] args)
	{
		string SECRET = args[0];
		string TEST_LINK = args[1];
		var REFRESH_TOKEN = AuthorizeUser(APP_ID, SECRET);
		Reddit = new RedditClient(APP_ID, REFRESH_TOKEN, SECRET);
		foreach (CommentContainer comment in GetCommentFromPermalink(TEST_LINK))
        {
			Console.WriteLine("https://reddit.com" + comment.Data.Children[0].Data.Permalink);
        }

		Console.WriteLine("Finished traversal");	
	}

	static IEnumerable<CommentContainer> GetCommentFromPermalink(string permalink)
    {
		string? nextPermalink = permalink;
		do
		{
			Match match = Regex.Match(nextPermalink, @"\/comments\/([a-z0-9]+)\/([^\/])+\/([a-z0-9]+)");
			string matchedID = match.Groups[0].Value;
			Match idmatch = Regex.Match(matchedID, @"\/comments\/([a-z0-9]+)\/");
			string postID = idmatch.Groups[1].Value;
			string commentID = match.Groups[match.Groups.Count - 1].Value;


			// Retrieve the post and return the result.  --Kris
			var x = new Reddit.Inputs.Listings.ListingsGetCommentsInput();
			x.comment = commentID;
			CommentContainer comment = Reddit.Models.Listings.GetComments(postID, x);
			yield return comment;
			string commentText = comment.Data.Children[0].Data.BodyHTML;
			Match nextMatch = Regex.Match(commentText, "href=\\\"(.+)\\\"");
			nextPermalink = nextMatch.Success ? nextMatch.Groups[1].Value : null;
		} while (nextPermalink != null);

    }


	public static string AuthorizeUser(string appId, string appSecret, int port = 8080)
	{
		// Create a new instance of the auth token retrieval library.  --Kris
		AuthTokenRetrieverLib authTokenRetrieverLib = new AuthTokenRetrieverLib(appId, port, appSecret: appSecret);

		// Start the callback listener.  --Kris
		// Note - Ignore the logging exception message if you see it.  You can use Console.Clear() after this call to get rid of it if you're running a console app.
		authTokenRetrieverLib.AwaitCallback();

		// Open the browser to the Reddit authentication page.  Once the user clicks "accept", Reddit will redirect the browser to localhost:8080, where AwaitCallback will take over.  --Kris
		OpenBrowser(authTokenRetrieverLib.AuthURL());

		while (authTokenRetrieverLib.RefreshToken == null) { }
		Thread.Sleep(1000);

		// Cleanup.  --Kris
		authTokenRetrieverLib.StopListening();

		return authTokenRetrieverLib.RefreshToken;
	}

	public static void OpenBrowser(string authUrl)
	{
		ProcessStartInfo processStartInfo = new ProcessStartInfo(authUrl);
		processStartInfo.UseShellExecute = true;
		Process.Start(processStartInfo);
	}
}