SELECT 
    TRIM(SUBSTR(text, 25, 5)) AS [_LogLevel], 
    COUNT([Index]) AS [_Count]
INTO Report.csv
FROM %file%
WHERE (CASE TRIM(SUBSTR(text, 25, 5)) 
        WHEN 'ERROR' THEN 1 
        WHEN 'DEBUG' THEN 1
        WHEN 'TRACE' THEN 1
        WHEN 'INFO' THEN 1
        ELSE 2
        END = 1)
GROUP BY TRIM(SUBSTR(text, 25, 5))
