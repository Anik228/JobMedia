namespace JobMedia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTableNewCol : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.User", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.User", "IsDeleted");
        }
    }
}


// If manullu create a table or change then for sync add migration and then update database command
// in the code command line for stop re creating the changes comment the creating part in the
//migration file then just execute update database for sync code to the db