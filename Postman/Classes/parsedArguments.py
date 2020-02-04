

class ParsedArguments():

    def __init__(
        self, 
        cert_file: str, 
        key_file: str, 
        registration_folder: str, 
        max_threads: int,
        environment: str,
        teamcity: bool,
        version: str
    ):
        self._cert_file = cert_file
        self._key_file = key_file
        self._registration_folder = registration_folder
        self._max_threads = max_threads
        self._environment = environment
        self._teamcity = teamcity
        self._version = version

    @property
    def cert_file(self) -> str:
        """
            The location of the signed certificate that has been registered on
            the ACVP server
        """

        return self._cert_file

    @property
    def key_file(self) -> str:
        """
            The location of the private key file that pertains to the cert
        """

        return self._key_file

    @property
    def registration_folder(self) -> str:
        """
            The folder to enumerate through containing json registrations.
        """

        return self._registration_folder

    @property
    def max_threads(self) -> int:
        """
            The maximum number of threads to make use of locally.
        """

        return self._max_threads

    @property
    def environment(self) -> str:
        """
            The environment to test against
        """

        return self._environment

    @property
    def teamcity(self) -> bool:
        """
            Turned on if the tests are being run on TeamCity
        """    

        return self._teamcity

    @property
    def version(self) -> str:
        """
            Specifies ACVP protocol version to use for testing
        """

        return self._version