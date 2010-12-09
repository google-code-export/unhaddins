ReadOnly tests must target a real/good copy of the database. This database could be configured as ReadOnly in Sql Server for instance.
For the readonly tests you need to create a database with these scripts:
http://code.google.com/p/unhaddins/source/browse/#svn/trunk/SampleDomain


Cascades and Inserts tests must target to an empty database.