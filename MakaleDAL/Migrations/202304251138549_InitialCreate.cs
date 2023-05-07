namespace MakaleDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Beğeni",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Kullanici_ID = c.Int(),
                        Makale_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kullanıcı", t => t.Kullanici_ID)
                .ForeignKey("dbo.Makale", t => t.Makale_ID)
                .Index(t => t.Kullanici_ID)
                .Index(t => t.Makale_ID);
            
            CreateTable(
                "dbo.Kullanıcı",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Adi = c.String(maxLength: 20),
                        Soyadi = c.String(maxLength: 20),
                        KullaniciAdi = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 50),
                        Sifre = c.String(nullable: false, maxLength: 20),
                        Aktif = c.Boolean(nullable: false),
                        AktifGuid = c.Guid(nullable: false),
                        Admin = c.Boolean(nullable: false),
                        KayitTarihi = c.DateTime(nullable: false),
                        DegistirmeTarihi = c.DateTime(nullable: false),
                        DegistirenKullanici = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Makale",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Baslik = c.String(nullable: false, maxLength: 100),
                        Icerik = c.String(nullable: false),
                        Taslak = c.Boolean(nullable: false),
                        BegeniSayisi = c.Int(nullable: false),
                        KayitTarihi = c.DateTime(nullable: false),
                        DegistirmeTarihi = c.DateTime(nullable: false),
                        DegistirenKullanici = c.String(nullable: false, maxLength: 30),
                        Kategori_ID = c.Int(),
                        Kullanici_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Kategori", t => t.Kategori_ID)
                .ForeignKey("dbo.Kullanıcı", t => t.Kullanici_ID)
                .Index(t => t.Kategori_ID)
                .Index(t => t.Kullanici_ID);
            
            CreateTable(
                "dbo.Kategori",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Baslik = c.String(nullable: false, maxLength: 50),
                        Aciklama = c.String(maxLength: 150),
                        KayitTarihi = c.DateTime(nullable: false),
                        DegistirmeTarihi = c.DateTime(nullable: false),
                        DegistirenKullanici = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Yorum",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Test = c.String(maxLength: 250),
                        KayitTarihi = c.DateTime(nullable: false),
                        DegistirmeTarihi = c.DateTime(nullable: false),
                        DegistirenKullanici = c.String(nullable: false, maxLength: 30),
                        Kullanici_ID = c.Int(),
                        Makale_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Kullanıcı", t => t.Kullanici_ID)
                .ForeignKey("dbo.Makale", t => t.Makale_ID)
                .Index(t => t.Kullanici_ID)
                .Index(t => t.Makale_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Yorum", "Makale_ID", "dbo.Makale");
            DropForeignKey("dbo.Yorum", "Kullanici_ID", "dbo.Kullanıcı");
            DropForeignKey("dbo.Makale", "Kullanici_ID", "dbo.Kullanıcı");
            DropForeignKey("dbo.Makale", "Kategori_ID", "dbo.Kategori");
            DropForeignKey("dbo.Beğeni", "Makale_ID", "dbo.Makale");
            DropForeignKey("dbo.Beğeni", "Kullanici_ID", "dbo.Kullanıcı");
            DropIndex("dbo.Yorum", new[] { "Makale_ID" });
            DropIndex("dbo.Yorum", new[] { "Kullanici_ID" });
            DropIndex("dbo.Makale", new[] { "Kullanici_ID" });
            DropIndex("dbo.Makale", new[] { "Kategori_ID" });
            DropIndex("dbo.Beğeni", new[] { "Makale_ID" });
            DropIndex("dbo.Beğeni", new[] { "Kullanici_ID" });
            DropTable("dbo.Yorum");
            DropTable("dbo.Kategori");
            DropTable("dbo.Makale");
            DropTable("dbo.Kullanıcı");
            DropTable("dbo.Beğeni");
        }
    }
}
