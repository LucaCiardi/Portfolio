USE FinancialTransactionDB;
GO

-- Calculate balance for a specific date
CREATE OR ALTER FUNCTION fn_GetBalanceAtDate
(
    @Date DATE
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Balance DECIMAL(18,2);
    
    SELECT @Balance = SUM(
        CASE 
            WHEN c.CategoryType = 'Income' THEN t.Amount
            ELSE -t.Amount
        END)
    FROM Transactions t
    JOIN Categories c ON t.CategoryID = c.CategoryID
    WHERE t.TransactionDate <= @Date;
    
    RETURN ISNULL(@Balance, 0);
END;
GO

-- Calculate monthly total by category type
CREATE OR ALTER FUNCTION fn_GetMonthlyTotalByCategoryType
(
    @Year INT,
    @Month INT,
    @CategoryType NVARCHAR(50)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Total DECIMAL(18,2);
    
    SELECT @Total = SUM(t.Amount)
    FROM Transactions t
    JOIN Categories c ON t.CategoryID = c.CategoryID
    WHERE YEAR(t.TransactionDate) = @Year
    AND MONTH(t.TransactionDate) = @Month
    AND c.CategoryType = @CategoryType;
    
    RETURN ISNULL(@Total, 0);
END;
GO

-- Calculate running balance
CREATE OR ALTER FUNCTION fn_GetRunningBalance
(
    @StartDate DATE,
    @EndDate DATE
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        t.TransactionDate,
        SUM(CASE 
            WHEN c.CategoryType = 'Income' THEN t.Amount
            ELSE -t.Amount
        END) OVER (ORDER BY t.TransactionDate) AS RunningBalance
    FROM Transactions t
    JOIN Categories c ON t.CategoryID = c.CategoryID
    WHERE t.TransactionDate BETWEEN @StartDate AND @EndDate
);
GO

-- Calculate category percentage
CREATE OR ALTER FUNCTION fn_GetCategoryPercentage
(
    @StartDate DATE,
    @EndDate DATE,
    @CategoryType NVARCHAR(50)
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        c.CategoryName,
        SUM(t.Amount) AS CategoryTotal,
        (SUM(t.Amount) * 100.0) / 
        NULLIF(SUM(SUM(t.Amount)) OVER (), 0) AS Percentage
    FROM Transactions t
    JOIN Categories c ON t.CategoryID = c.CategoryID
    WHERE t.TransactionDate BETWEEN @StartDate AND @EndDate
    AND c.CategoryType = @CategoryType
    GROUP BY c.CategoryName
);
GO
