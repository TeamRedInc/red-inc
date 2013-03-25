bcp "DELETE FROM dbo.[Solution]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[ProblemSetProblem]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[Problem]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[Prereq]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[ProblemSet]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[Student]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[Class]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp "DELETE FROM dbo.[User]; SELECT 1 AS 'blah';" queryout blah.txt -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp dbo.[User] in 01User.dat -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp dbo.[Class] in 02Class.dat -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp dbo.[Student] in 03Student.dat -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp dbo.[ProblemSet] in 04ProblemSet.dat -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp dbo.[Prereq] in 05Prereq.dat -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp dbo.[Problem] in 06Problem.dat -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp dbo.[ProblemSetProblem] in 07ProblemSetProblem.dat -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
bcp dbo.[Solution] in 08Solution.dat -F 2 -E -c -U red-inc-admin@zcm2garak5 -S zcm2garak5.database.windows.net -P CS4911team18 -d red-inc_db
del blah.txt
