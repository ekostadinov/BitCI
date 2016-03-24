# BitCI

BitCI is a self-hosted Continuous integration system. There is only one current version of it, 
but it can be used for both private and public GitHub repositories.

This repository contains the [central issue
tracker](https://github.com/ekostadinov/BitCI/issues) for the BitCI project.

## Documentation

Documentation for the Travis CI project can also be found here.

## Other repositories

BitCI consists of many different sub-functionalities using ASP.Net MVC. The main ones are:

### bitci-context

[bitci-context](https://github.com/ekostadinov/BitCI/tree/master/BitCI/Context) is the [Entity framework](https://msdn.microsoft.com/en-us/data/ef.aspx) context that's
responsible for serving our DB work. It responds to different LINQ/SQL queries and
runs CRUD operations. Very little custom logic is in this
repository.

### bitci-build

[bitci-build](https://github.com/ekostadinov/BitCI/tree/master/BitCI/Models/BuildSteps) creates the build
script for each job and related steps. It takes the configuration from the `Web.config` file and
creates a `bash` or `batch` scripts that is then run in the build (MSBuild) environment.

### bitci-core

[travis-core](https://github.com/ekostadinov/BitCI/tree/master/BitCI) holds most of the logic
for BitCI. This functionality is shared across several others and
holds the models, controllers, services, and other things that these apps need.

### bitci-logs

[travis-logs](https://github.com/ekostadinov/BitCI/tree/master/BitCI) receives log updates
from build steps, saves them to the database and local file, after that pushes
them to the Web client for monitoring. When a job is finished, travis-logs is
responsible for archiving.

### bitci-web

[bitci-web](https://github.com/ekostadinov/BitCI/tree/master/BitCI/Views) is our main Web client logic.
It is written using MVC and communicates with
bitci-core to get information and gets live updates from
bitci-logs and DB through Entity framework.
