Use WebBanHang
GO
CREATE PROCEDURE sp_AddProduct
    @Name NVARCHAR(200), @Price DECIMAL(18,2), @Quantity INT, @CategoryId INT, @Image NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Products (Name, Price, Quantity, CategoryId, Image)
    VALUES (@Name, @Price, @Quantity, @CategoryId, @Image);
    
    -- Trả về duy nhất một giá trị ID
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END

GO
CREATE PROCEDURE sp_SearchProducts @Keyword NVARCHAR(100)
AS
BEGIN
    SELECT * FROM Products WHERE Name LIKE '%' + @Keyword + '%'
END

GO
CREATE PROCEDURE sp_UpdateProduct
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

GO
CREATE PROCEDURE sp_DeleteProduct @Id INT
AS
BEGIN
    DELETE FROM Products WHERE Id = @Id
END


GO
CREATE VIEW View_ProductDetails AS
SELECT 
    p.Id, 
    p.Name AS ProductName, 
    p.Price, 
    p.Quantity, 
    c.Name AS CategoryName,
    p.Image
FROM Products p
JOIN Categories c ON p.CategoryId = c.Id;

GO
CREATE FUNCTION fn_CalculateStock()
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @TotalValue DECIMAL(18,2);
    SELECT @TotalValue = SUM(Price * Quantity) FROM Products;
    RETURN ISNULL(@TotalValue, 0);
END;