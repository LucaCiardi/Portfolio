CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    CategoryType NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Transactions (
    TransactionID BIGINT IDENTITY(1,1) PRIMARY KEY,
    TransactionDate DATE NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    Description NVARCHAR(255),
    TransactionType NVARCHAR(50) NOT NULL,
    Source NVARCHAR(100),
    CreatedDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE AuditLog (
    AuditID BIGINT IDENTITY(1,1) PRIMARY KEY,
    TableName NVARCHAR(100),
    ActionType NVARCHAR(50),
    RecordID BIGINT,
    OldValue NVARCHAR(MAX),
    NewValue NVARCHAR(MAX),
    ActionDate DATETIME DEFAULT GETDATE(),
    UserName NVARCHAR(100)
);