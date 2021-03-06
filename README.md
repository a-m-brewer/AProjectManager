# A Project Manager

A program for managing multiple git repos

## Features

### Git and Docker Compose support

Start and stop docker containers at once based on Repository Groups/Sessions.

Manage git repositories based on Repository Groups/Sessions

### Repository Groups

Group git repositories and manage them in bulk. This is useful in for example a micro-service architecture where you have multiple dependent micro-services that all need to run during a development session.

### Repository Sessions

Create sessions which are similar to Repository Groups except that they will checkout a git branch on all repositories, and help in management of those repositories whilst working. e.g. Starting/Stopping docker containers on enter and exit of the session.

### YML Configuration

All config can be managed through the CLI or by modifying config files.

Another feature of this is that Project Groups are all stored in a way that does not reference local folders, so could be stored in a team git repo.

__Linux__
~~~~bash
/home/username/.config/aprojectmanager
~~~~
__OSX__
~~~~bash
/Users/username/.config/aprojectmanager
~~~~
__Windows__
~~~~bash
c:\Users\username\.config\aprojectmanager
~~~~

## Getting Started

### Install

Download latest release from [here](https://github.com/a-m-brewer/AProjectManager/releases)

Linux/OSX

1. Download latest release
2. Copy to /usr/local/bin/apm

Windows

1. Download latest release
2. Put somewhere that is accessible via your PATH env variable

### Importing Repositories (Repository Source)

At the moment there are two options for importing git projects into the manager.

#### From BitBucket

1. Login to BitBucket

~~~~bash
apm login -u <oauthkey> -p <oauthsecret> -s bitbucket
~~~~

this will create a Token.yml file and a User.yml file in your config folder

2. Clone required repositories

~~~~bash
apm clone -u <username> -r <role> -s bitbucket -p <projectkey> -d <clonefolder>
~~~~

__-u__ usename or group holding the repositories you want to clone

__-r__ your role in the repository

member: returns repositories to which the user has explicit read access

contributor: returns repositories to which the user has explicit write access

admin: returns repositories to which the user has explicit administrator access

owner: returns all repositories owned by the current user

__-s__ service the service to clone from (currently this is only bitbucket)

__-p__ project key, bitbucket project key

__-d__ folder you would like to clone all of the repositories to.

This step will create two files in your config folder

__RepositoryRegister.yml__: localtion of Repository Sources

__Repositories/bitbucket-username.yml__: file containing all the information about the repositories cloned

#### From Existing Repositories

1. Navigate to directory with git repositores in them

~~~~bash
apm repository-source -a "Add" -r $(ls)
~~~~

or individually via

~~~~bash
apm repository-source -a "Add" -r repo-folder1 repo-folder2
~~~~

### Creating a Repository Group

~~~~bash
apm group -a Add -g "Name of Group" -s repo_slug_1 repo_slug_2
~~~~

__-a__ action

__-g__ Name of the group

__-s__ the repo slugs to add 

e.g. for https://github.com/a-m-brewer/AProjectManager AProjectManager would be the slug

## Repository Sessions

#### Start

For the first time entering a session

~~~~bash
apm session -g "Group Name" -b "Branch Name" -c -a "Start" -s extra_repo_slug -d -f docker-compose-local.yml
~~~~

__-g__ an existing group to add to repository session

__-b__ the name of the branch that the session creates

__-c__ checkout on session start (default true)

__-a__ action Start,Checkout,Exit

__-s__ extra slugs to be a part of this session

__-d__ start docker containers on session start

__-f__ name of docker compose file (default docker-compose.yml)

#### Checkout

For entering a pre-existing session

~~~~bash
apm session -b "Branch Name" -c -a "Start" -d -f docker-compose-local.yml
~~~~

#### Exit

For exiting a session

~~~~bash
apm session -b "Branch Name" -a "Exit" -d -f docker-compose-local.yml
~~~~

## Git

### Pull

#### Pull all Repositories

~~~~bash
apm git -a "Pull"
~~~~

For all git actions not specifying a name will perform the action on all repositories registed. 

__-a__ action: Pull, Fetch, Checkout, Super

#### Pull repositories by name

This does not need to just be repository names, this can be Session or Group name.
This is the same for all Git Actions e.g. Pull, Fetch and Super

~~~~bash
apm git -a "Pull" -n ButHowDoItComputer "Group Name" "Session Name"
~~~~

### Fetch

~~~~bash
apm git -a "Fetch" -n ButHowDoItComputer "Group Name" "Session Name"
~~~~

### Checkout 

~~~~bash
apm git -a "Checkout" -n ButHowDoItComputer "Group Name" "Session Name" -b "a-branch" 
~~~~

__-b__ existing or non existing branch name

### Super

Run any command not supported by AProjectManager on some repositories like above

NOTE: remember quotes e.g "" around super arguments

~~~~bash
apm git -a "Super" -n ButHowDoItComputer "Group Name" "Session Name" -s "branch -a"
~~~~

## Docker Compose

### Up

~~~~bash
apm docker-compose -b -f docker-compose-local.yml -n "Group or Session or Repo" -a Up
~~~~

__-b__ build docker containers

__-f__ docker compose file

__-n__ name of Repository Group/Session or Repository.

__-a__ Action: Up or Down

### Down

~~~~bash
apm docker-compose -b -f docker-compose-local.yml -n "Group or Session or Repo" -a Down
~~~~

### Super

Run any command currently not implemented yet on a repository group / session or repo.

NOTE: remember quotes e.g "" around super arguments

~~~~bash
docker-compose -f docker-compose-local.yml -n "Group or Session or Repo" --super "up -d --build"
~~~~

## Print

Used for printing out Sources, Repositories, Groups and Sessions

### Print All

~~~~bash
apm print
~~~~

### Print Type

~~~~bash
apm print -t Session
~~~~

__-t__ types: Repository, Source, Session, Group

## Contributions

### Ideas

Here are some ideas of features that could be added that would be a great thing to contribute.

1. Display if there are changes on a branch when printing repositories
2. Print Repositories by Group / Session
3. Be able to add docker compose files to be part of a group / session.
    - For example when have a set of containers that need to be ran as part of a group.
    like a database.
