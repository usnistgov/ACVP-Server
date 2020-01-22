import os
from shutil import copy2

rootdir = 'C:\\Users\\ctc\\Documents\\ACVP\\gen-val\\json-files'

for subdir, dirs, files in os.walk(rootdir):
	for algo in dirs:
		open(rootdir + "\\..\\registrations\\" + algo + ".json", "w+")
		copy2(rootdir + "\\" + algo + "\\registration.json", rootdir + "\\..\\registrations\\" + algo + ".json")

