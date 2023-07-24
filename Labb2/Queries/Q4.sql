SELECT Cars.Name
FROM Cars
WHERE Cars.Id IN
  (SELECT Purchases.CarId
  FROM Purchases
  WHERE Purchases.CustomerId IN
   (SELECT Customers.Id
   FROM Customers
   WHERE Customers.Name = X AND Customers.Surname = Y));