arcmap-layer-search
===================

This project is for a C# and python application that searches for specific layers in a set of MXDs

Given a specific folder, we want the ability to search all MXDs and find all known layers within those MXDs in order to locate unique layers used within each MXD.

## Overview ##

This idea came from Lauren Franklin who needed to locate a specific layer that she recently used but could not find the shared path.

### Todo ###
Application can now read layers from single MXD layers added to the system and display their unique datasource. We will need to add the ability to add multiple sources and recursive searching of directories for MXD's in an asynchronous manner.

Additionally, we will need the ability to dynamically drag and drop files into the window.

