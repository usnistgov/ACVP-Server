# newman-reporter-html
HTML reporter for [Newman](https://github.com/postmanlabs/newman) that provides the information about the collection run in HTML format.
This needs to be used in conjunction with Newman so that it can recognize HTML reporting options.

## Install
> The installation should be global if newman is installed globally, local otherwise. (Replace -g from the command below with -S for a local installation)

```console
$ npm install -g newman-reporter-html
```

## Usage
In order to enable this reporter, specify `html` in Newman's `-r` or `--reporters` option.

```console
$ newman run https://www.getpostman.com/collections/631643-f695cab7-6878-eb55-7943-ad88e1ccfd65-JsLv -r html
```

### Options

#### With Newman CLI

| CLI Option  | Description       |
|-------------|-------------------|
| `--reporter-html-export <path>` | Specify a path where the output HTML file will be written to disk. If not specified, the file will be written to `newman/` in the current working directory. |
| `--reporter-html-template <path>` | Specify a path to the custom template which will be used to render the HTML report. This option depends on `--reporter html` and `--reporter-html-export` being present in the run command. If this option is not specified, the [default template](https://github.com/postmanlabs/newman-reporter-html/blob/develop/lib/template-default.hbs) is used |

Custom templates (currently handlebars only) can be passed to the HTML reporter via `--reporter-html-template <path>` with `--reporters html` and `--reporter-html-export`.
The [default template](https://github.com/postmanlabs/newman-reporter-html/blob/develop/lib/template-default.hbs) is used in all other cases.

#### With Newman as a Library
The CLI functionality is available for programmatic use as well.

```javascript
const newman = require('newman');

newman.run({
    collection: require('./examples/sample-collection.json'), // can also provide a URL or path to a local JSON file.
    reporters: 'html',
    reporter: {
        html: {
            export: './htmlResults.html', // If not specified, the file will be written to `newman/` in the current working directory.
            template: './customTemplate.hbs' // optional, this will be picked up relative to the directory that Newman runs in.
        }
    }
}, function (err) {
	if (err) { throw err; }
    console.log('collection run complete!');
});
```

## Compatibility

| **newman-reporter-html** | **newman** | **node** |
|:------------------------:|:----------:|:--------:|
|          v0.1.0          | >= v3.10.0 | >= v6.x  |
|          v1.0.0          | >= v4.0.0  | >= v6.x  |

## Troubleshooting

### Reporter not found
The reporter and newman must be installed at the same level, the installation should be global if newman is installed globally, local otherwise.

### Getting different HTML output
You are most probably getting in-built reporter output used in older versions of newman, Please check the newman's [compatibility](#compatibility) section above.

> If you are facing any other problem, please check the open [issues](https://github.com/postmanlabs/newman-reporter-html/issues) or create new.


## Community Support

<img src="https://avatars1.githubusercontent.com/u/3220138?v=3&s=120" align="right" />
If you are interested in talking to the Postman team and fellow Newman users, you can find us on our <a href="https://community.getpostman.com">Postman Community Forum</a>. Feel free to drop by and say hello. You'll find us posting about upcoming features and beta releases, answering technical support questions, and contemplating world peace.

Sign in using your Postman account to participate in the discussions and don't forget to take advantage of the <a href="https://community.getpostman.com/search?q=newman">search bar</a> - the answer to your question might already be waiting for you! Don’t want to log in? Then lurk on the sidelines and absorb all the knowledge.


## License
This software is licensed under Apache-2.0. Copyright Postdot Technologies, Inc. See the [LICENSE.md](LICENSE.md) file for more information.
