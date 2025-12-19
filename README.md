# Guide_Construction_DB

Les commandes dotnet ef sont spécifiques à Entity Framework Core CLI (outil en ligne de commande EF).

1. Restaurer les dépendances
--> dotnet restore
2. Créer la première migration
--> dotnet ef migrations add InitialCreate
3. Construire la base de données
--> dotnet ef database update
4. Lancer l'application
--> dotnet run

Prérequis
SQL Server installé et démarré
EF Tools : dotnet tool install --global dotnet-ef