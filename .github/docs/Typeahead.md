Blazor Components Typeahead controls
============
[![Build Status](https://dev.azure.com/major-soft/GitHub/_apis/build/status/blazor-components/blazor-components-build-check)](https://dev.azure.com/major-soft/GitHub/_build/latest?definitionId=6)
[![Package Version](https://img.shields.io/nuget/v/Majorsoft.Blazor.Components.Typeahead?label=Latest%20Version)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Typeahead/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Majorsoft.Blazor.Components.Typeahead?label=Downloads)](https://www.nuget.org/packages/Majorsoft.Blazor.Components.Typeahead/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/majorimi/blazor-components/blob/master/LICENSE)

# About

Blazor component that renders an HTML `<input>` or **Blazor** provided `InputText` with Typeahead panel with optional debounce (delay) and minimal required chars. **All components work with WebAssembly and Server hosted models**. 
For code examples [see usage](https://github.com/majorimi/blazor-components/blob/master/src/Blazor.Components.TestApps.Common/Components/Typeahead.razor).

You can try it out by using the [demo app](https://blazorextensions.z6.web.core.windows.net/typeahead).

# Components
- **`DebounceInput`**: Wraps around HTML `<input>` control and adds Typeahead control with optional debounce (delay) and minimal required chars. 
- **`DebounceInput`**: Wraps around **Blazor InputText** control which enabled form validation and adds Typeahead control with optional debounce (delay) and minimal required chars.