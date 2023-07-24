SELECT C.Name, C.Surname, C.Email
FROM Customers C
WHERE C.Email != Z
AND NOT EXISTS
    ((SELECT Purchases.CarId
      FROM Purchases
      WHERE Purchases.CustomerId IN
         (SELECT Customers.Id
           FROM Customers
           WHERE Customers.Email = Z))
     EXCEPT
     (SELECT Purchases.CarId
      FROM Purchases
      WHERE Purchases.CustomerId = C.Id))
AND NOT EXISTS
    ((SELECT Purchases.CarId
      FROM Purchases
      WHERE Purchases.CustomerId = C.Id)
     EXCEPT
     (SELECT Purchases.CarId
      FROM Purchases
      WHERE Purchases.CustomerId IN
          (SELECT Customers.Id
           FROM Customers
           WHERE Customers.Email = Z)));
