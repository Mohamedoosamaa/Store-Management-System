USE store_Management;
GO

-- ================= DEFAULT USERS =================

INSERT INTO Users
(Username, PasswordHash, FullName, Role)
VALUES
('admin', 'admin123', 'System Admin', 'Admin');

INSERT INTO Users
(Username, PasswordHash, FullName, Role)
VALUES
('cashier1', 'cash123', 'Cashier User', 'Cashier');

-- ================= DEFAULT PRODUCTS =================

INSERT INTO Products
(Name, SKU, Price, StockQuantity, Category)
VALUES
('Coffee', 'CF001', 50, 100, 'Drinks'),

('Tea', 'TE001', 30, 70, 'Drinks'),

('Burger', 'BG001', 120, 40, 'Food');