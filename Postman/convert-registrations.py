import os

rootdir = 'C:\\Users\\ctc\\Documents\\Postman Tests\\new-registrations'
front_tag = "[{\"algorithms\": \""
back_tag = "\"}]"

excludeFromCamelCasing = [
    "SHA1"
    "SHA2-224",
    "SHA2-256",
    "SHA2-384",
    "SHA2-512",
    "SHA2-512/224",
    "SHA2-512/256",
    "SHA3-224",
    "SHA3-256",
    "SHA3-384",
    "SHA3-512",
    "AES-CCM",
    "TDES",
    "CMAC",
    "SHA2-224",
    "SHA2-256",
    "SHA2-384",
    "SHA2-512",
    "SHA2-512/224",
    "SHA2-512/256",
    "SHA3-224",
    "SHA3-256",
    "SHA3-384",
    "SHA3-512",
    "HMAC-SHA2-224",
    "HMAC-SHA2-256",
    "HMAC-SHA2-384",
    "HMAC-SHA2-512",
    "HMAC-SHA2-512/224",
    "HMAC-SHA2-512/256",
    "HMAC-SHA3-224",
    "HMAC-SHA3-256",
    "HMAC-SHA3-384",
    "HMAC-SHA3-512",
]

def fixCamelCasingProperties(line):
    testLine = line.strip()
    # this is gross, don't judge me
    lineParts = testLine.split('"', 2)

    # if split has more than one item, should be working with key/value pair
    if len(lineParts) > 1:
        newLineParts = []
        iterator = 0
        for part in lineParts:
            # the second index, first character should be the character that needs lowering
            if iterator == 1:
                # exclude specific items from camelcasing
                # like "HMAC-SHA2-", or "CMAC", etc...
                if any(part in s for s in excludeFromCamelCasing):
                    return line.upper()

                # TODO "PQGen" won't be formatted properly, special case lookup as they come up?

                newLineParts.append('"' + part[0].lower() + part[1:] + '"')
            else:
                newLineParts.append(part)
            iterator += 1

        # put it all back together
        newLine = ''
        for part in newLineParts:
            newLine = newLine + part

        return newLine
    else:
        return line

for subdir, dirs, files in os.walk(rootdir):
    for file in files:
        path = rootdir + "\\" + file
        print(path)
        f = open(path, "r+")

        content = ""
        line = f.readline()
        while line:
            line = fixCamelCasingProperties(line)
            line = line.replace('\n', '')
            line = line.replace('\t', '')
            line = line.replace(' ', '')
            line = line.replace('\"', '\\\"')
            if "isSample" not in line:
                content += line
            line = f.readline()

        content = front_tag + content + back_tag

        print(content)
        f.close()

        g = open(rootdir + "\\..\\" + "formatted-registrations" + "\\" + file, "w+")
        g.write(content)
        g.close()