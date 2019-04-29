## Extent Framework 4 - Community Edition - .Net Core / .Net Standard

[![NuGet](https://img.shields.io/badge/extentreports--core-1.02-blue.svg)](https://www.nuget.org/packages/ExtentReports.Core)
[![Join the chat at https://gitter.im/anshooarora/extentreports-csharp](https://badges.gitter.im/anshooarora/extentreports-csharp.svg)](https://gitter.im/anshooarora/extentreports-csharp?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/8d4e66d07b9e4ebca7cef7c5b5eb7ba2)](https://www.codacy.com/app/anshooarora/extentreports-csharp?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=extent-framework/extentreports-csharp&amp;utm_campaign=Badge_Grade)
![](https://img.shields.io/github/license/extent-framework/extentreports-csharp.svg?style=plastic)

.NET Framework 4.5+
.NET Core 2.1+
.NET Standard 1.5+

### Documentation

This is a modified version of the popular test reporting framework ExtentReports, which supports now all current versions of .NET Frameworks including .NET Core and .NET Standard framework. Now you can use it in all types of your .NET projects.

#### Usage:

``` c# 
public AventStack.ExtentReports.ExtentReports Reporter =  new AventStack.ExtentReports.ExtentReports();
```

*Hint:* Due to the overlapping naming of the namespace and the class ExtentReports, .NET Core compiler shows an error
'ExtentReports' is a namespace but is used like a type'. You can get rid of this issue using full qualified names like above or using a class alias.


Try it out and enjoy, your SimplyTest team.

View [extentreports.com](http://extentreports.com/docs/versions/4/net/) for complete documentation.

### License

Apache-2.0
