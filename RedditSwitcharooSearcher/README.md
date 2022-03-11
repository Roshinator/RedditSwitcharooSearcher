# Reddit Switcharoo Searcher
This program will traverse a reddit "ol switcharoo" link chain.

## Steps to run
1. Make sure you have [.NET SDK](https://dotnet.microsoft.com/en-us/download) installed
1. Go to https://reddit.com/prefs/apps
3. Create an app
    - Select script
    - Make the redirect URI `http://127.0.0.1:8080/Reddit.NET/oauthRedirect`
4. After creating it, click "edit" and note the app id under where it says "personal use script" and the secret.
5. Clone this repository and navigate to the RedditSwitcharooSearcher folder in your terminal.
6. Run `dotnet run [APP ID] [SECRET] [LINK TO SWITCHAROO COMMENT]`
7. Let it run. It will only get 60 comment links a minute, so there may be idle periods. It is finished when it prints `Finished traversal`.