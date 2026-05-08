@echo off

::rem TestApp
::xcopy .\Majorsoft.Blazor.Components.TestApps.Common\Pages .\Majorsoft.Blazor.Components.TestApp\Pages /E /Y
::
::rem TestServerApp
::xcopy .\Majorsoft.Blazor.Components.TestApps.Common\Pages .\Majorsoft.Blazor.Components.TestServerApp\Pages /E /Y

rem DemoApp
xcopy .\Majorsoft.Blazor.Components.TestApps.Common\Pages ..\demo\Majorsoft.Blazor.Components.DemoApp\Majorsoft.Blazor.Components.DemoApp.Client\Pages /E /Y
xcopy .\Majorsoft.Blazor.Components.TestApps.Common\Components ..\demo\Majorsoft.Blazor.Components.DemoApp\Majorsoft.Blazor.Components.DemoApp.Client\Components /E /Y
xcopy .\Majorsoft.Blazor.Components.TestApps.Common\Shared ..\demo\Majorsoft.Blazor.Components.DemoApp\Majorsoft.Blazor.Components.DemoApp.Client\Shared  /E /Y
rem copy .\Majorsoft.Blazor.Components.TestApps.Common\_Imports.razor ..\demo\Blazor.Components.DemoApp\_Imports.razor /Y