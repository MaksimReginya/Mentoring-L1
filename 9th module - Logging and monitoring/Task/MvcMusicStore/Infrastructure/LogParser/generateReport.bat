SET logFile=%1
.\LogParser.exe -i "TEXTLINE" file:reportScript.sql?file=%logFile% -o "CSV" -filemode 1 -headers ON
.\LogParser.exe -i "TEXTLINE" file:onlyErrorsScript.sql?file=%logFile% -o "CSV" -filemode 0 -headers ON