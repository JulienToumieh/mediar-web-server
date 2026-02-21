bin\initdb -D data
bin\postgres --single -D data postgres

:: Paste the command below once the script is run (without the "::")

:: CREATE ROLE postgres WITH LOGIN SUPERUSER PASSWORD 'adminpass';