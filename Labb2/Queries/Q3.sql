SELECT Customers.Name, Customers.Surname
FROM Customers
WHERE Customers.Id IN
 (SELECT Purchases.CustomerId
  FROM Purchases
  WHERE Purchases.CarId IN
   (SELECT Cars.id
   FROM Cars
   WHERE Cars.ProducerId IN
    (SELECT Producers.Id
    FROM Producers
    WHERE Producers.Name = X)));