# WOTTracker - YAZAKI Over Time Tracker 🕒

![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![Version](https://img.shields.io/badge/version-1.0-blue)
![License](https://img.shields.io/badge/license-MIT-lightgrey)

**WOTTracker** (Work OverTime Tracker) est une application de bureau pour Windows conçue pour automatiser le suivi du temps de travail effectif d'un utilisateur sur son poste de travail. L'application fonctionne en arrière-plan pour enregistrer de manière fiable les périodes d'activité et d'inactivité en se basant sur les événements du système (démarrage, arrêt, veille). Conçue pour une résilience maximale, elle est capable de reconstruire l'historique de travail même après des plantages du système ou de l'application.

---

## ✨ Fonctionnalités Clés

* **Suivi Automatisé et Fiable** : Enregistre les sessions de travail en se basant sur les événements système (Démarrage, Arrêt, Veille, Réveil).
* **Récupération de Données Robuste** : Utilise le journal d'événements Windows comme source de vérité pour reconstituer les périodes de travail manquées et garantir qu'aucune donnée n'est perdue.
* **Gestion Intelligente des Absences** : Un module dédié permet de justifier les absences ou de les compenser avec le solde d'heures supplémentaires disponible.
* **Configuration Flexible et Versionnée** : Un assistant de première exécution et un écran de paramètres permettent de configurer les heures de travail et les préférences. Chaque changement de configuration est versionné dans la base de données pour assurer l'auditabilité des calculs passés.
* **Interface en Temps Réel** : Un tableau de bord principal affiche en direct le statut de la session en cours et les soldes d'heures actualisés.
* **Rapports et Analyses Avancés** :
    * Exportez des rapports complexes sur plusieurs feuilles Excel.
    * Envoyez des résumés par e-mail.
    * Visualisez des statistiques détaillées (répartition des heures supplémentaires, risque d'épuisement professionnel, etc.).

---

## 📸 Présentation Détaillée de l'Application

Voici un aperçu des fonctionnalités clés de WOTTracker, illustré par les écrans principaux de l'application.

### 1. Configuration Initiale et Paramètres

<img src="images/Configuration.JPG" alt="Écran de configuration" width="600">

> L'application guide l'utilisateur à travers un assistant de configuration lors du premier lancement. Cet écran permet de définir tous les paramètres essentiels : les heures de travail et de pause, les tolérances, la configuration des e-mails et les règles d'expiration des heures supplémentaires.

### 2. Démarrage de Session : Choix du Lieu de Travail

<img src="images/WorkingType.JPG" alt="Sélection du lieu de travail" width="600">

> Au début de chaque nouvelle session de travail, l'utilisateur est invité à spécifier son lieu de travail (par exemple, "HomeOffice" ou "On-Site"). Cette information contextuelle permet d'enrichir le suivi et peut déclencher des actions spécifiques, comme l'envoi de notifications.

### 3. Tableau de Bord en Temps Réel

<img src="images/LiveDisplay.JPG" alt="Affichage en temps réel" width="600">

> L'écran principal offre une vue d'ensemble en direct de la session en cours. Il affiche le statut actuel, l'heure de début, le solde total des heures supplémentaires et le temps d'absence ("DownTime") à justifier. Une répartition des heures supplémentaires par catégorie (jours normaux, samedis, dimanches) est également visible.

### 4. Gestion et Compensation des Absences

<img src="images/CompensationDownTime.JPG" alt="Compensation des absences" width="600">

> Lorsque des périodes d'absence non justifiées sont détectées, cette interface s'affiche. Elle liste tous les "DownTime" et permet à l'utilisateur de les compenser en utilisant son solde d'heures supplémentaires disponible, affiché en haut de l'écran.

### 5. Analyses et Statistiques Avancées

<img src="images/Dashboard.JPG" alt="Tableau de bord et statistiques" width="600">

> Pour une analyse plus approfondie, le tableau de bord statistique offre des visualisations de données claires. Il inclut :
> * **Une répartition des heures supplémentaires** par type de jour (graphique circulaire).
> * **Une analyse des jours les plus productifs** en heures supplémentaires (graphique à barres).
> * **La liste des 5 plus longues sessions** de travail enregistrées.
> * **Un indicateur de risque de burnout** pour un suivi proactif du bien-être.

---

## 🛠️ Stack Technique

L'application est construite sur un ensemble de technologies fiables de l'écosystème .NET.

### Core Stack
* **Langage** : C#
* **Framework** : .NET Framework avec Windows Forms
* **Base de données** : SQL Server Compact Edition 4.0
* **ORM** : Entity Framework Core (via un fournisseur communautaire pour SQL CE)

### Librairies NuGet Clés
* **Serilog** : Pour une journalisation structurée et avancée de l'application.
* **ClosedXML** : Pour générer des rapports Excel .xlsx modernes sans installation d'Office.
* **Newtonsoft.Json** : Pour la sérialisation/désérialisation des objets JSON.
* **System.Windows.Forms.DataVisualization** : Librairie officielle Microsoft pour la création de graphiques.

### Framework de Test
* **xUnit** : Utilisé pour écrire des tests unitaires automatisés qui valident la logique métier complexe de l'application.

---

## 🏗️ Architecture et Conception

L'application est basée sur une architecture modulaire qui met l'accent sur le principe de séparation des préoccupations.

* `/Classes` : Contient les classes "manager" qui orchestrent la logique métier (`ConfigManager`, `SessionManager`, etc.).
* `/Models` : Contient les classes du modèle de données (POCOs) qui représentent les tables de la base de données.
* `/UserControls` : Contient les composants de l'interface utilisateur qui sont affichés dynamiquement dans la fenêtre principale.

---

## 🚀 Démarrage

Pour lancer ce projet sur votre machine, suivez ces étapes.

### Prérequis
* Visual Studio 2019 ou plus récent
* .NET Framework 4.8

### Installation
1.  Clonez le dépôt sur votre machine :
    ```bash
    git clone [https://github.com/SmaikiAnas2003/WOTTracker-code.git](https://github.com/SmaikiAnas2003/WOTTracker-code.git)
    ```
2.  Ouvrez le fichier de solution (`.sln`) dans Visual Studio.
3.  Restaurez les packages NuGet (normalement automatique).
4.  Lancez le projet en mode Débogage (`F5`).

---

## ✍️ Auteur

* **Anas Smaiki**
