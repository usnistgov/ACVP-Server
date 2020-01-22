# Postman Collection Transformer Changelog

#### v2.6.2 (August 6, 2018)
* #111 Updated `url.parse` to correctly account for path variables

#### v2.6.1 (July 24, 2018)
* #110 Prevent folder id replacement in v2.x -> v1 transformations :bug:
* #109 Increased test coveragee to 95% :arrow_up:

#### v2.6.0 (July 10, 2018)
* #107 Added support for `retainEmptyValues` in **all** flows :tada:.

#### v2.5.10 (July 4, 2018)
* #106 Updated dependencies :arrow_up:
* #105 Fixed a bug in url parsing protocol extraction :bug:

#### v2.5.9 (April 30, 2018)
* #103 Fixed `retainIds` behaviour for v1 to v2 conversions :bug:
* #104 Updated dependencies :arrow_up:

#### v2.5.8 (March 13, 2018)
* :arrow_up: Updated dependencies
* #102 Improved handling for `null` valued query parameters in transformations from `v1`

#### v2.5.7 (March 1, 2018)
* #101 Fixed text `type` mapping for `auth` and `variable` fields. :bug:

#### v2.5.6 (February 23, 2018)
* #100 Auth mapping improvements

#### v2.5.5 (February 6, 2018)
* :arrow_up: Updated dependencies
* :bug: Fixed empty description bug for v1 -> v2 transformations #99

#### v2.5.4 (January 7, 2018)
* :arrow_up: Updated dependencies
* #98 Improved `script.exec` type safety in v1 normalization :bug:

#### v2.5.3 (December 18, 2017)
* #97 Fixed a bug that caused query params to be duplicted :bug:

#### v2.5.2 (December 12, 2017)
* #96 Made empty description pruning optional with `retainEmptyValues` :bug:

#### v2.5.1 (December 1, 2017)
* #95 Improved type safety in folder and request transformations :bug:

#### v2.5.0 (November 22, 2017)
* #94 Added support for the `prioritizeV2` option to v1 normalization and v1 -> v2.x conversions. :tada:

#### v2.4.3 (November 20, 2017)
* #92 Fixed legacy property handling for non-complete event normalization :bug:

#### v2.4.2 (November 20, 2017)
* #91 Fixed default auth state derivation logic :bug:

#### v2.4.1 (November 18, 2017)
* :arrow_up: Updated dependencies #88
* :bug: Fixed empty stage handling logic for auth helpers #90
* :art: Reorganized package structure #89

#### v2.4.0 (November 7, 2017)
* #87 Tested code on Node v8 :tada:
* #84 Added support for `noDefaults` for v1 normalizations :tada:
* #82 `noauth` and `normal` auth handlers are now left alone :bug:
* #81 Added support for `mutate` to v1 normalizations :tada:
* #79 Added `normalize.*` function to sanitize v1 collections. :tada:
* #78 Added support for inherited entity transformations :tada:

#### v2.3.1 (October 3, 2017)
* :bug: Corrected OAuth2 param transformations #77

#### v2.3.0 (September 30, 2017)
* :tada: Added support for NTLM and Bearer Token auth transformations #76

#### v2.2.1 (September 28, 2017)
* Restored support for string-object hybrid v2 URLs #75

#### v2.2.0 (September 4, 2017)
* :tada: Added support for transformations to and from the `v2.1.0` format. #66, #67, #68
* :arrow_up: Updated dependencies #65
* :bug: Enforced an object structure for V2 collection request URLs. #62
* :bug: Prevented empty descriptions from showing up across transformations. #61

#### 2.1.5 (June 28, 2017)
* Allowed both: `key` and `id` as identifers for path variables in v2 collections #60

#### 2.1.4 (June 19, 2017)
* No code changes

#### 2.1.3 (June 09, 2017)
* Fixed a bug in the URL parse implementation, by copying the parser & unparser from the sdk

#### 2.1.2 (May 25, 2017)
* Handle an edge case where an unhandled ID is provided in the `requestObject`

#### 2.1.1 (May 22, 2017)
* Fixed a bug where the `requestObject` was missing when doing v2 to v1 conversions
* Added the ability to convert single responses as well

#### 2.1.0 (May 12, 2017)
* Descriptions for headers, url query params, url path variables, and request body data are now handled in transformations.
* Fix disabled: true header (`v2`) <-> // (commented) header (`v1`) transformations.
* Fixed bug where both enabled and disabled would appear in transformed entities.
* Request body transformations now follow `v1` - `v2` compliance
* Removed unwanted `enabled: true` properties from query params, headers, and request body data
