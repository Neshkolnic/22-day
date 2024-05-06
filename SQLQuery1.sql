-- Создание таблицы Продукты с автоинкрементным первичным ключом
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY,
    ProductName NVARCHAR(100),
    Description NVARCHAR(MAX),
    Price DECIMAL(10, 2),
    Image VARBINARY(MAX)
);

-- Создание таблицы Заказы с автоинкрементным первичным ключом
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY,
    CustomerID INT,
    OrderDate DATETIME,
    TotalAmount DECIMAL(10, 2)
);

-- Создание таблицы Детали заказа с автоинкрементным первичным ключом
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY,
    OrderID INT,
    ProductID INT,
    Quantity INT,
    UnitPrice DECIMAL(10, 2),
    CONSTRAINT FK_OrderDetails_OrderID FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    CONSTRAINT FK_OrderDetails_ProductID FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
