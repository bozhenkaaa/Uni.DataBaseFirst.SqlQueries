SELECT C.surname, C.email
FROM Customers C
WHERE C.name = Z
AND NOT EXISTS
 ((SELECT Purchases.CarId
   FROM Purchases
   WHERE Purchases.CustomerId = C.Id)
  EXCEPT
  (SELECT Cars.Id
   FROM Cars))
AND NOT EXISTS
 ((SELECT Cars.Id
   FROM Cars)
  EXCEPT
  (SELECT Purchases.CarId
   FROM Purchases
   WHERE Purchases.CustomerId = C.Id));