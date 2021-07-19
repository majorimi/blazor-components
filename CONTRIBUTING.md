<img align="left" width="140" height="140" src="https://github.com/majorimi/blazor-components/blob/master/.github/Images/blazor.components.png" />

# Majorsoft Blazor Components
Majorsoft Blazor Components is a set of UI Components and other useful Extensions for [Blazor](https://blazor.net) applications.
All components are available on [NuGet](https://www.nuget.org/profiles/Blazor.Components). 

## Information and Guidelines for Contributors

If you want to contribute to a project and make it better, your help is very welcome. 
Contributing is also a great way to learn more about new technologies like WASM and Balzor. 
You can also help by making constructive, helpful bug reports, feature requests and the noblest of all contributions: a good, clean pull request.

### Project structure

For better understanding of the project please check the project structure:

- `Components\Majorsoft.Blazor.Components.*`: All component projects (with UI element .razor files) are here.
- `Extensions\Majorsoft.Blazor.Extensions.*`: All non UI element projects are here.
- `_Demo\Majorsoft.Blazor.Components.DemoApp`: Published WASM application for demo purposes and uses already published Nuget packages as it would be used by a "package consumer" projects. This project is making sure all published Nuget packages are working as expected. No direct references to any other projects in this solution.
- `TestApps`: Test apps provide executable Blazor Sever hosted and WASM projects. To have quick development loops by modifying and running components immediatelly inside Blazor Server and WASM Apps without sample code duplication.
  - `TestApps\Majorsoft.Blazor.Components.TestApps.Common`:  this project **has references for all Component and Extension in the solution** and provides usage examples and other components with `\page` directive for navigation. This is **shared between WASM and Server hosted Blazor app**. With this project sample code is not duplicated.
  - `TestApps\Majorsoft.Blazor.Components.TestApp`: **Blazor WASM app using the `Common` project** and during component development can be used to make sure all components works with **Blazor WASM**.
  - `TestApps\Majorsoft.Blazor.Components.TestServerApp`: **Blazor Server Hosted application using the `Common` project** and during component development can be used to make sure all components works with **Server side Blazor**.
- `Tests`: All Unit and UI tests projects are here.

### How to submit new Issue or Feature request?

#### Please follow these guidelines when you submit a new Issue:
- First make sure the problem is reproducable, you read all docs and followed code examples and problem still persists.
- Please check if similar issue was aleardy posted, even closed ones.
- Create a new Issue where you clearly discribe the problem with all important details:
  - Which component has the problem
  - Which version of Nuget package is it
  - What is your problem 
  - Code example
  - Error message (if any)
  - Context of component tried to use

#### Please follow these guidelines when you submit a new feature/component request:
- First make sure you has the latest Nuget packages, read all docs and checked desired component or feature does not exists.
- Please check if similar request was aleardy posted, even closed ones.
- Create a new Issue where you clearly discribe what is your request:
  - Is it a new component or a feature for and existing one
  - What should the new component do or new feature should do what
  - Provide some example if possible (URL for description or existing component even for other technology)

### How to contribute in the code?

#### Prerequisites to Compile from Source
- .NET 5
- Visual Studio 2019/Visual Studio Code.

#### Code contribution steps:
1. Pull down to code-base to your local environment
2. Make sure code can be compiled
3. Create a new feature, bugfix branch locally. All your code changes should be committed to this branch
4. Post a new issue (if there is none) for your new feature or bug
5. Implement your new feature/component or fix the bug you found
6. Make sure code can be compiled, no Unit test got broken and all new code Unit tested (if possible)
7. Push your branch to Github and link it with the Issue you have been working on
8. Create a pull request from your branch targetting `master` or potential new realease version branch (TBD)
9. Wait for PR rewiev and comments (if any)
10. After all comments got resolved branche will be marged
11. Eventually your change gets compiled into Nuget packages with outer features and Published on Nuget.org
12. You can update Majorsoft Nuget packages to the latest with your feature in it.

**Thanks for all contributions!**
