SELECT Cars.Name, Cars.Price
FROM Cars
WHERE Cars.ProducerId IN
 (SELECT Producers.Id
  FROM Producers
  WHERE Producers.CountryId IN
   (SELECT Countries.Id
   FROM Countries
   WHERE Countries.Name = X));