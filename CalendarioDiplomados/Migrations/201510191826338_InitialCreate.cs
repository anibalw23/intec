namespace CalendarioDiplomados.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Calendarios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        fechaInicio = c.DateTime(nullable: false),
                        fechaFin = c.DateTime(nullable: false),
                        GrupoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Grupoes", t => t.GrupoID, cascadeDelete: true)
                .Index(t => t.GrupoID);
            
            CreateTable(
                "dbo.Eventoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        fechaIncicio = c.DateTime(nullable: false),
                        fechaFin = c.DateTime(nullable: false),
                        duracion = c.Int(nullable: false),
                        orden = c.Int(nullable: false),
                        CalendarioID = c.Int(nullable: false),
                        TallerID = c.Int(),
                        FacilitadorID = c.Int(),
                        ChoferID = c.Int(),
                        isLunes = c.Boolean(nullable: false),
                        isMartes = c.Boolean(nullable: false),
                        isMiercoles = c.Boolean(nullable: false),
                        isJueves = c.Boolean(nullable: false),
                        isViernes = c.Boolean(nullable: false),
                        isSabado = c.Boolean(nullable: false),
                        isDomingo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Calendarios", t => t.CalendarioID, cascadeDelete: true)
                .ForeignKey("dbo.Chofers", t => t.ChoferID)
                .ForeignKey("dbo.Facilitadors", t => t.FacilitadorID)
                .ForeignKey("dbo.Tallers", t => t.TallerID)
                .Index(t => t.CalendarioID)
                .Index(t => t.TallerID)
                .Index(t => t.FacilitadorID)
                .Index(t => t.ChoferID);
            
            CreateTable(
                "dbo.Chofers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        cedula = c.String(),
                        nombre = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Facilitadors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        cedula = c.String(),
                        nombre = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Tallers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        ModuloID = c.Int(nullable: false),
                        orden = c.Int(nullable: false),
                        FacilitadorID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Facilitadors", t => t.FacilitadorID)
                .ForeignKey("dbo.Moduloes", t => t.ModuloID, cascadeDelete: true)
                .Index(t => t.ModuloID)
                .Index(t => t.FacilitadorID);
            
            CreateTable(
                "dbo.Moduloes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        DiplomadoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Diplomadoes", t => t.DiplomadoID, cascadeDelete: true)
                .Index(t => t.DiplomadoID);
            
            CreateTable(
                "dbo.Diplomadoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        fechaInicio = c.DateTime(nullable: false),
                        fechaFin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Grupoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        cantidadParticipantes = c.Int(nullable: false),
                        DiplomadoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Diplomadoes", t => t.DiplomadoID, cascadeDelete: true)
                .Index(t => t.DiplomadoID);
            
            CreateTable(
                "dbo.Recurrencias",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        repeticiones = c.Int(nullable: false),
                        EventoID = c.Int(nullable: false),
                        DiaSemanaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DiaSemanas", t => t.DiaSemanaID, cascadeDelete: true)
                .ForeignKey("dbo.Eventoes", t => t.EventoID, cascadeDelete: true)
                .Index(t => t.EventoID)
                .Index(t => t.DiaSemanaID);
            
            CreateTable(
                "dbo.DiaSemanas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        numero = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Recurrencias", "EventoID", "dbo.Eventoes");
            DropForeignKey("dbo.Recurrencias", "DiaSemanaID", "dbo.DiaSemanas");
            DropForeignKey("dbo.Tallers", "ModuloID", "dbo.Moduloes");
            DropForeignKey("dbo.Moduloes", "DiplomadoID", "dbo.Diplomadoes");
            DropForeignKey("dbo.Grupoes", "DiplomadoID", "dbo.Diplomadoes");
            DropForeignKey("dbo.Calendarios", "GrupoID", "dbo.Grupoes");
            DropForeignKey("dbo.Tallers", "FacilitadorID", "dbo.Facilitadors");
            DropForeignKey("dbo.Eventoes", "TallerID", "dbo.Tallers");
            DropForeignKey("dbo.Eventoes", "FacilitadorID", "dbo.Facilitadors");
            DropForeignKey("dbo.Eventoes", "ChoferID", "dbo.Chofers");
            DropForeignKey("dbo.Eventoes", "CalendarioID", "dbo.Calendarios");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Recurrencias", new[] { "DiaSemanaID" });
            DropIndex("dbo.Recurrencias", new[] { "EventoID" });
            DropIndex("dbo.Grupoes", new[] { "DiplomadoID" });
            DropIndex("dbo.Moduloes", new[] { "DiplomadoID" });
            DropIndex("dbo.Tallers", new[] { "FacilitadorID" });
            DropIndex("dbo.Tallers", new[] { "ModuloID" });
            DropIndex("dbo.Eventoes", new[] { "ChoferID" });
            DropIndex("dbo.Eventoes", new[] { "FacilitadorID" });
            DropIndex("dbo.Eventoes", new[] { "TallerID" });
            DropIndex("dbo.Eventoes", new[] { "CalendarioID" });
            DropIndex("dbo.Calendarios", new[] { "GrupoID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.DiaSemanas");
            DropTable("dbo.Recurrencias");
            DropTable("dbo.Grupoes");
            DropTable("dbo.Diplomadoes");
            DropTable("dbo.Moduloes");
            DropTable("dbo.Tallers");
            DropTable("dbo.Facilitadors");
            DropTable("dbo.Chofers");
            DropTable("dbo.Eventoes");
            DropTable("dbo.Calendarios");
        }
    }
}
