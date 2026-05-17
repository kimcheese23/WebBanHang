USE WebBanHang
GO

-- Procedure
-- 1
CREATE OR ALTER PROCEDURE sp_AddProduct
    @Name NVARCHAR(200), @Price DECIMAL(18,2), @Quantity INT, @CategoryId INT, @Image NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Products (Name, Price, Quantity, CategoryId, Image, IsDeleted)
    VALUES (@Name, @Price, @Quantity, @CategoryId, @Image, 0);
    
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO
-- 2
CREATE OR ALTER PROCEDURE sp_UpdateProduct
    @Id INT, @Name NVARCHAR(200), @Price DECIMAL(18,2), 
    @Quantity INT, @CategoryId INT, @Image NVARCHAR(MAX)
AS
BEGIN
    UPDATE Products 
    SET Name = @Name, Price = @Price, Quantity = @Quantity, 
        CategoryId = @CategoryId, Image = @Image
    WHERE Id = @Id
END
GO
-- 3
CREATE OR ALTER PROCEDURE sp_DeleteProduct 
    @Id INT
AS
BEGIN
    UPDATE Products SET IsDeleted = 1 WHERE Id = @Id
END
GO
-- 4
CREATE OR ALTER PROCEDURE sp_SearchProducts 
    @Keyword NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Products 
    WHERE Name LIKE '%' + @Keyword + '%' AND IsDeleted = 0
END
GO

-- 5
CREATE OR ALTER PROCEDURE sp_AddCategory
    @Name NVARCHAR(200)
AS
BEGIN
    INSERT INTO Categories (Name) 
    VALUES (@Name);
    
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- 6
CREATE OR ALTER PROCEDURE sp_UpdateCategory
    @Id INT, 
    @Name NVARCHAR(200)
AS
BEGIN
    UPDATE Categories 
    SET Name = @Name 
    WHERE Id = @Id;
END
GO

-- 7
CREATE OR ALTER PROCEDURE sp_DeleteCategory 
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CategoryName NVARCHAR(200);
    SELECT @CategoryName = Name FROM Categories WHERE Id = @Id;

    IF @CategoryName IS NULL RETURN;

    IF LOWER(LTRIM(RTRIM(@CategoryName))) LIKE N'%không xác định%'
    BEGIN
        RAISERROR (N'HỆ THỐNG BẢO VỆ: Bạn tuyệt đối không được xóa danh mục mặc định này!', 16, 1);
        RETURN;
    END

    DECLARE @DefaultCategoryId INT;
    SELECT @DefaultCategoryId = Id FROM Categories WHERE LOWER(LTRIM(RTRIM(Name))) LIKE N'%không xác định%';

    IF @DefaultCategoryId IS NULL
    BEGIN
        INSERT INTO Categories (Name) VALUES (N'Không xác định');
        SET @DefaultCategoryId = SCOPE_IDENTITY();
    END

    IF @Id = @DefaultCategoryId
    BEGIN
        RAISERROR (N'HỆ THỐNG BẢO VỆ: Không thể xóa danh mục hệ thống!', 16, 1);
        RETURN;
    END

    UPDATE Products 
    SET CategoryId = @DefaultCategoryId 
    WHERE CategoryId = @Id;

    DELETE FROM Categories WHERE Id = @Id;
END
GO

-- 8
CREATE OR ALTER PROCEDURE sp_SearchCategories 
    @Keyword NVARCHAR(100)
AS
BEGIN
    SELECT Id, Name 
    FROM Categories 
    WHERE Name LIKE N'%' + @Keyword + N'%'
END
GO
-- View
-- 1
CREATE OR ALTER VIEW View_ProductDetails AS
SELECT 
    p.Id, 
    p.Name AS ProductName, 
    p.Price, 
    p.Quantity, 
    c.Name AS CategoryName,
    p.Image
FROM Products p
JOIN Categories c ON p.CategoryId = c.Id
WHERE p.IsDeleted = 0;
GO
-- Function
-- 1
CREATE OR ALTER FUNCTION fn_CalculateStock()
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @TotalValue DECIMAL(18,2);
    SELECT @TotalValue = SUM(Price * Quantity) FROM Products WHERE IsDeleted = 0;
    RETURN ISNULL(@TotalValue, 0);
END;
GO
-- 2
CREATE OR ALTER FUNCTION dbo.fn_GetOrderTotal (@OrderId INT)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @TotalAmount DECIMAL(18, 2);
    
    SELECT @TotalAmount = ISNULL(SUM(Quantity * UnitPrice), 0)
    FROM dbo.OrderDetails
    WHERE OrderId = @OrderId;

    RETURN @TotalAmount;
END
GO
-- View
-- 2
CREATE OR ALTER VIEW dbo.View_AdminOrderSummary
AS
SELECT 
    o.Id AS OrderId,
    o.OrderDate,
    o.ShippingName AS CustomerName, 
    u.Email AS CustomerAccount,
    dbo.fn_GetOrderTotal(o.Id) AS TotalAmount,
    o.Status
