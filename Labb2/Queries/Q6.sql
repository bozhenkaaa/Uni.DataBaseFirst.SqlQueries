SELECT DISTINCT Cars.Name
FROM Cars
WHERE Cars.ProducerId IN
 (SELECT Producers.Id
  FROM Producers
  WHERE Producers.Name = X);