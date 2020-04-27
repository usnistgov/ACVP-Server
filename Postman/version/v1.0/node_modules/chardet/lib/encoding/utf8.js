"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var match = require('../match').default;
class Utf8 {
    name() {
        return 'UTF-8';
    }
    match(det) {
        var hasBOM = false, numValid = 0, numInvalid = 0, input = det.fRawInput, trailBytes = 0, confidence;
        if (det.fRawLength >= 3 &&
            (input[0] & 0xff) == 0xef &&
            (input[1] & 0xff) == 0xbb &&
            (input[2] & 0xff) == 0xbf) {
            hasBOM = true;
        }
        for (var i = 0; i < det.fRawLength; i++) {
            var b = input[i];
            if ((b & 0x80) == 0)
                continue;
            if ((b & 0x0e0) == 0x0c0) {
                trailBytes = 1;
            }
            else if ((b & 0x0f0) == 0x0e0) {
                trailBytes = 2;
            }
            else if ((b & 0x0f8) == 0xf0) {
                trailBytes = 3;
            }
            else {
                numInvalid++;
                if (numInvalid > 5)
                    break;
                trailBytes = 0;
            }
            for (;;) {
                i++;
                if (i >= det.fRawLength)
                    break;
                if ((input[i] & 0xc0) != 0x080) {
                    numInvalid++;
                    break;
                }
                if (--trailBytes == 0) {
                    numValid++;
                    break;
                }
            }
        }
        confidence = 0;
        if (hasBOM && numInvalid == 0)
            confidence = 100;
        else if (hasBOM && numValid > numInvalid * 10)
            confidence = 80;
        else if (numValid > 3 && numInvalid == 0)
            confidence = 100;
        else if (numValid > 0 && numInvalid == 0)
            confidence = 80;
        else if (numValid == 0 && numInvalid == 0)
            confidence = 10;
        else if (numValid > numInvalid * 10)
            confidence = 25;
        else
            return null;
        return match(det, this, confidence);
    }
}
exports.default = Utf8;
//# sourceMappingURL=utf8.js.map