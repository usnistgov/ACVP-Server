# Web.Public

Hosts the ACVP public API, description of API found at: https://usnistgov.github.io/ACVP/

## First time setup

* Set up database structure with SSDT
* Set JWT signing key within the ACVPPublic database
  * `exec [external].[SecretKeyValuePairSet] 'jwtSigningKey', 'some long and secret value to use for signing the JWT'`
* Set up a local CA, trust it, create a client cert signed by that CA, and update your appsettings.local.json with your "MtlsConfig:CaSubjectKeyId" which should be the Subject Key Identifier from the root CA. See !452 for more details.
  * See https://deliciousbrains.com/ssl-certificate-authority-for-local-https-development/ for more information on setting up a local CA depending on your oS.