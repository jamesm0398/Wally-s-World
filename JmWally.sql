/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


CREATE DATABASE IF NOT EXISTS JMWally;
use JMWally;


DROP TABLE IF EXISTS customer;
CREATE TABLE customer (

	CustomerID varchar(5) DEFAULT NULL,
    FirstName mediumtext,
    LastName mediumtext,
    PhoneNum mediumtext
) ENGINE=InnoDB;

DROP TABLE IF EXISTS dealership;
CREATE TABLE dealership (
	DealershipID varchar(3) DEFAULT NULL,
	DealershipName mediumtext
    
) ENGINE=InnoDB;

DROP TABLE IF EXISTS vehicle;
CREATE TABLE vehicle (
	VIN varchar(11) DEFAULT NULL,
    Yeaar varchar(4) DEFAULT NULL,
    Make mediumtext,
    Model mediumtext,
    Colour mediumtext,
    Kms int(6) DEFAULT NULL,
    wPrice varchar(6) DEFAULT NULL,
    inStock mediumtext,
    DealershipID varchar(3)

) ENGINE=InnoDB;

DROP TABLE IF EXISTS `order`;
CREATE TABLE `order` (
	`OrderID` varchar(4) DEFAULT NULL,
    `Date` mediumtext,
    `CustomerID` varchar(5) DEFAULT NULL,
    `subTotal` varchar(6) DEFAULT NULL,
    `sPrice` varchar(6) DEFAULT NULL,
    `Status` mediumtext,
    `DealershipID` varchar(3)
) ENGINE=InnoDB;

DROP TABLE IF EXISTS `orderLine`;
CREATE TABLE `orderLine` (
	`OrderID` varchar(4) DEFAULT NULL,
    `VIN` varchar(11) DEFAULT NULL
) ENGINE=InnoDB;

INSERT INTO customer(CustomerID, FirstName, LastName, PhoneNum) VALUES
(00001, 'Wally World of Wheels', 'Inc.', '519-555-0000'),
(00002, 'Norbert', 'Mika', '416-555-1111'),
(00003, 'Russell', 'Foubert', '519-555-2222'),
(00004, 'Sean', 'Clarke', '519-555-3333');

INSERT INTO dealership (DealershipID, DealershipName) VALUES
(000, 'Unknown'),
(001, 'Sports World'),
(002, 'Guelph Auto Mall'),
(003, 'Waterloo');

INSERT INTO vehicle (VIN, Yeaar, Make, Model, Colour, Kms, wPrice, inStock, DealershipID) VALUES
('58847722BRB', '2010', 'Honda', 'Civic', 'Blue', '120332', 6500, 'Yes', 002),
('26663747GTG', '2009', 'Ford', 'Focus', 'Black', '89221', 8950, 'Yes', 000),
('99277544LOL', '2012', 'Volkswagen', 'Jetta', 'Silver', '156233', 13450, 'Yes', 003),
('53347223WTF', '2011', 'Buick', 'Regal', 'Mint', '134538', -7950, 'Yes', 003),
('99146514OMG', '2008', 'Volkswagen', 'Jetta', 'White', '199012', -2500, 'Yes', 005),
('27764534RTB', '2013', 'Dodge', 'Ram', 'Red', '211023', 10900, 'Yes', 001);


INSERT INTO `order` (`OrderID`, `Date`, `CustomerID`, `subTotal`, `sPrice`, `Status`) VALUES
(001, 'Sept 20th, 2017', 00004, '10900', 10900, 'PAID'),
(002, 'Sept 22nd, 2017', 00001, '-7950', -7950, 'PAID'),
(003, 'October 6th, 2017',00003, '6500', 6500, 'HOLD'),
(004, 'October 20th, 2017', 00003, '6500', 6500, 'CNCL'),
(005, 'November 2nd, 2017', 00002,  '-2500', -2500, 'PAID'),
(006, 'November 2nd, 2017', 00002, '13450', 13450, 'PAID');

INSERT INTO `orderLine` (`OrderID`, `VIN`) VALUES
(001, '27764534RTB'),
(002, '53347223WTF'),
(003, '58847722BRB'),
(004, '58847722BRB'),
(005, '99146514OMG'),
(006, '99277544LOL');
