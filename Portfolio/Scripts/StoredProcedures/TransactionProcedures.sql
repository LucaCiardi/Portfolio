USE FinancialTransactionDB;
GO

-- Add new transaction
CREATE OR ALTER PROCEDURE sp_AddTransaction
    @TransactionDate DATE,
    @Amount DECIMAL(18,2),
    @CategoryID INT,
    @Description NVARCHAR(255),
    @TransactionType NVARCHAR(50),
    @Source NVARCHAR(100)
AS
BEGIN
    INSERT INTO Transactions (
        TransactionDate,
        Amount,
        CategoryID,
        Description,
        TransactionType,
        Source
    )
    VALUES (
        @TransactionDate,
        @Amount,
        @CategoryID,
        @Description,
        @TransactionType,
        @Source
    );
    
    SELECT SCOPE_IDENTITY() AS TransactionID;
END;
GO

-- Get transaction by ID
CREATE OR ALTER PROCEDURE sp_GetTransactionByID
    @TransactionID BIGINT
AS
BEGIN
    SELECT 
        t.TransactionID,
        t.TransactionDate,
        t.Amount,
        t.CategoryID,
        c.CategoryName,
        t.Description,
        t.TransactionType,
        t.Source,
        t.CreatedDate
    FROM Transactions t
    LEFT JOIN Categories c ON t.CategoryID = c.CategoryID
    WHERE t.TransactionID = @TransactionID;
END;
GO

-- Get transactions by date range
CREATE OR ALTER PROCEDURE sp_GetTransactionsByDateRange
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        t.TransactionID,
        t.TransactionDate,
        t.Amount,
        t.CategoryID,
        c.CategoryName,
        t.Description,
        t.TransactionType,
        t.Source,
        t.CreatedDate
    FROM Transactions t
    LEFT JOIN Categories c ON t.CategoryID = c.CategoryID
    WHERE t.TransactionDate BETWEEN @StartDate AND @EndDate
    ORDER BY t.TransactionDate DESC;
END;
GO

-- Update transaction
CREATE OR ALTER PROCEDURE sp_UpdateTransaction
    @TransactionID BIGINT,
    @TransactionDate DATE,
    @Amount DECIMAL(18,2),
    @CategoryID INT,
    @Description NVARCHAR(255),
    @TransactionType NVARCHAR(50),
    @Source NVARCHAR(100)
AS
BEGIN
    UPDATE Transactions
    SET 
        TransactionDate = @TransactionDate,
        Amount = @Amount,
        CategoryID = @CategoryID,
        Description = @Description,
        TransactionType = @TransactionType,
        Source = @Source
    WHERE TransactionID = @TransactionID;
END;
GO

-- Delete transaction
CREATE OR ALTER PROCEDURE sp_DeleteTransaction
    @TransactionID BIGINT
AS
BEGIN
    DELETE FROM Transactions
    WHERE TransactionID = @TransactionID;
END;
GO
