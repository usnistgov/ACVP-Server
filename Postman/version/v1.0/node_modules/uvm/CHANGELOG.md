# Postman UVM Changelog

#### v1.7.3 (May 23, 2018)
* Updated dependencies :arrow_up:

#### v1.7.2 (April 25, 2018)
* Updated dependencies :arrow_up:

#### v1.7.1 (April 6, 2018)
* :bug: Use `srcdoc` attribute in `iframe`, when available, for loading sandbox code browser environments

#### 1.7.0 (May 31, 3017)
* removed dispatch of `disconnect` event when .disconnect() is called
* add ability to remove all events when only event name is provided to `bridge.off`

#### 1.6.0 (May 30, 2017)
* add support for removal of bridge events (internal) using `bridge.off`

#### 1.5.1 (May 29, 2017)
* uvm now dispatches `disconnect` event right before disconnecting

#### 1.5.0 (March 22, 2017)
* Edge case error handling for greater stability

#### 1.4.0 (December 27, 2016)
* Delegate timers to Node VM
* :art: Unified the way code looks while delegating clear and set VM timers.

#### 1.3.0 (December 21 2016)
* Dispatch timeout support
* Finalizing external browser sandbox
* Updated the browser firmware code to return only the script and exclude the outer HTML
* Wrapped the dispatcher inside a closure to allow deletion of global variables

#### 1.3.0-beta.1 (December 20 2016)
* Ensured that dispatched messages are read only by intended listeners
* Abandoned the whole idea of escaping the dispatch and instead setting it as string in context
* Added additional character escaping (thinking of doing base64, but that would be slow)
* Rename bootcode parameter to camel Case
* Added bootTimeout feature on node bridge. Not possible in browser bridge
* Circular JSON support
* Setting the interface __uvm_* variables to null instead of deleting it. Also wrapping bridge-client to keep CircularJSON inside closure
* Ensure that CircularJSON dependency is deleted accurately by removing the `var` statement
* Restored the previously modified loopback test spec and ensured that the new circular-son tests use a different event name
* Fixed an issue where CircularJSON was left running amock in globals scope
* Temporarily modified the tests to allow multi-window tests as window.postMessage is bleeding
* Modified tests to ensure cyclic objects are going through
* Replaced all JSON parse and stringing with their circular counterpart

#### 1.2.0 (November 28 2016)
* Added more globals to the list of protected globals
* Updated the bridges to now accept emits as string (thus requiring to do JSON.parse)

#### 1.1.0 (November 28 2016)
* Make the dispatch functions be resilient to deletion of bridge from global
* Updated dependencies

#### 1.0.0 (November 27 2016)
* Initial release
* Added stub code with config and tests
* Migrated first batch of release code
