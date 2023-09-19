CREATE TABLE `Post`(
	`Id` bigint(20) NOT NULL AUTO_INCREMENT,
	`Date` datetime NOT NULL,
	`Descricao` varchar(255) NOT NULL,
	`CategoriaId` bigint(20) NOT NULL,
	`Title` varchar(255) NOT NULL,
	`Content` varchar(255) NOT NULL,
	`Url` varchar(255) DEFAULT NULL,
	PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `Comment`(
	`Id` bigint(20) NOT NULL AUTO_INCREMENT,
	`PostId` bigint(20) NOT NULL,
	`Date` datetime NOT NULL,
	`Content` varchar(255) NOT NULL,
	`UserId` varchar(255) NULL,
	`CommentId` bigint(20) NULL,
	PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `Category`(
	`Id` bigint(20) NOT NULL AUTO_INCREMENT,
	`Date` datetime NOT NULL,
	`Name` varchar(255) NOT NULL,
	PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `PostCategory`(
	`Id` bigint(20) NOT NULL AUTO_INCREMENT,
	`PostId` bigint(20) NOT NULL,
	`CategoryId` bigint(20) NOT NULL,
	PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;