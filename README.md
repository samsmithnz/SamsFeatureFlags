# Sams Feature Flags
As part of a [series of blog posts about Feature Flags](https://samlearnsazure.blog/2019/09/13/implementing-feature-flags/), we decided to make our own custom Feature flags system. 
Our main application will call our feature flags service, asking for the state of particular feature flags based on their environment – for example, if we were implementing a new menu and wanted to use the feature flag in dev, the dev website will ask if the dev version of the new menu feature flag is enabled. Current features include 
- A REST API service to retrieve the state of the feature flag
- Tracking to record the total number of uses and last date/time the feature flag was used
<kbd><img src="https://samlearnsazure.files.wordpress.com/2019/09/23samsappfeatureflags-2.png?w=1160" style="border: 1px solid black" /></kbd>

# Build and Test
Uses dotnet core 2.2 and ms test v2. A GitHub action currently builds the code and runs the tests. 
![](https://github.com/samsmithnz/FeatureFlags/workflows/Feature%20Flags%20CI%2FCD/badge.svg

# Contribute
Feel free to fork and/or add any relevant feature suggestions, bug reports, or features!  
