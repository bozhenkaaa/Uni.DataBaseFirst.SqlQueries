SELECT Countries.Name
FROM Countries
WHERE Countries.Id IN
	(SELECT Producers.CountryId
	 FROM Producers
	 WHERE Producers.Id IN
		(SELECT P.id
		 FROM Producers P
		 WHERE NOT EXISTS
	 		((SELECT Cars.Price
			  FROM Cars
		      WHERE Cars.ProducerId = Z)
		     EXCEPT
		     (SELECT Cars.Price
		      FROM Cars
		      WHERE Cars.ProducerId = P.Id AND Cars.ProducerId != Z))));