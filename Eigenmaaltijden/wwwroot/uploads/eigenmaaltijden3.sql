-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Gegenereerd op: 23 mrt 2021 om 12:22
-- Serverversie: 10.4.17-MariaDB
-- PHP-versie: 8.0.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `eigenmaaltijden`
--

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `maaltijden`
--

CREATE TABLE `maaltijden` (
  `MealID` int(10) UNSIGNED NOT NULL,
  `UserID` int(10) UNSIGNED NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` text NOT NULL,
  `PhotoPath` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Gegevens worden geëxporteerd voor tabel `maaltijden`
--

INSERT INTO `maaltijden` (`MealID`, `UserID`, `Name`, `Description`, `PhotoPath`) VALUES
(3, 1, 'Hutspot', 'Lekker hutspot', 'img/maaltijden/asdasdsd.png'),
(4, 1, 'Boerkool', 'Boerenkool Vers', 'img/maaltijden/obansdio.png'),
(5, 3, 'Macaroni and cheese', 'Lekkere mac & Cheese gemaakt in eigen keuken', 'img/maaltijden/obansdio.png');

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `maaltijd_info`
--

CREATE TABLE `maaltijd_info` (
  `MealID` int(10) UNSIGNED NOT NULL,
  `AmountAvailable` int(10) UNSIGNED NOT NULL,
  `Type` tinyint(3) UNSIGNED NOT NULL,
  `PortionPrice` float NOT NULL,
  `PortionWeight` int(10) UNSIGNED NOT NULL COMMENT 'Portion Weight is in Grams 	',
  `Fresh` tinyint(1) NOT NULL,
  `AvailableUntil` date NOT NULL,
  `PreparedOn` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Gegevens worden geëxporteerd voor tabel `maaltijd_info`
--

INSERT INTO `maaltijd_info` (`MealID`, `AmountAvailable`, `Type`, `PortionPrice`, `PortionWeight`, `Fresh`, `AvailableUntil`, `PreparedOn`) VALUES
(3, 5, 1, 2.5, 200, 1, '2021-03-24', '2021-03-22'),
(4, 3, 1, 4, 350, 1, '2021-03-23', '2021-03-22'),
(5, 3, 1, 1.5, 100, 1, '2021-03-21', '2021-03-25');

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `maaltijd_ingredienten`
--

CREATE TABLE `maaltijd_ingredienten` (
  `MealID` int(10) UNSIGNED NOT NULL,
  `Ingredient` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `verkoper`
--

CREATE TABLE `verkoper` (
  `UserID` int(10) UNSIGNED NOT NULL,
  `Email` varchar(255) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `SessionID` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Gegevens worden geëxporteerd voor tabel `verkoper`
--

INSERT INTO `verkoper` (`UserID`, `Email`, `Password`, `SessionID`) VALUES
(1, 'klaas@gmail.com', 'Test1234', 639805200),
(2, 'pieter@gmail.com', 'ILoveKlaas', 0),
(3, 'anna@gmail.com', 'AIOHSDina', 0),
(4, 'lissa@gmail.com', '^alkmnsdCAS&', 0);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `verkoper_adres`
--

CREATE TABLE `verkoper_adres` (
  `UserID` int(10) UNSIGNED NOT NULL,
  `Street` varchar(255) NOT NULL,
  `Number` smallint(5) UNSIGNED NOT NULL,
  `Addon` varchar(3) NOT NULL,
  `City` varchar(255) NOT NULL,
  `Country` varchar(255) NOT NULL,
  `PostCode` varchar(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Gegevens worden geëxporteerd voor tabel `verkoper_adres`
--

INSERT INTO `verkoper_adres` (`UserID`, `Street`, `Number`, `Addon`, `City`, `Country`, `PostCode`) VALUES
(1, 'Dorpstraat', 5, 'b', 'Amsterdam', 'Nederland', '1234XT'),
(2, 'Benelux', 1, '', 'Rotterdam', 'Nederland', '1522AB'),
(3, 'AppelStraat', 92, '', 'Leeuwarden ', 'Nederland', '8999XD'),
(4, 'GrachtStraat', 2, '', 'Ede', 'Nederland', '6132DS');

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `verkoper_profiel`
--

CREATE TABLE `verkoper_profiel` (
  `UserID` int(10) UNSIGNED NOT NULL,
  `Name` varchar(255) NOT NULL,
  `ProfilePhotoPath` text NOT NULL,
  `PhoneNumber` varchar(15) NOT NULL,
  `Description` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Gegevens worden geëxporteerd voor tabel `verkoper_profiel`
--

INSERT INTO `verkoper_profiel` (`UserID`, `Name`, `ProfilePhotoPath`, `PhoneNumber`, `Description`) VALUES
(1, 'Klaas Jackson', 'img/pf/asduihconzxc.png', '0612345678', 'Goed in de keuken'),
(2, 'Pieter Andreson', 'img/pf/vhuioasnjc.png', '0612345678', 'Goed in de keuken'),
(3, 'Anna Andreson', 'img/pf/onsidhgsuhdb.png', '0612345678', 'Goed in de keuken'),
(4, 'Lissa Vegas', 'img/pf/noasd8ibnsod.png', '0612345678', 'Goed in de keuken');

--
-- Indexen voor geëxporteerde tabellen
--

--
-- Indexen voor tabel `maaltijden`
--
ALTER TABLE `maaltijden`
  ADD PRIMARY KEY (`MealID`),
  ADD KEY `UserID` (`UserID`);

--
-- Indexen voor tabel `maaltijd_info`
--
ALTER TABLE `maaltijd_info`
  ADD UNIQUE KEY `MealID` (`MealID`);

--
-- Indexen voor tabel `maaltijd_ingredienten`
--
ALTER TABLE `maaltijd_ingredienten`
  ADD UNIQUE KEY `MealID_2` (`MealID`,`Ingredient`),
  ADD KEY `MealID` (`MealID`);

--
-- Indexen voor tabel `verkoper`
--
ALTER TABLE `verkoper`
  ADD PRIMARY KEY (`UserID`),
  ADD UNIQUE KEY `E-mail` (`Email`);

--
-- Indexen voor tabel `verkoper_adres`
--
ALTER TABLE `verkoper_adres`
  ADD KEY `UserID` (`UserID`);

--
-- Indexen voor tabel `verkoper_profiel`
--
ALTER TABLE `verkoper_profiel`
  ADD UNIQUE KEY `UserID_2` (`UserID`),
  ADD KEY `UserID` (`UserID`);

--
-- AUTO_INCREMENT voor geëxporteerde tabellen
--

--
-- AUTO_INCREMENT voor een tabel `maaltijden`
--
ALTER TABLE `maaltijden`
  MODIFY `MealID` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT voor een tabel `verkoper`
--
ALTER TABLE `verkoper`
  MODIFY `UserID` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Beperkingen voor geëxporteerde tabellen
--

--
-- Beperkingen voor tabel `maaltijden`
--
ALTER TABLE `maaltijden`
  ADD CONSTRAINT `maaltijden_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `verkoper` (`UserID`) ON DELETE CASCADE;

--
-- Beperkingen voor tabel `maaltijd_info`
--
ALTER TABLE `maaltijd_info`
  ADD CONSTRAINT `maaltijd_info_ibfk_1` FOREIGN KEY (`MealID`) REFERENCES `maaltijden` (`MealID`) ON DELETE CASCADE;

--
-- Beperkingen voor tabel `maaltijd_ingredienten`
--
ALTER TABLE `maaltijd_ingredienten`
  ADD CONSTRAINT `maaltijd_ingredienten_ibfk_1` FOREIGN KEY (`MealID`) REFERENCES `maaltijden` (`MealID`) ON DELETE CASCADE;

--
-- Beperkingen voor tabel `verkoper_adres`
--
ALTER TABLE `verkoper_adres`
  ADD CONSTRAINT `verkoper_adres_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `verkoper` (`UserID`) ON DELETE CASCADE;

--
-- Beperkingen voor tabel `verkoper_profiel`
--
ALTER TABLE `verkoper_profiel`
  ADD CONSTRAINT `verkoper_profiel_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `verkoper` (`UserID`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
