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
-- Table structure for table `packs_list_config`
--

DROP TABLE IF EXISTS `packs_list_config`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `packs_list_config` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `site_name` varchar(20) NOT NULL,
  `packs_list_homeurl` varchar(200) NOT NULL,
  `packs_list_urlsxpath` varchar(200) NOT NULL,
  `next_page_urlxpath` varchar(200) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `packs_list_config`
--

LOCK TABLES `packs_list_config` WRITE;
/*!40000 ALTER TABLE `packs_list_config` DISABLE KEYS */;
INSERT INTO `packs_list_config` VALUES (1,'任玩堂','http://bbs.appgame.com/plugin.php?id=moeac_grantcard&ac=all','//div[@id=\'main\']//li//a','//div[@class=\'pg\']//a'),(2,'着迷网','http://www.joyme.com/gift','//ul[@class=\'clearfix\']//li//a[last()]','//div[@class=\'page\']//a'),(3,'口袋巴士','http://bbs.ptbus.com/libao/','//div[@class=\'Spree_write fm rg\']//h2//a','//div[@class=\'pg\']//a'),(4,'18183网','http://ka.18183.com/list_ty_1.shtml','//ul[@class=\'checklist\']//div[@class=\'receive\']//a','');
/*!40000 ALTER TABLE `packs_list_config` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2014-04-21 18:05:33
