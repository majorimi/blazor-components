---
name: verify
description: Build, launch and drive this repo's demo apps to verify Blazor component changes end-to-end in a real browser.
---

# Verifying blazor-components changes

Component demos live in `src/Majorsoft.Blazor.Components.TestApps.Common/Components/*Demo.razor`,
routed by `Pages/*.razor` (e.g. `/media`, `/canvas`). The Blazor Server host is the easiest handle:

```powershell
# from src/
$env:ASPNETCORE_ENVIRONMENT='Development'; $env:ASPNETCORE_URLS='http://localhost:5241'
dotnet run --project Majorsoft.Blazor.Components.TestServerApp\Majorsoft.Blazor.Components.TestServerApp.csproj --no-launch-profile
```

Wait for `http://localhost:5241/<page>` to return 200 (~15-30s incl. build).

Drive it with Playwright from Node (`npx playwright --version` is available globally; if the
`playwright` module is missing, `npm install playwright` in a scratch dir — browsers are already installed).

Gotchas:
- Camera/microphone flows: launch Chromium with
  `--use-fake-device-for-media-stream --use-fake-ui-for-media-stream --autoplay-policy=no-user-gesture-required`
  and grant `permissions: ["microphone", "camera"]` on the context. The fake mic produces a tone,
  so audio level meters / FFT visualizers must show non-zero data.
- Canvas-rendering components: assert on pixels via `ctx.getImageData(...)` (count non-background pixels),
  not on markup.
- Demo sections repeat button captions (camera and audio sections both have "⏺ Record" / "Stop recording"):
  scope locators to the section container, e.g. `page.locator(".container-fluid").nth(1)`.
- New `AddX()` DI extensions must be registered in all host apps (TestServerApp `Program.cs` etc.)
  or the page throws on inject.
