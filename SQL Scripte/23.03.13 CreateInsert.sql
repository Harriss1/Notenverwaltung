USE tutorial_notenverwaltung;

#Verbindungstabellen
DROP TABLE if EXISTS Schueler_Hat_Klasse;

#Haupttabellen
DROP TABLE if EXISTS Note;
DROP TABLE if EXISTS Dozent;
DROP TABLE if EXISTS lehrer;
DROP TABLE if EXISTS Teilnehmer;
DROP TABLE if EXISTS schueler;
DROP TABLE if EXISTS person;

#Von den Haupttabellen abhängige Haupttabellen
DROP TABLE if EXISTS Klasse;
DROP TABLE if EXISTS Bildungsgang;
DROP TABLE if EXISTS Kurs;
DROP TABLE if EXISTS fach;
DROP TABLE if EXISTS NotenTyp;





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
			
CREATE TABLE Bildungsgang (
BildungsgangId INT NOT NULL AUTO_INCREMENT,
Bezeichnung VARCHAR(50),
PRIMARY KEY (BildungsgangId)
);

INSERT 	INTO Bildungsgang (Bezeichnung)
			VALUES
			('Systemintegration'),
			('Anwendungsentwicklung');

CREATE TABLE Klasse(
KlasseId INT NOT NULL AUTO_INCREMENT,
Bezeichnung VARCHAR(50) NOT NULL,
StartDatum DATE NOT NULL,
EndDatum DATE,
BildungsgangId INT NOT NULL,
PRIMARY KEY (KlasseId),
FOREIGN KEY (BildungsgangId) REFERENCES Bildungsgang(BildungsgangId)
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

CREATE TABLE Fach (
FachId INT NOT NULL AUTO_INCREMENT,
Bezeichnung VARCHAR(50),
Akronym VARCHAR(50),
PRIMARY KEY (FachId)
);

INSERT 	INTO Fach (Bezeichnung, Akronym)
			VALUES
			('Mathematik', 'MA'),
			('Deutsch', 'DEU');

CREATE TABLE Kurs (
KursId INT NOT NULL AUTO_INCREMENT,
Bezeichnung VARCHAR(50),
StartDatum DATE NOT NULL,
EndDatum DATE,
FachId INT NOT NULL,
PRIMARY KEY (KursId),
FOREIGN KEY (FachId) REFERENCES Fach(FachId)
);

INSERT 	INTO Kurs (Bezeichnung, StartDatum, FachId)
			VALUES
			('Mathe 1. LJ', '2022-08-31', 1),
			('Deutsch 2. HJ', '2022-03-31', 2);

CREATE TABLE Teilnehmer(
TeilnehmerId INT NOT NULL AUTO_INCREMENT,
SchuelerId INT,
KursId INT,
EndNote VARCHAR(50),
PRIMARY KEY (TeilnehmerId),
FOREIGN KEY (SchuelerId) REFERENCES schueler(SchuelerId),
FOREIGN KEY (KursId) REFERENCES Kurs(KursId)
);

INSERT 	INTO Teilnehmer (SchuelerId, KursId, Endnote)
			VALUES
			(1, 1, null),
			(2, 1, '3,0'),
			(2, 2, '2+');

CREATE TABLE Dozent(
DozentId INT NOT NULL AUTO_INCREMENT,
LehrerId INT,
KursId INT,
PRIMARY KEY (DozentId),
FOREIGN KEY (LehrerId) REFERENCES Lehrer(LehrerId),
FOREIGN KEY (KursId) REFERENCES Kurs(KursId)
);

INSERT 	INTO Dozent (LehrerId, KursId)
			VALUES
			(1, 1),
			(2, 1),
			(2, 2);

CREATE TABLE NotenTyp(
NotenTypId INT NOT NULL AUTO_INCREMENT,
Bezeichnung VARCHAR(50),
Akronym VARCHAR(50),
PRIMARY KEY (NotenTypId)
);

INSERT 	INTO NotenTyp (Bezeichnung, Akronym)
			VALUES
			('Klausur', 'KA'),
			('Sonstige Mitarbeit', 'SoMi'),
			('Projekt', 'PR');

# Tabelle Gewichtungsart: Feld 1= NotentypA, Feld 2 = NotenTypAGewichtung

CREATE TABLE Note(
NoteId INT NOT NULL AUTO_INCREMENT,
TeilnehmerId INT NOT NULL,
DozentId INT NOT NULL,
Bemerkung VARCHAR(50),
Datum DATE,
NotenTypId INT,
WertInProzent INT,
PRIMARY KEY (NoteId),
FOREIGN KEY (TeilnehmerId) REFERENCES Teilnehmer(TeilnehmerId),
FOREIGN KEY (NotenTypId) REFERENCES NotenTyp(NotenTypId),
FOREIGN KEY (DozentId) REFERENCES Dozent(DozentId)
);
INSERT 	INTO Note (TeilnehmerId, DozentId, Bemerkung, Datum, NotenTypId, WertInProzent)
			VALUES
			(1, 1, 'logisch über Algebra Aufgabe argumentiert', '2022-10-03', 2, '98'),
			(1, 1, '', '2022-11-23', 1, '83'),
			(1, 2, '', '2023-01-05', 3, '75'),
			(2, 3, 'Fragen wiederholt nicht beantwortet', '2022-10-14', 2, '65'),
			(2, 3, '', '2022-11-23', 1, '90'),
			(2, 3, 'Abgabe drei Tage zu spät am 8.1.2023', '2023-01-05', 3, '0');


