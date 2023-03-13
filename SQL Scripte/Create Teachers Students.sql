USE tutorial_notenverwaltung;

#Verbindungstabellen
DROP TABLE if EXISTS Schueler_Hat_Klasse;

#Haupttabellen
DROP TABLE if EXISTS Lehrer;
DROP TABLE if EXISTS Schueler;
DROP TABLE if EXISTS person;

#Von den Haupttabellen abhängige Haupttabellen
DROP TABLE if EXISTS Klasse;


CREATE TABLE Person (
PersonId INT NOT NULL AUTO_INCREMENT,
Vorname VARCHAR(50),
Nachname VARCHAR(50),
Geburtsdatum DATE,
Geburtsort VARCHAR(50),
Benutzername VARCHAR(50) NOT NULL UNIQUE,
Passwort VARCHAR(50),
PRIMARY KEY (`PersonId`)
);

INSERT 	INTO person (Vorname, Nachname, Geburtsdatum, Geburtsort, Benutzername, Passwort) 
			VALUES 
			('Max', 'Mustermann', '2022-01-01','Bielefeld', 'user', 'user'),
			('Paula', 'Pause', '1990-12-05', 'Münchhausen', 'guest', 'guest'),
			('Admin', 'Nistrator', '1900-01-01', 'Münchhausen', 'admin', 'admin'),
			('Herbert', 'Herbtraube', '1940-03-19', 'Münchhausen', 'teacher', 'teacher');

CREATE TABLE Lehrer(
LehrerId INT NOT NULL AUTO_INCREMENT,
PersonId INT NOT NULL,
FOREIGN KEY (PersonId) REFERENCES Person(PersonId),
PRIMARY KEY (LehrerId)
);		

INSERT 	INTO Lehrer (PersonId)
			VALUES
			(3),
			(4);
			
CREATE TABLE Schueler(
SchuelerId INT NOT NULL AUTO_INCREMENT,
PersonId INT NOT NULL,
FOREIGN KEY (PersonId) REFERENCES Person(PersonId),
PRIMARY KEY (SChuelerId)
);	
INSERT 	INTO Schueler (PersonId)
			VALUES
			(1),
			(2);
			
CREATE TABLE Klasse(
KlasseId INT NOT NULL AUTO_INCREMENT,
Bezeichnung VARCHAR(50) NOT NULL,
StartDatum DATE NOT NULL,
EndDatum DATE,
BildungsgangId INT NOT NULL,
PRIMARY KEY (KlasseId)
);

INSERT 	INTO Klasse (Bezeichnung, StartDatum, BildungsgangId)
			VALUES
			('IA121', '2021-08-01', 1),
			('IS322', '2019-08-01', 2);

CREATE TABLE Schueler_Hat_Klasse(
SchuelerId INT NOT NULL,
KlasseId INT NOT NULL,
FOREIGN KEY (SchuelerId) REFERENCES Schueler(SchuelerId),
FOREIGN KEY (KlasseId) REFERENCES Klasse(KlasseId)
);

INSERT 	INTO Schueler_Hat_Klasse (SchuelerId, KlasseId)
			VALUES
			(1,1),
			(1,2), # Schueler 1 geht in zwei Klassen
			(2,2);