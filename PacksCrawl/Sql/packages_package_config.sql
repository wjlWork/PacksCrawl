CREATE DATABASE  IF NOT EXISTS `packages` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `packages`;
-- MySQL dump 10.13  Distrib 5.6.13, for Win32 (x86)
--
-- Host: localhost    Database: packages
-- ------------------------------------------------------
-- Server version	5.6.14

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `package_config`
--

DROP TABLE IF EXISTS `package_config`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `package_config` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `list_configid` int(11) NOT NULL,
  `game_name_xpath` varchar(200) NOT NULL,
  `total_xpath` varchar(200) NOT NULL,
  `surplus_xpath` varchar(200) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `package_config`
--

LOCK TABLES `package_config` WRITE;
/*!40000 ALTER TABLE `package_config` DISABLE KEYS */;
INSERT INTO `package_config` VALUES (1,1,'//div[@class=\'title\']//h1','//div[@class=\'bar\']//p//b','//div[@class=\'bar\']//p//b'),(2,2,'//div[@class=\'gift-detail\']//dl//dd//h2','//div[@class=\'gift-detail\']//dl//dd//p[@class=\'p1\']//span[@id=\'countNum\']','//div[@class=\'gift-detail\']//dl//dd//p[@class=\'p1\']//span[@id=\'resetNum2\']'),(3,3,'//div[@class=\'Grab_txt fm rg\']/span','null','//div[@class=\'Grab_txt fm rg\']//p[@class=\'remain\']'),(4,4,'//div[@class=\'con\']//ul/li[1]','//div[@class=\'con\']//ul//li[5]//div[@class=\'bfbnum\']//i','//div[@class=\'con\']//ul//li[5]//div[@class=\'bfbnum\']//b');
/*!40000 ALTER TABLE `package_config` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2014-04-21 18:05:32
