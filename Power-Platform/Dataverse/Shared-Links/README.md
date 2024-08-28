# Getting started with shared links to table rows

This code sample demonstrates using the Microsoft Dataverse shared links messages. The shared links feature allows the caller to share a link to a table row with another user. 

More information: [Sharing and assigning](https://docs.microsoft.com/power-apps/developer/data-platform/security-sharing-assigning)

The provided code samples are listed below.

|Sample folder|Description|Build target|
|---|---|---|
|shared-links|Demonstrates enabling, creating, retrieving, and revoking a shared link.|.NET 8|

## Instructions

1. Download the solution folder from GitHub.

1. Open the *shared-links.sln* solution file in Visual Studio 2022.

1. Edit the *appsettings.json* file in the **Solution Items** folder of Solution Explorer. Set the connection string `Url` and `Username` parameters as appropriate for your test environment.

	The environment Url can be found in the Power Platform admin center. It has the form https://\<environment-name>.crm.dynamics.com.

1. Build the solution, and then run the project.

When the code sample runs, you will be prompted in the default browser to select an environment user account and enter a password. To avoid having to do this every time you run a sample, insert a password parameter into the connection string in the appsettings.json file. For example:

```json
{
"ConnectionStrings": {
    "default": "AuthType=OAuth;Url=https://myorg.crm.dynamics.com;Username=someone@myorg.onmicrosoft.com;Password=mypassword;RedirectUri=http://localhost;AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;LoginPrompt=Auto"
  }
}
```

**Tip**: You can set a user environment variable named DATAVERSE_APPSETTINGS to the file path of the appsettings.json file stored anywhere on your computer. The samples will use that appsettings file if the environment variable exists and is not null. Be sure to log out and back in again after you define the variable for it to take affect. To set an environment variable, go to **Settings > System > About**, select **Advanced system settings**, and then choose **Environment variables**. 

