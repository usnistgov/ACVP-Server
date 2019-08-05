#!/usr/bin/env python

def die(message):
    print(message)
    import sys
    sys.exit(1)

import optparse
import json
from getpass import getpass
try:
    from restkit import Resource, BasicAuth
except ImportError:
    die("Please install restkit (in debian: python-restkit)")

def fetcher_factory(url, auth):
    """ This factory will create the actual method used to fetch issues from JIRA. This is really just a closure that saves us having
        to pass a bunch of parameters all over the place all the time. """
    def get_issue(key):
        """ Given an issue key (i.e. JRA-9) return the JSON representation of it. This is the only place where we deal
            with JIRA's REST API. """
        print('Fetching ' + key)
        # we need to expand subtasks and links since that's what we care about here.
        resource = Resource(url + ('/rest/api/latest/issue/%s' % key), filters=[auth])
        
        try:
            response = resource.get(headers={'Content-Type': 'application/json'})
        except:
            return None

        if response.status_int == 200:
            # Not all resources will return 200 on success. There are other success status codes. Like 204. We've read
            # the documentation for though and know what to expect here.
            issue = json.loads(response.body_string())
            return issue
        else:
            return None
    return get_issue

def poster_factory(url, auth):
    def post_issue(issue):
        print('Posting ' + issue['id'])

        resource = Resource(url + (''))

def parse_args():
    parser = optparse.OptionParser()
    parser.add_option('-u', '--user', dest='user', default='admin', help='Username to access JIRA')
    parser.add_option('-p', '--password', dest='password', help='Password to access JIRA')
    parser.add_option('-j', '--jira', dest='jira_url', default='http://jira.example.com', help='JIRA Base URL')
    parser.add_option('-g', '--gitlab', dest='gitlab_url', default='http://gitlab.example.com', help='GitLab Base URL')

    return parser.parse_args()

def get_password():
    return getpass("Please enter the Jira Password:")

if __name__ == '__main__':
    (options, args) = parse_args()

    # Basic Auth is usually easier for scripts like this to deal with than Cookies.
    auth = BasicAuth(options.user, options.password or get_password())
    issue_fetcher = fetcher_factory(options.jira_url, auth)
    issue_poster = poster_factory(options.gitlab_url, auth)

    issues = []
    for x in xrange(1,10):
        key = "ACVP-" + str(x)
        issue = issue_fetcher(key)
        if issue is not None:
            issues.append(issue)

    print(str(len(issues)) + " issues collected from JIRA")

    #for x in xrange(1, 10):

    print(issues[0]['fields']['description'])
