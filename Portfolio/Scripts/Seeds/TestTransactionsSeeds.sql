USE FinancialTransactionDB;
GO

INSERT INTO Transactions (TransactionDate, Amount, CategoryID, Description, TransactionType, Source)
VALUES 
('2025-01-01', 3000.00, 1, 'Monthly salary', 'Income', 'Employer'),
('2025-01-05', 150.00, 3, 'Grocery shopping', 'Expense', 'Supermarket'),
('2025-01-10', 100.00, 4, 'Electricity bill', 'Expense', 'Utility Company'),
('2025-01-15', 500.00, 2, 'Freelance project', 'Income', 'Client'),
('2025-01-20', 50.00, 5, 'Movie night', 'Expense', 'Cinema');
