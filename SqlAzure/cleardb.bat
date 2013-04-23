bcp "DELETE FROM dbo.[Solution]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[ProblemSetProblem]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[Problem]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[Prereq]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[ProblemSet]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[Student]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[Class]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
del blah.txt
