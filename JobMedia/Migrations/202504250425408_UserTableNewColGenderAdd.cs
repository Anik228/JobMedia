namespace JobMedia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTableNewColGenderAdd : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.User", "Gender", c => c.String());
        }
        
        public override void Down()
        {
           // DropColumn("dbo.User", "Gender");
        }
    }
}
