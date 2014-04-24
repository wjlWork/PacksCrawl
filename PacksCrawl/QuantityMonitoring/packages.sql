CREATE TABLE `packages`.`packs_list_config` (
  `id`                       int(11)      NOT NULL AUTO_INCREMENT,
  `site_name`                varchar(20)  NOT NULL,
  `packs_list_homeurl`       varchar(200) NOT NULL,
  `packs_list_urlsxpath`     varchar(200) NOT NULL,
  `next_page_urlxpath`       varchar(200) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;


CREATE TABLE `packages`.`package_config` (
  `id`                   int(11)      NOT NULL AUTO_INCREMENT,
  `list_configid`        int(11)      NOT NULL,
  `game_name_xpath`      varchar(200) NOT NULL,
  `total_xpath`     	 varchar(200) NOT NULL,
  `surplus_xpath`        varchar(200) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;


CREATE TABLE `packages`.`package` (
  `id`                   int(11)      NOT NULL AUTO_INCREMENT,
  `list_configid`        int(11)      NOT NULL,
  `game_name`            varchar(200) NOT NULL,
  `package_url`     	 varchar(200) NOT NULL,
  `total`                varchar(200) NOT NULL,
  `surplus`              varchar(200) NOT NULL,
  `send_count`          varchar(200) NOT NULL,
  `star_time`            datetime     NOT NULL,
  `current_time`         datetime     NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;


CREATE TABLE `packages`.`statis` (
  `id`                   int(11)      NOT NULL AUTO_INCREMENT,
  `list_configid`        int(11)      NOT NULL,
  `send_count`           varchar(200) NOT NULL,
  `star_time`            datetime     NOT NULL,
  `current_time`         datetime     NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;