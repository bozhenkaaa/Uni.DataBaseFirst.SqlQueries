SELECT Sum(Cars.Price)
FROM Cars
WHERE Cars.ProducerId IN
 (SELECT Producers.Id
  FROM Producers
  WHERE Producers.Name = X);