FROM dbo.Orders o
LEFT JOIN dbo.ApplicationUser u ON o.UserId = u.Id;
GO
-- Procedure
-- 9
CREATE OR ALTER PROCEDURE dbo.sp_GetAdminOrders
AS
BEGIN
    SET NOCOUNT ON;
    SELECT OrderId, OrderDate, CustomerName, CustomerAccount, TotalAmount, Status
    FROM dbo.View_AdminOrderSummary
    ORDER BY OrderDate DESC;
END
GO
-- Trigger
-- 1
CREATE OR ALTER TRIGGER trg_UpdateProductQuantity
ON [dbo].[OrderDetails]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE p
    SET p.Quantity = p.Quantity - i.Quantity
    FROM [dbo].[Products] p
    INNER JOIN inserted i ON p.Id = i.ProductId;

    IF EXISTS (SELECT 1 FROM [dbo].[Products] p INNER JOIN inserted i ON p.Id = i.ProductId WHERE p.Quantity < 0)
    BEGIN
        RAISERROR ('Số lượng tồn kho không đủ để thực hiện đơn hàng này!', 16, 1);
        ROLLBACK TRANSACTION;
    END
END
GO

-- Function
-- 3
CREATE OR ALTER FUNCTION dbo.fn_CheckReviewEligibility (
    @UserId NVARCHAR(450),
    @ProductId INT
)
RETURNS BIT
AS
BEGIN
    DECLARE @IsValid BIT = 0;

    IF EXISTS (
        SELECT 1 FROM dbo.Orders o
        JOIN dbo.OrderDetails od ON o.Id = od.OrderId
        WHERE o.UserId = @UserId AND o.Status = 3 AND od.ProductId = @ProductId
    )
    BEGIN
        BEGIN
            SET @IsValid = 1;
        END
    END

    RETURN @IsValid;
END;
GO

-- View
-- 3
CREATE OR ALTER VIEW View_ProductRatingSummary AS
SELECT 
    p.Id AS ProductId,
    p.Name AS ProductName,
    COUNT(r.Id) AS TotalReviews,
    ISNULL(AVG(CAST(r.Rating AS DECIMAL(3,2))), 5.0) AS AverageRating
FROM Products p
LEFT JOIN Reviews r ON p.Id = r.ProductId
WHERE p.IsDeleted = 0
GROUP BY p.Id, p.Name;
GO

-- function
-- 4
CREATE OR ALTER FUNCTION fn_GetMonthlyRevenueReport (@Year INT)
RETURNS TABLE
AS
RETURN (
    WITH Months AS (
        SELECT 1 AS Month UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 
        UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 
        UNION ALL SELECT 9 UNION ALL SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12
    )
    SELECT 
        m.Month,
        ISNULL(SUM(dbo.fn_GetOrderTotal(o.Id)), 0) AS TotalRevenue,
        COUNT(o.Id) AS TotalOrders
    FROM Months m
    LEFT JOIN Orders o ON m.Month = MONTH(o.OrderDate) AND YEAR(o.OrderDate) = @Year AND o.Status = 3
    GROUP BY m.Month
);
GO
-- Procedure
-- 10
CREATE OR ALTER PROCEDURE sp_GetTopSellingProducts
    @TopCount INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT TOP (@TopCount)
        p.Id AS ProductId,
        p.Name AS ProductName,
        SUM(od.Quantity) AS TotalQuantitySold,
        SUM(od.Quantity * od.UnitPrice) AS TotalRevenueGenerated
    FROM OrderDetails od
    JOIN Products p ON od.ProductId = p.Id
    JOIN Orders o ON od.OrderId = o.Id
    WHERE o.Status = 3
    GROUP BY p.Id, p.Name
    ORDER BY TotalQuantitySold DESC;
END;
GO

-- Trgger 
-- 2
CREATE OR ALTER TRIGGER trg_LogOrderStatusChange
ON Orders
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF UPDATE(Status)
    BEGIN
        INSERT INTO OrderStatusLogs (OrderId, OldStatus, NewStatus, ChangedDate)
        SELECT 
            i.Id,
            d.Status,
            i.Status,
            GETDATE()
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id
        WHERE i.Status <> d.Status;
    END
END;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderStatusLogs]') AND type in (N'U'))
BEGIN
    CREATE TABLE OrderStatusLogs (
        Id INT IDENTITY(1000,1) PRIMARY KEY,
        OrderId INT,
        OldStatus INT,
        NewStatus INT,
        ChangedDate DATETIME DEFAULT GETDATE()
    );
END
GO

-- 3
CREATE OR ALTER TRIGGER trg_RestoreProductQuantityOnCancel
ON Orders
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF UPDATE(Status)
    BEGIN
        UPDATE p
        SET p.Quantity = p.Quantity + od.Quantity
        FROM Products p
        JOIN OrderDetails od ON p.Id = od.ProductId
        JOIN inserted i ON od.OrderId = i.Id
        JOIN deleted d ON i.Id = d.Id
        WHERE i.Status = 4 AND d.Status <> 4;
    END
END;
GO

CREATE OR ALTER FUNCTION dbo.fn_CalculateTotalRevenue()
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @TotalRevenue DECIMAL(18,2);
    
    SELECT @TotalRevenue = SUM(dbo.fn_GetOrderTotal(Id)) 
    FROM dbo.Orders 
    WHERE Status = 3;
    
    RETURN ISNULL(@TotalRevenue, 0);
END;
GO