# ACVP-Server

An implementation of the [Automated Cryptographic Validation Protocol (ACVP)](https://github.com/usnistgov/acvp) for NIST. This repository will be used to track deployments and issues of the Demo and Production ACVP Servers hosted by NIST. The server implementation *MAY* differ from the protocol specification. We will track those differences in this repository. Some modifications may be additional requirements on top of the protocol that are NIST specific. The protocol is intended to be general purpose for any testing body to host a compliant instance. 

## Releases

Release notes will often be posted on this repository for both the Demo and Production NIST ACVP servers. Release notes marked as "prerelease" are for the Demo server. Notes marked as "release" are for the Prod server. 

## Wiki

* See the [ACVP-Server Wiki](https://github.com/usnistgov/ACVP-Server/wiki) for information such as documentation on ACVP Server specific endpoints.
* See the [ACVP Protocol Wiki](https://github.com/usnistgov/ACVP/wiki) for information regarding protocol specific usage / FAQs.

## Issues

Please report issues found on the Demo or Prod servers on this repository. Issues that can be reported here include

* Errors generating or validating vector sets
* Questions about the server/implementation
* Questions about authentication
* Noticed differences from the protocol specifications
* Suggestions for improved tests

Questions or problems with the specifications, can be raised with issues on the protocol repository. Questions or problems with the CAVP's use of ACVP or how ACVP fits into the larger CMVP should be raised via email to a member of the CAVP. 

When creating an issue, DO NOT share any secret values used for authentication. DO NOT share a JWT, and DO NOT share a TOTP seed. 

## Disclaimer

```
“***WARNING***WARNING***WARNING
You are accessing a U.S. Government information system, which includes: 
1) this computer, 2) this computer network, 3) all computers connected 
to this network, and 4) all devices and storage media attached to this 
network or to a computer on this network. You understand and consent to 
the following: you may access this information system for authorized use 
only; you have no reasonable expectation of privacy regarding any 
communication of data transiting or stored on this information system; 
at any time and for any lawful Government purpose, the Government may 
monitor, intercept, and search and seize any communication or data 
transiting or stored on this information system; and any communications 
or data transiting or stored on this information system may be disclosed 
or used for any lawful Government purpose.
***WARNING***WARNING***WARNING”
```
