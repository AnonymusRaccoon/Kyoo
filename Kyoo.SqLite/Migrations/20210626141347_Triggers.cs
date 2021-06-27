﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Kyoo.SqLite.Migrations
{
	public partial class Triggers : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// language=SQLite
			migrationBuilder.Sql(@"
			CREATE TRIGGER SeasonSlugInsert AFTER INSERT ON Seasons FOR EACH ROW 
			BEGIN 
			    UPDATE Seasons SET Slug = (SELECT Slug from Shows WHERE ID = ShowID) || '-s' || SeasonNumber
				WHERE ID == new.ID;
			END");
			// language=SQLite
			migrationBuilder.Sql(@"
			CREATE TRIGGER SeasonSlugUpdate AFTER UPDATE OF SeasonNumber, ShowID ON Seasons FOR EACH ROW 
			BEGIN 
			    UPDATE Seasons SET Slug = (SELECT Slug from Shows WHERE ID = ShowID) || '-s' || SeasonNumber
				WHERE ID == new.ID;
			END");
			
			// language=SQLite
			migrationBuilder.Sql(@"
			CREATE TRIGGER EpisodeSlugInsert AFTER INSERT ON Episodes FOR EACH ROW 
			BEGIN 
			    UPDATE Episodes 
			    	SET Slug = (SELECT Slug from Shows WHERE ID = ShowID) || 
			    	           CASE
			    	               WHEN SeasonNumber IS NULL THEN '-' || AbsoluteNumber
			    	               ELSE '-s' || SeasonNumber || 'e' || EpisodeNumber
			    	           END
				WHERE ID == new.ID;
			END");
			// language=SQLite
			migrationBuilder.Sql(@"
			CREATE TRIGGER EpisodeSlugUpdate AFTER UPDATE OF AbsoluteNumber, EpisodeNumber, SeasonNumber, ShowID 
			    ON Episodes FOR EACH ROW 
			BEGIN 
			    UPDATE Episodes 
			    	SET Slug = (SELECT Slug from Shows WHERE ID = ShowID) || 
			    	           CASE
			    	               WHEN SeasonNumber IS NULL THEN '-' || AbsoluteNumber
			    	               ELSE '-s' || SeasonNumber || 'e' || EpisodeNumber
			    	           END
				WHERE ID == new.ID;
			END");
			
			
			// language=SQLite
			migrationBuilder.Sql(@"
			CREATE TRIGGER ShowSlugUpdate AFTER UPDATE OF Slug ON Shows FOR EACH ROW
			BEGIN
			    UPDATE Seasons SET Slug = new.Slug || '-s' || SeasonNumber WHERE ShowID = new.ID;
			    UPDATE Episodes 
			    	SET Slug = new.Slug || 
			    	           CASE
			    	               WHEN SeasonNumber IS NULL THEN '-' || AbsoluteNumber
			    	               ELSE '-s' || SeasonNumber || 'e' || EpisodeNumber
			    	           END
				WHERE ShowID = new.ID;
			END;");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// language=SQLite
			migrationBuilder.Sql("DROP TRIGGER SeasonSlugInsert;");
			// language=SQLite
			migrationBuilder.Sql("DROP TRIGGER SeasonSlugUpdate;");
			// language=SQLite
			migrationBuilder.Sql("DROP TRIGGER EpisodeSlugInsert;");
			// language=SQLite
			migrationBuilder.Sql("DROP TRIGGER EpisodeSlugUpdate;");
			// language=SQLite
			migrationBuilder.Sql("DROP TRIGGER ShowSlugUpdate;");
		}
	}
}