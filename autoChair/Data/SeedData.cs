using autoChair.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace autoChair.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Vérifier si les données existent déjà
                if (context.Categories.Any())
                {
                    return;
                }

                // ================== CATÉGORIES ==================
                var categories = new Category[]
                {
                    new Category
                    {
                        Name = "Chaises de Bureau",
                        Description = "Chaises ergonomiques pour le travail prolongé"
                    },
                    new Category
                    {
                        Name = "Chaises de Salle à Manger",
                        Description = "Chaises élégantes et confortables pour la table"
                    },
                    new Category
                    {
                        Name = "Chaises Gaming",
                        Description = "Chaises haute performance pour gamers"
                    },
                    new Category
                    {
                        Name = "Chaises Lounge",
                        Description = "Chaises relaxantes et confortables pour se détendre"
                    }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();

                // ================== PRODUITS ==================
                // Récupérer les catégories créées (avec les bons IDs)
                var catBureau = context.Categories.FirstOrDefault(c => c.Name == "Chaises de Bureau");
                var catSalleManger = context.Categories.FirstOrDefault(c => c.Name == "Chaises de Salle à Manger");
                var catGaming = context.Categories.FirstOrDefault(c => c.Name == "Chaises Gaming");
                var catLounge = context.Categories.FirstOrDefault(c => c.Name == "Chaises Lounge");

                var products = new Product[]
                {
                    // ====== BUREAU (1 → 10) ======
                new Product { Name="ErgoFlex Pro", Description="Chaise de bureau ergonomique avec soutien lombaire renforcé, idéale pour le travail intensif et le télétravail.", Price=199.99m, ImageUrl="1.jpg", Stock=15, CategoryId=catBureau.Id },
                new Product { Name="OfficeComfort Plus", Description="Chaise professionnelle avec dossier ajustable et accoudoirs réglables pour un confort personnalisé.", Price=209.99m, ImageUrl="2.jpg", Stock=12, CategoryId=catBureau.Id },
                new Product { Name="WorkEase Mesh", Description="Chaise de bureau en mesh respirant assurant une bonne circulation de l’air pendant les longues journées.", Price=189.99m, ImageUrl="3.jpg", Stock=20, CategoryId=catBureau.Id },
                new Product { Name="FlexiSeat Smart", Description="Chaise moderne avec assise rembourrée et mécanisme d’inclinaison pour une posture saine.", Price=179.99m, ImageUrl="4.jpg", Stock=18, CategoryId=catBureau.Id },
                new Product { Name="DailyOffice Comfort", Description="Chaise fonctionnelle à hauteur ajustable, parfaite pour une utilisation quotidienne.", Price=169.99m, ImageUrl="5.jpg", Stock=25, CategoryId=catBureau.Id },
                new Product { Name="Executive Line", Description="Chaise de bureau au design élégant combinant confort, stabilité et finition premium.", Price=219.99m, ImageUrl="6.jpg", Stock=10, CategoryId=catBureau.Id },
                new Product { Name="ProTask Chair", Description="Chaise ergonomique conçue pour améliorer la productivité et réduire la fatigue.", Price=199.99m, ImageUrl="7.jpg", Stock=14, CategoryId=catBureau.Id },
                new Product { Name="OfficeElite", Description="Chaise haut de gamme avec base renforcée et soutien optimal pour les longues sessions.", Price=229.99m, ImageUrl="8.jpg", Stock=9, CategoryId=catBureau.Id },
                new Product { Name="ComfortWork Plus", Description="Chaise ergonomique offrant un excellent maintien du dos et un confort longue durée.", Price=189.99m, ImageUrl="9.jpg", Stock=22, CategoryId=catBureau.Id },
                new Product { Name="HomeOffice Max", Description="Chaise idéale pour le télétravail, combinant ergonomie, design et robustesse.", Price=249.99m, ImageUrl="10.jpg", Stock=8, CategoryId=catBureau.Id },

                // ====== SALLE À MANGER (11 → 20) ======
                new Product { Name="WoodStyle Classic", Description="Chaise élégante en bois, parfaite pour une salle à manger chaleureuse.", Price=99.99m, ImageUrl="11.jpg", Stock=30, CategoryId=catSalleManger.Id },
                new Product { Name="DiningComfort", Description="Chaise de salle à manger avec assise confortable pour des repas prolongés.", Price=109.99m, ImageUrl="12.jpg", Stock=28, CategoryId=catSalleManger.Id },
                new Product { Name="Tradition Wood", Description="Chaise au style classique apportant charme et authenticité à votre intérieur.", Price=119.99m, ImageUrl="13.jpg", Stock=20, CategoryId=catSalleManger.Id },
                new Product { Name="OakLine Premium", Description="Chaise en bois massif offrant solidité et durabilité.", Price=129.99m, ImageUrl="14.jpg", Stock=18, CategoryId=catSalleManger.Id },
                new Product { Name="ModernDining", Description="Chaise moderne au design épuré, idéale pour les intérieurs contemporains.", Price=139.99m, ImageUrl="15.jpg", Stock=16, CategoryId=catSalleManger.Id },
                new Product { Name="ElegantSeat", Description="Chaise raffinée avec finition premium pour une table élégante.", Price=149.99m, ImageUrl="16.jpg", Stock=14, CategoryId=catSalleManger.Id },
                new Product { Name="EasyClean Chair", Description="Chaise pratique et facile à nettoyer, parfaite pour un usage quotidien.", Price=99.99m, ImageUrl="17.jpg", Stock=35, CategoryId=catSalleManger.Id },
                new Product { Name="SolidDining", Description="Chaise robuste avec structure stable pour une utilisation durable.", Price=109.99m, ImageUrl="18.jpg", Stock=22, CategoryId=catSalleManger.Id },
                new Product { Name="Timeless Dining", Description="Chaise au design intemporel qui s’adapte à tous les styles.", Price=119.99m, ImageUrl="19.jpg", Stock=19, CategoryId=catSalleManger.Id },
                new Product { Name="FamilyMeal Chair", Description="Chaise conviviale idéale pour les repas en famille.", Price=129.99m, ImageUrl="20.jpg", Stock=17, CategoryId=catSalleManger.Id },

                // ====== GAMING (21 → 30) ======
                new Product { Name="GameForce Core", Description="Chaise gaming ergonomique conçue pour offrir un confort optimal pendant le jeu.", Price=299.99m, ImageUrl="21.jpg", Stock=10, CategoryId=catGaming.Id },
                new Product { Name="PlayerSeat X", Description="Chaise gaming avec coussin lombaire inclus pour un meilleur maintien.", Price=319.99m, ImageUrl="22.jpg", Stock=9, CategoryId=catGaming.Id },
                new Product { Name="ProGamer Elite", Description="Chaise avec repose-tête ajustable pour un soutien complet du corps.", Price=339.99m, ImageUrl="23.jpg", Stock=8, CategoryId=catGaming.Id },
                new Product { Name="RacingStyle Chair", Description="Chaise gaming au design racing inspiré des sièges automobiles.", Price=359.99m, ImageUrl="24.jpg", Stock=7, CategoryId=catGaming.Id },
                new Product { Name="GamerFlex 4D", Description="Chaise haut de gamme avec accoudoirs 4D et réglages avancés.", Price=379.99m, ImageUrl="25.jpg", Stock=6, CategoryId=catGaming.Id },
                new Product { Name="WidePlay Seat", Description="Chaise gaming à assise large offrant liberté de mouvement et confort.", Price=399.99m, ImageUrl="26.jpg", Stock=5, CategoryId=catGaming.Id },
                new Product { Name="RGB Gaming Chair", Description="Chaise gaming avec éclairage RGB intégré pour une immersion totale.", Price=429.99m, ImageUrl="27.jpg", Stock=4, CategoryId=catGaming.Id },
                new Product { Name="PremiumGamer", Description="Chaise conçue avec des matériaux premium pour des performances durables.", Price=449.99m, ImageUrl="28.jpg", Stock=6, CategoryId=catGaming.Id },
                new Product { Name="UltraSession Chair", Description="Chaise idéale pour les longues sessions de jeu avec confort maximal.", Price=469.99m, ImageUrl="29.jpg", Stock=3, CategoryId=catGaming.Id },
                new Product { Name="Gaming Apex", Description="Chaise gaming haut de gamme offrant performance, confort et style.", Price=499.99m, ImageUrl="30.jpg", Stock=2, CategoryId=catGaming.Id },

                // ====== LOUNGE (31 → 40) ======
                new Product { Name="Nordic Relax", Description="Chaise lounge au design scandinave, parfaite pour un intérieur moderne.", Price=259.99m, ImageUrl="31.jpg", Stock=10, CategoryId=catLounge.Id },
                new Product { Name="SoftLounge", Description="Chaise avec tissu doux et confortable pour des moments de détente.", Price=269.99m, ImageUrl="32.jpg", Stock=9, CategoryId=catLounge.Id },
                new Product { Name="WoodRelax Chair", Description="Chaise lounge avec pieds en bois apportant chaleur et élégance.", Price=279.99m, ImageUrl="33.jpg", Stock=8, CategoryId=catLounge.Id },
                new Product { Name="Minimal Lounge", Description="Chaise au style minimaliste, idéale pour les espaces épurés.", Price=289.99m, ImageUrl="34.jpg", Stock=7, CategoryId=catLounge.Id },
                new Product { Name="RelaxMax", Description="Chaise lounge conçue pour une détente maximale.", Price=299.99m, ImageUrl="35.jpg", Stock=6, CategoryId=catLounge.Id },
                new Product { Name="ModernRelax", Description="Chaise lounge moderne combinant esthétique et confort.", Price=309.99m, ImageUrl="36.jpg", Stock=5, CategoryId=catLounge.Id },
                new Product { Name="WideComfort Lounge", Description="Chaise lounge à assise large pour un confort supérieur.", Price=319.99m, ImageUrl="37.jpg", Stock=4, CategoryId=catLounge.Id },
                new Product { Name="Premium Lounge Seat", Description="Chaise lounge avec finitions premium pour un intérieur élégant.", Price=329.99m, ImageUrl="38.jpg", Stock=3, CategoryId=catLounge.Id },
                new Product { Name="LivingRoom Comfort", Description="Chaise parfaite pour le salon, alliant confort et design.", Price=339.99m, ImageUrl="39.jpg", Stock=4, CategoryId=catLounge.Id },
                new Product { Name="ZenRelax Chair", Description="Chaise lounge favorisant relaxation et bien-être au quotidien.", Price=349.99m, ImageUrl="40.jpg", Stock=2, CategoryId=catLounge.Id }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
