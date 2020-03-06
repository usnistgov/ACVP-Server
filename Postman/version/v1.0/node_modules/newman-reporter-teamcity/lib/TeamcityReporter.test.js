const {EventEmitter} = require('events');
const TeamcityReporter = require('./TeamcityReporter');

const reporterOptions = {};
const options = {
 collection: {name: "TestCollection"},
};
const err = undefined;
const args = {
  item: {name: "TestName", parent() { return {name: "FolderName"}; }},
};
let outputData = "";
const storeLog = inputs => (outputData += inputs + "~~~");

test('#escape() escapes special chars correctly', () => {
  const sut = new TeamcityReporter(new EventEmitter(), reporterOptions, options);
  const actual = sut.escape('Apostrophe: \', line feed: \n, carriage return: \r, Unicode: \ubabe, pipe: |, left bracket: [, right bracket: ]');
  expect(actual).toBe('Apostrophe: |\', line feed: |n, carriage return: |r, Unicode: |0xbabe, pipe: ||, left bracket: |[, right bracket: |]');
});

test('#beforeItem correctly handles tests within a folder', () => {
  const emitter = new EventEmitter();
  const reporter = new TeamcityReporter(emitter, reporterOptions, options);

  outputData = "";
  console["log"] = jest.fn(storeLog);

  emitter.emit('beforeItem', err, args);
  expect(outputData).toEqual(expect.stringMatching(/^##teamcity\[testStarted name='FolderName\/TestName' .*\]~~~$/));
});

test('#beforeItem correctly handles tests with iterations', () => {
  const emitter = new EventEmitter();
  const reporter = new TeamcityReporter(emitter, reporterOptions, options);

  outputData = "";
  console["log"] = jest.fn(storeLog);

  let args2 = args;
  args2 = {
    ...args,
    cursor: { iteration: 2, cycles: 4 },
  };

  emitter.emit('beforeItem', err, args2);
  expect(outputData).toEqual(expect.stringMatching(/^##teamcity\[testStarted name='FolderName\/TestName\/2' .*\]~~~$/));
});


test('#item reports response code of failed requests correctly', () => {
  const emitter = new EventEmitter();
  const reporter = new TeamcityReporter(emitter, reporterOptions, options);

  outputData = "";
  console["log"] = jest.fn(storeLog);

  let err2 = undefined;
  let args2 = args;
  emitter.emit('beforeItem', err2, args2);
  args2 = {
    response: {
      code: 500,
      responseTime: 20,
      reason() { return "Ineptitude"; },
    },
    assertion: "Response is success",
  };
  emitter.emit('request', err2, args2);
  err2 = true;
  emitter.emit('assertion', err2, args2);
  emitter.emit('item', err2, args2);

  expect(outputData).toEqual(expect.stringMatching(/~~~##teamcity\[testFailed name='FolderName\/TestName' .*\]~~~/));
  const details = outputData.match(/~~~##teamcity\[testFailed .* details='([^']*)' /);
  expect(details[1]).toEqual("Response is success - Response code: 500, reason: Ineptitude");
});

test('reporter exclusively emits well-formatted TC messages', () => {
  const emitter = new EventEmitter();
  const reporter = new TeamcityReporter(emitter, reporterOptions, options);
  const events = 'start beforeIteration iteration beforeItem item beforePrerequest prerequest beforeScript script beforeRequest request beforeTest test beforeAssertion assertion console exception beforeDone done'.split(' ');

  outputData = "";
  console["log"] = jest.fn(storeLog);

  events.forEach((e) => { emitter.emit(e, err, args) });
  outputData.split("~~~") // separate into individual messages
            .slice(0, -1) // drop the last element which is an empty string
            .forEach((msg) => {
              expect(msg).toEqual(expect.stringMatching(/^##teamcity\[.*\]$/));
            });
});

test('reporter handles unicode characters properly', () => {
  const emitter = new EventEmitter();
  const reporter = new TeamcityReporter(emitter, reporterOptions, options);

  outputData = "";
  console["log"] = jest.fn(storeLog);

  emitter.emit('beforeItem', err, {item: {name: "Доб", parent() {}}});
  expect(outputData).toEqual(expect.stringMatching(/^##teamcity\[testStarted name='|0x0414|0x043e|0x0431' .*\]~~~$/));
});
