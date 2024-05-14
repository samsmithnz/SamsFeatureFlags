# Sams Feature Flags

[![Build](https://github.com/samsmithnz/FeatureFlags/workflows/Feature%20Flags%20CI%2FCD/badge.svg)](https://github.com/samsmithnz/SamsFeatureFlags/actions?query=workflow%3A%22Feature+Flags+CI%2FCD%22) 
[![Coverage Status](https://coveralls.io/repos/github/samsmithnz/SamsFeatureFlags/badge.svg?branch=main)](https://coveralls.io/github/samsmithnz/SamsFeatureFlags?branch=main)

As part of a [series of blog posts about Feature Flags](https://samlearnsazure.blog/2019/09/13/implementing-feature-flags/), we needed to create our own custom Feature flags system. 
Our main application will call our feature flags service, asking for the state of particular feature flags based on their environment – for example, if we were implementing a new menu and wanted to use the feature flag in dev, the dev website will ask if the dev version of the new menu feature flag is enabled. Current features include 
- A REST API service to retrieve the state of the feature flag
- A simple website to toggle the feature flags
- Automated unit, integration and functional tests
- Tracking to record the total number of uses and last date/time the feature flag was used
<kbd><img src="https://samlearnsazure.files.wordpress.com/2019/09/23samsappfeatureflags-2.png?w=1160" style="border: 1px solid black" /></kbd>

## GitHub OAuth Setup

To enable login with GitHub account, you need to set up GitHub OAuth credentials. Follow these steps:

1. Go to your GitHub account settings.
2. Navigate to Developer settings > OAuth Apps.
3. Click on the "New OAuth App" button.
4. Fill in the application name, homepage URL, and the authorization callback URL as specified by your application requirements.
5. Once the application is registered, GitHub will provide a `Client ID` and a `Client Secret`. Keep these credentials secure.

### Configuring the credentials in your project

Add the following settings to your `appsettings.json` file in the `FeatureFlags.Web` project:

```json
"GitHubOAuth": {
  "ClientId": "YourClientIdHere",
  "ClientSecret": "YourClientSecretHere"
}
```

Replace `YourClientIdHere` and `YourClientSecretHere` with the credentials provided by GitHub.

# Build and Test
Uses .NET 6, MSTest, and Selenium. A GitHub action runs the CI/CD process. 

Currently the CI/CD process: 
1. builds the code
2. runs the unit tests
3. deploys the web service and website to a web app staging slot
4. runs Selenium smoke tests on the staging slot to ensure the project is working as expected
5. swaps the staging and production slots

Dependabot runs daily to check for dependency upgrades, and will automatically create a pull request, and approve/close it if all of the tests pass successfully 

# Contribute
Feel free to fork and/or add any relevant feature suggestions, bug reports, or features!  
