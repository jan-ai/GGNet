[![License](https://img.shields.io/github/license/BlazorExtensions/Storage.svg?longCache=true&style=flat-square)](https://github.com/pablofrommars/GGNet/blob/master/LICENSE.TXT)
[![Package Version](https://img.shields.io/badge/nuget-v2.2.0-blue.svg?longCache=true&style=flat-square)](https://www.nuget.org/packages/Twins.Blazor.GGNet/2.2.0)
# GG.Net Data Visualization

GG.Net lets Data Scientists and Developers create interactive and flexible charts for .NET and [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) Web Apps.

Taking its inspiration from the highly popular [ggpplot2](https://ggplot2.tidyverse.org) R package, GG.Net provides natively rich features for your Data Analysis Workflow. Build publication quality charts with just a few lines of code in C# and F#.

[Learn more about GG.Net](https://pablofrommars.github.io/)

### Examples Gallery

![visitors](https://user-images.githubusercontent.com/52295393/103322181-774e1b00-4a3d-11eb-901b-62279e113ecb.gif) 

| | | |
|-|-|-|
![image](https://user-images.githubusercontent.com/52295393/103013648-ca0b6c80-453d-11eb-9370-50da64656f97.png) | ![image](https://user-images.githubusercontent.com/52295393/103013693-d8f21f00-453d-11eb-9124-44d3d98b65db.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/scatterplot.png)
![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/bubbleplot.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/barchart.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/candlestick.png)
![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/linechart.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/areachart.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/barplot.png) 
![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/stacked.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/hbarplot.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/lolipop.png) 
![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/errorbar.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/violin.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/hex.png) 
![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/ridgeline.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/choropleth.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/sparkline.png) 
![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/CFR.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/abline.png) | ![](https://github.com/pablofrommars/GGNet.Site/blob/master/wwwroot/img/bubblemap.png)

### Release Notes

#### Version 2.2
* Added support for updateable limits and expand limits on refresh of chart via XLim/YLim and XExpandLim/YExpandLim
* Added DateTimePosition axis via Scale_X_Continuous() using type of DateTime, which enables support for expand limits on dates and time values
* Added demo for a self-updating chart of (web site) visitors statistic using DateTimePosition and dynamic Lim & ExpandLim
* Fixed: axis layout was not updated on refresh

#### Version 2.1
* Added support for expand limits as optional parameter to geoms to have a certain range of axis even data is not available for the complete range

##### Version 2.0
* Added support for coordinates systems (currently cartesian, flip cartesian and polar)
* Added demo for donut and pie chart via polar coordinates
