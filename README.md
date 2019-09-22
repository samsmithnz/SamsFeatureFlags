![](https://github.com/samsmithnz/FeatureFlags/workflows/Feature%20Flags%20CI%2FCD/badge.svg

# Introduction 
As part of a [series of blog posts about Feature Flags](https://samlearnsazure.blog/2019/09/13/implementing-feature-flags/), we decided to make our own custom Feature flags system. 
Our main application will call our feature flags service, asking for the state of particular feature flags based on their environment – for example, if we were implementing a new menu and wanted to use the feature flag in dev, the dev website will ask if the dev version of the new menu feature flag is enabled.

# Build and Test
Uses dotnet core 2.2 and ms test v2. A GitHub action currently builds the code and runs the tests. 
![](https://github.com/samsmithnz/FeatureFlags/workflows/Feature%20Flags%20CI%2FCD/badge.svg

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# Contribute
Feel free to fork and/or add any relevant features!  
