SELECT 
    Text AS Errors
INTO Report.csv
FROM %file%
WHERE Text LIKE '%ERROR%'