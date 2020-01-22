from Classes.runner import Runner
from Classes.argParser import ArgParser

if __name__ == '__main__':
	arg_parser = ArgParser()
	parsed_args = arg_parser.parse()
	
	runner = Runner(parsed_args)
	runner.run()
