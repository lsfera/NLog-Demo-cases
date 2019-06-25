For https://stackoverflow.com/questions/56743110/get-nlog-values-from-appsettings-json

results in:

```
2019-06-25 23:24:41.3640||INFO|AspNetCore2WithConfigSetting.Controllers.HomeController|Hi from home index|ConnectionStrings4=a connectionString |url: https://localhost/|action: Index
```

in 

"C:\temp\nlog-own-2019-06-25.log"

appsettings.json:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Trace",
      "Microsoft": "Information"
    }
  },
  "AllowedHosts": "*",
  "Mode": "testMe",
  "ConnectionStrings": {
    "ApplicationDatabase": "a connectionString"
  },
  "DatabaseCredentials": {
    "User": "",
    "Password": ""
  }
}
```

nlog.config

```
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      internalLogLevel="Info"
      internalLogFile="c:\temp\internal-nlog.txt">

  <!-- enable asp.net core layout renderers -->
  <extensions>
    <add assembly="NLog.Web.AspNetCore"/>
  </extensions>

  <!-- the targets to write to -->
  <targets>
    <!-- write logs to file  -->
    <target xsi:type="File" name="allfile" fileName="c:\temp\nlog-all-${shortdate}.log"
            layout="${longdate}|${event-properties:item=EventId_Id}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}" />

    <!-- another file log, only own logs. Uses some ASP.NET core renderers -->
    <target xsi:type="File" name="ownFile-web" fileName="c:\temp\nlog-own-${shortdate}.log"
            layout="${longdate}|${event-properties:item=EventId_Id}|${uppercase:${level}}|${logger}|${message}|ConnectionStrings4=${configsetting:name=ConnectionStrings.ApplicationDatabase} ${exception:format=tostring}|url: ${aspnet-request-url}|action: ${aspnet-mvc-action}" />
  </targets>

  <!-- rules to map from logger name to target -->
  <rules>
   
    <!--Skip non-critical Microsoft logs and so log only own logs-->
    <logger name="Microsoft.*" maxlevel="Info" final="true" /> <!-- BlackHole without writeTo -->
    <logger name="*" minlevel="Trace" writeTo="ownFile-web" />
  </rules>
</nlog>
```
