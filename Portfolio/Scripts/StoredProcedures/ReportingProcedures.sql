USE FinancialTransactionDB;
GO

-- Get monthly summary
CREATE OR ALTER PROCEDURE sp_GetMonthlySummary
    @Year INT,
    @Month INT
AS
BEGIN
    SELECT 
        c.CategoryType,
        c.CategoryName,
        COUNT(t.TransactionID) AS TransactionCount,
        SUM(t.Amount) AS TotalAmount,
        AVG(t.Amount) AS AverageAmount
    FROM Transactions t
    JOIN Categories c ON t.CategoryID = c.CategoryID
    WHERE YEAR(t.TransactionDate) = @Year 
    AND MONTH(t.TransactionDate) = @Month
    GROUP BY c.CategoryType, c.CategoryName
    ORDER BY c.CategoryType, TotalAmount DESC;
END;
GO

-- Get date range summary
CREATE OR ALTER PROCEDURE sp_GetDateRangeSummary
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        c.CategoryType,
        SUM(t.Amount) AS TotalAmount,
        COUNT(t.TransactionID) AS TransactionCount
    FROM Transactions t
    JOIN Categories c ON t.CategoryID = c.CategoryID
    WHERE t.TransactionDate BETWEEN @StartDate AND @EndDate
    GROUP BY c.CategoryType;
END;
GO

-- Get daily balance
CREATE OR ALTER PROCEDURE sp_GetDailyBalance
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        t.TransactionDate,
        SUM(CASE 
            WHEN c.CategoryType = 'Income' THEN t.Amount
            ELSE -t.Amount 
        END) AS DailyBalance
    FROM Transactions t
    JOIN Categories c ON t.CategoryID = c.CategoryID
    WHERE t.TransactionDate BETWEEN @StartDate AND @EndDate
    GROUP BY t.TransactionDate
    ORDER BY t.TransactionDate;
END;
GO

-- Get category trends
CREATE OR ALTER PROCEDURE sp_GetCategoryTrends
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        c.CategoryName,
        MONTH(t.TransactionDate) AS Month,
        YEAR(t.TransactionDate) AS Year,
        SUM(t.Amount) AS TotalAmount,
        COUNT(t.TransactionID) AS TransactionCount
    FROM Transactions t
    JOIN Categories c ON t.CategoryID = c.CategoryID
    WHERE t.TransactionDate BETWEEN @StartDate AND @EndDate
    GROUP BY 
        c.CategoryName,
        MONTH(t.TransactionDate),
        YEAR(t.TransactionDate)
    ORDER BY 
        Year, 
        Month, 
        TotalAmount DESC;
END;
GO

-- Get top spending categories
CREATE OR ALTER PROCEDURE sp_GetTopSpendingCategories
    @StartDate DATE,
    @EndDate DATE,
    @TopCount INT = 5
AS
BEGIN
    SELECT TOP(@TopCount)
        c.CategoryName,
        SUM(t.Amount) AS TotalAmount,
        COUNT(t.TransactionID) AS TransactionCount
    FROM Transactions t
    JOIN Categories c ON t.CategoryID = c.CategoryID
    WHERE 
        t.TransactionDate BETWEEN @StartDate AND @EndDate
        AND c.CategoryType = 'Expense'
    GROUP BY c.CategoryName
    ORDER BY TotalAmount DESC;
END;
GO

-- Get income vs expenses summary
CREATE OR ALTER PROCEDURE sp_GetIncomeVsExpenses
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        MONTH(t.TransactionDate) AS Month,
        YEAR(t.TransactionDate) AS Year,
        SUM(CASE 
            WHEN c.CategoryType = 'Income' THEN t.Amount 
            ELSE 0 
        END) AS TotalIncome,
        SUM(CASE 
            WHEN c.CategoryType = 'Expense' THEN t.Amount 
            ELSE 0 
        END) AS TotalExpenses,
        SUM(CASE 
            WHEN c.CategoryType = 'Income' THEN t.Amount 
            ELSE -t.Amount 
        END) AS NetAmount
    FROM Transactions t
    JOIN Categories c ON t.CategoryID = c.CategoryID
    WHERE t.TransactionDate BETWEEN @StartDate AND @EndDate
    GROUP BY 
        YEAR(t.TransactionDate),
        MONTH(t.TransactionDate)
    ORDER BY 
        Year,
        Month;
END;
GO
