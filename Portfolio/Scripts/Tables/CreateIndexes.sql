USE FinancialTransactionDB;
GO

-- Transaction indexes
CREATE INDEX IX_Transactions_TransactionDate 
ON Transactions (TransactionDate);

CREATE INDEX IX_Transactions_CategoryID 
ON Transactions (CategoryID);

-- Category indexes
CREATE INDEX IX_Categories_CategoryName 
ON Categories (CategoryName);

CREATE INDEX IX_Categories_CategoryType 
ON Categories (CategoryType);
