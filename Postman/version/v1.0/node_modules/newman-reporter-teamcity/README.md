# newman-reporter-teamcity
A [newman](https://github.com/postmanlabs/newman) reporter for TeamCity.  See the [newman documentation](https://www.getpostman.com/docs/postman/collection_runs/command_line_integration_with_newman) for more info.

## Getting Started

1. Install `newman`
2. Install `newman-reporter-teamcity`
3. Add to your TeamCity build
4. Party! ![partyparrot](https://user-images.githubusercontent.com/109331/28225265-ab5e1a20-6897-11e7-8cd4-cc26a304daa8.gif)

### Prerequisites

1. TeamCity
2. [npm](https://www.npmjs.com/)
3. `newman`

```
npm install -g newman
```

### Installing

Install with npm

```
npm install -g newman-reporter-teamcity
```

Add a command line step to your TeamCity build with something like this.  The `-r teamcity` is the flag to enable TeamCity reporting.

```
newman run "https://www.getpostman.com/collections/<your-collection-url>" -x --delay-request 10 -r teamcity
```

The output will show up in your Build Log like this:

![TeamCity Log View](https://user-images.githubusercontent.com/109331/28225215-74855590-6897-11e7-84ae-00db6e60a2cb.PNG)

and in the Tests tab in TeamCity like this:

![TeamCity Tests View](https://user-images.githubusercontent.com/109331/28225212-70810732-6897-11e7-9b66-692dc17a0641.PNG)

