USE FinancialTransactionDB;
GO

-- Add new category
CREATE OR ALTER PROCEDURE sp_AddCategory
    @CategoryName NVARCHAR(100),
    @CategoryType NVARCHAR(50),
    @Description NVARCHAR(255)
AS
BEGIN
    INSERT INTO Categories (
        CategoryName,
        CategoryType,
        Description
    )
    VALUES (
        @CategoryName,
        @CategoryType,
        @Description
    );
    
    SELECT SCOPE_IDENTITY() AS CategoryID;
END;
GO

-- Get all categories
CREATE OR ALTER PROCEDURE sp_GetAllCategories
AS
BEGIN
    SELECT 
        CategoryID,
        CategoryName,
        CategoryType,
        Description,
        CreatedDate
    FROM Categories
    ORDER BY CategoryName;
END;
GO

-- Get category by ID
CREATE OR ALTER PROCEDURE sp_GetCategoryByID
    @CategoryID INT
AS
BEGIN
    SELECT 
        CategoryID,
        CategoryName,
        CategoryType,
        Description,
        CreatedDate
    FROM Categories
    WHERE CategoryID = @CategoryID;
END;
GO

-- Update category
CREATE OR ALTER PROCEDURE sp_UpdateCategory
    @CategoryID INT,
    @CategoryName NVARCHAR(100),
    @CategoryType NVARCHAR(50),
    @Description NVARCHAR(255)
AS
BEGIN
    UPDATE Categories
    SET 
        CategoryName = @CategoryName,
        CategoryType = @CategoryType,
        Description = @Description
    WHERE CategoryID = @CategoryID;
END;
GO

-- Delete category (with check for existing transactions)
CREATE OR ALTER PROCEDURE sp_DeleteCategory
    @CategoryID INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Transactions WHERE CategoryID = @CategoryID)
    BEGIN
        DELETE FROM Categories
        WHERE CategoryID = @CategoryID;
        SELECT 1 AS Success;
    END
    ELSE
    BEGIN
        SELECT 0 AS Success;
    END
END;
GO

-- Get categories with transaction counts
CREATE OR ALTER PROCEDURE sp_GetCategoriesWithCount
AS
BEGIN
    SELECT 
        c.CategoryID,
        c.CategoryName,
        c.CategoryType,
        c.Description,
        COUNT(t.TransactionID) AS TransactionCount,
        SUM(t.Amount) AS TotalAmount
    FROM Categories c
    LEFT JOIN Transactions t ON c.CategoryID = t.CategoryID
    GROUP BY 
        c.CategoryID,
        c.CategoryName,
        c.CategoryType,
        c.Description
    ORDER BY c.CategoryName;
END;
GO
