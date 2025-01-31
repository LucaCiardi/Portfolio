USE FinancialTransactionDB;
GO

INSERT INTO Categories (CategoryName, CategoryType, Description)
VALUES 
('Salary', 'Income', 'Regular income from employment'),
('Freelance', 'Income', 'Additional income from freelance work'),
('Groceries', 'Expense', 'Food and household supplies'),
('Utilities', 'Expense', 'Electricity, water, internet bills'),
('Entertainment', 'Expense', 'Movies, dining out, subscriptions');
