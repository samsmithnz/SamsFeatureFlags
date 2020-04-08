# Sams Feature Flags
As part of a [series of blog posts about Feature Flags](https://samlearnsazure.blog/2019/09/13/implementing-feature-flags/), we needed to create our own custom Feature flags system. 
Our main application will call our feature flags service, asking for the state of particular feature flags based on their environment – for example, if we were implementing a new menu and wanted to use the feature flag in dev, the dev website will ask if the dev version of the new menu feature flag is enabled. Current features include 
- A REST API service to retrieve the state of the feature flag
- A simple website to toggle the feature flags
- Automated unit, integration and functional tests
- Tracking to record the total number of uses and last date/time the feature flag was used
<kbd><img src="https://samlearnsazure.files.wordpress.com/2019/09/23samsappfeatureflags-2.png?w=1160" style="border: 1px solid black" /></kbd>

# Build and Test
Uses .Net CORE 3.1, MSTest, and Selenium. A GitHub action runs the CI/CD process. 

[![Build](https://github.com/samsmithnz/FeatureFlags/workflows/Feature%20Flags%20CI%2FCD/badge.svg)](https://github.com/samsmithnz/SamsFeatureFlags/actions?query=workflow%3A%22Feature+Flags+CI%2FCD%22)

Currently the CI/CD process: 
1. builds the code
2. runs the unit tests
3. deploys the web service and website to a web app staging slot
4. runs Selenium smoke tests on the staging slot to ensure the project is working as expected
5. swaps the staging and production slots


# Contribute
Feel free to fork and/or add any relevant feature suggestions, bug reports, or features!  
