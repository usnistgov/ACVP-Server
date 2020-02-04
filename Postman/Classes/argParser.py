import argparse

from Classes.parsedArguments import ParsedArguments

class ArgParser():

    def parse(self) -> ParsedArguments:
        """
            Parses command line arguments and returns that in a ParsedArguments object
        """

        parser = argparse.ArgumentParser(description='Arguments for running registrations against the ACVP driver.')
        
        # required arguments
        requiredNamed = parser.add_argument_group('required named arguments')
        requiredNamed.add_argument(
            '-c', 
            '--cert', 
            help="Full file name of certificate used to authenticate to ACVP driver", 
            required=True
        )
        requiredNamed.add_argument(
            '-k',
            '--key',
            help='Full file name of the private key file',
            required=True
        )
        requiredNamed.add_argument(
            '-r',
            '--reg',
            help='The folder location where the json formatted registrations can be found.',
        )

        # optional argument
        parser.add_argument(
            '-t',
            '--threads',
            help='The maximum number of threads to use locally.',
            type=int,
            default=1
        )

        choices_environment = ['dev', 'test', 'demo']
        parser.add_argument(
            '-e',
            '--environment',
            help='The ACVP environment to test against (dev if argument not provided)',
            default='dev',
            choices=choices_environment
        )

        parser.add_argument(
            '-j',
            '--teamcity',
            help='Indicates the test is being run on TeamCity',
            default=False
        )

        # todo can probably make this argument required at some point, but providing a default right now for backwards compatibility
        parser.add_argument(
            '-v',
            '--version',
            help='specifies the protocol version to use for postman testing',
            default='v0.5'
        )

        args = parser.parse_args()

        return ParsedArguments(args.cert, args.key, args.reg, args.threads, args.environment, args.teamcity, args.version)
        