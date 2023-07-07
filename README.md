# DotNetNuke.Services.Analytics
This library overrides or extendes behaviors from DNN analytics providers such as the Google Analytics provider.

## Extended behaviors
The class `GoogleAnalyticsEngineWithRoles` inherits the `GoogleAnalyticsEngine` class from the DotNetNuke library, to allow to exclude traffic from users that are in specific roles. Since the current implementation only allows to exclude traffic from Administrators, this extension allows to also exclude for other non-admin users. This is useful when you don't want to track traffic from your internal company users, but want to do it for the rest of the users.

### Installation
1. Download the .zip file from the releases section
2. Unzip the file and drop it to the \bin folder of your DNN installation
3. Setup your Google Analytics connector through the DNN user interface
4. Edit the \Portals\0\GoogleAnalytics.config file ("0" is the ID of your portal) and add this setting, with the roles you want to exclude comma separated (i.e. "My Role 1,My Role 2")
```xml
<?xml version="1.0" encoding="utf-8"?>
<AnalyticsConfig xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Settings>
    ...
    <AnalyticsSetting>
      <SettingName>OtherUntrackedRoles</SettingName>
      <SettingValue>My Role 1,My Role 2</SettingValue>
    </AnalyticsSetting>	
  </Settings>
</AnalyticsConfig>
```
5. Edit the \SiteAnalytics.config and replace the engine type with this value:
```xml
<EngineType>DotNetNuke.Services.Analytics.GoogleAnalyticsEngineWithRoles, DotNetNuke.Services.Analytics</EngineType>
```

### Known issues
* If you update the Google Analytics connector settings through the DNN user interface, you need to manually add again the `OtherUntrackedRoles` setting by repeating the step #4 mentioned above
