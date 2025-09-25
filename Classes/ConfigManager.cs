using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WOTTracker.Models;

namespace WOTTracker
{
    public static class ConfigManager
    {
        private static string _databasePath;
        private static string _appSettingsPath;

        public static void Initialize(string databasePath, string appSettingsPath)
        {
            _databasePath = databasePath;
            _appSettingsPath = appSettingsPath;
        }        /// <summary>
                 /// Fonction principale qui garantit le chargement d'une configuration valide.
                 /// Elle lit le fichier local, le valide, et si nécessaire, lance la récupération ou l'assistant.
                 /// Ne se terminera que lorsqu'une configuration valide sera disponible.
                 /// </summary>
        public static IConfiguration LoadAndSaveFile()
        {

            Log.Information("Start of the configuration loading procedure.");

            // On essaie de lire le fichier JSON.
            if (File.Exists(_appSettingsPath))
            {
                Log.Information("Configuration file found. Attempting validation.");
                try
                {
                    var builder = new ConfigurationBuilder().AddJsonFile(_appSettingsPath, false, true);
                    var config = builder.Build();

                    // Si le fichier est valide (tous les champs sont présents)
                    if (IsConfigurationValid(config))
                    {
                        Log.Information("Configuration loaded and validated successfully from the local file.");
                        // On s'assure que notre backup BDD est à jour.
                        SyncLatestConfigToDatabase(config);
                        return config;
                    }

                    // Le fichier est présent mais INCOMPLET. On lance la récupération.
                    Log.Warning("The configuration file is incomplete. Starting the repair procedure.");
                    return RecoveryFromDatabase(); ;
                }
                catch (JsonReaderException jsonEx)
                {
                    // Le fichier est PRÉSENT mais CORROMPU.On lance la récupération.
                    Log.Error(jsonEx, "The configuration file is corrupted. Starting the repair procedure.");
                    return RecoveryFromDatabase(); ;
                }
            }
            else
            {
                // Le fichier N'EXISTE PAS. On lance la récupération.
                Log.Warning("Configuration file not found. Starting the repair procedure.");
                return RecoveryFromDatabase();
            }
        }


        /// <summary>
        /// Tente de restaurer la configuration depuis la BDD.
        /// Si la restauration échoue, retourne null pour signaler qu'une saisie utilisateur est nécessaire.
        /// </summary>
        /// <returns>L'objet IConfiguration restauré, ou null si la restauration est impossible.</returns>
        private static IConfiguration RecoveryFromDatabase()
        {
            // 1. TENTATIVE DE RÉCUPÉRATION DEPUIS LA BASE DE DONNÉES
            try
            {
                using (var db = new YotTrackerDbContext(_databasePath))
                {
                    var latestBackup = db.AppSettingsHistory.Where(v => v.isActive).OrderByDescending(v => v.ActivationDate).FirstOrDefault();
                    if (latestBackup != null)
                    {
                        Log.Information("Backup found in the database. Restoring the configuration file.");
                        RestoreFileFromBackup(latestBackup);
                        // On recharge la configuration à partir du fichier qu'on vient de créer.
                        return new ConfigurationBuilder().AddJsonFile(_appSettingsPath, false, true).Build();
                    }
                }
            }
            catch (Exception dbEx)
            {
                Log.Error(dbEx, "Unable to connect to the database for retrieval.");
            }

            // 2. ÉCHEC DE LA RÉCUPÉRATION
            Log.Warning("No valid backup found in the database. User input required.");
            return null;
        }

        /// <summary>
        /// Sauvegarde une nouvelle configuration fournie par l'utilisateur 
        /// </summary>
        /// <param name="configDataFromControl">Les données de configuration saisies.</param>
        /// <returns>Le nouvel objet IConfiguration valide.</returns>
        public static IConfiguration SaveNewConfigFromUserInput(AppSettingsHistory configDataFromControl)
        {
            // On recrée la structure JSON complète...
            var fullJsonStructure = CreateFullJsonStructure(configDataFromControl);

            // On l'écrit sur le disque...
            File.WriteAllText(_appSettingsPath, JsonConvert.SerializeObject(fullJsonStructure, Formatting.Indented));

            // On sauvegarde en BDD...
            using (var db = new YotTrackerDbContext(_databasePath))
            {
                // 1. On désactive toutes les autres configurations.
                var currentlyActiveConfigs = db.AppSettingsHistory.Where(c => c.isActive).ToList();
                foreach (var config in currentlyActiveConfigs)
                {
                    config.isActive = false;
                }

                // 2. On définit la nouvelle configuration comme étant la seule active.
                configDataFromControl.isActive = true;
                configDataFromControl.ActivationDate = DateTime.Now;
                db.AppSettingsHistory.Add(configDataFromControl);
                db.SaveChanges();
            }

            Log.Information("Configuration successfully created from the UserControl.");

            // On recharge la config depuis le nouveau fichier et on la retourne.
            return new ConfigurationBuilder().AddJsonFile(_appSettingsPath, false, true).Build();
        }


        /// <summary>
        /// Vérifie que l'objet de configuration contient toutes les clés nécessaires et valides.
        /// </summary>
        /// <param name="config">L'objet IConfiguration à valider.</param>

        private static bool IsConfigurationValid(IConfiguration config)
        {
            // --- 1. Validation du Rôle Utilisateur ---
            var allowedRoles = new[] { "Manager", "Coordinator", "Responsible", "HR" };
            if (string.IsNullOrWhiteSpace(config["UserRole"]) || !allowedRoles.Contains(config["UserRole"]))
            {
                Log.Warning("Validation failed: UserRole is invalid.");
                return false;
            }

            // --- 2. Validation des horaires ---
            if (!TimeSpan.TryParse(config["WorkingHours:Start"], out TimeSpan workStart))
            {
                Log.Warning("Validation failed: WorkingHours:Start is not in the format HH:mm.");
                return false;
            }

            if (!TimeSpan.TryParse(config["WorkingHours:End"], out TimeSpan workEnd))
            {
                Log.Warning("Validation failed: WorkingHours:End is not in the format HH:mm.");
                return false;
            }

            if (!TimeSpan.TryParse(config["BreakHours:Start"], out TimeSpan breakStart))
            {
                Log.Warning("Validation failed: BreakHours:Start is not in the format HH:mm.");
                return false;
            }

            if (!TimeSpan.TryParse(config["BreakHours:End"], out TimeSpan breakEnd))
            {
                Log.Warning("Validation failed: BreakHours:End is not in the format HH:mm.");
                return false;
            }

            // --- 3. Validation des relations horaires ---
            if (workEnd <= workStart)
            {
                Log.Warning("Validation failed: WorkingHours:End must be after WorkingHours:Start.");
                return false;
            }

            if (breakEnd <= breakStart)
            {
                Log.Warning("Validation failed: BreakHours:End must be after BreakHours:Start.");
                return false;
            }

            if (breakStart <= workStart)
            {
                Log.Warning("Validation failed: BreakHours:End must be after BreakHours:Start.");
                return false;
            }

            if (breakEnd >= workEnd)
            {
                Log.Warning("Validation failed: BreakHours:End must be before WorkingHours:End.");
                return false;
            }

            // --- 4. Validation des Tolerances ---
            if (!int.TryParse(config["WorkingHours:Tolerance:StartMinutes"], out _))
            {
                Log.Warning("Validation failed: Tolerance:StartMinutes is not a valid integer.");
                return false;
            }

            if (!int.TryParse(config["WorkingHours:Tolerance:EndMinutes"], out _))
            {
                Log.Warning("Validation failed: Tolerance:EndMinutes is not a valid integer.");
                return false;
            }

            // --- 5. Validation des E-mails ---
            if (!IsValidEmail(config["Email:Recipient"]))
            {
                Log.Warning("Validation failed: Email:Recipient is not a valid email address.");
                return false;
            }

            if (!IsValidEmail(config["Email:Sender"]))
            {
                Log.Warning("Validation failed: Email:Sender is not a valid email address.");
                return false;
            }

            if (!DateTime.TryParse(config["ExpirationOverTime"], out DateTime expirationDate))
            {
                Log.Warning("Validation failed: ExpirationOverTime is not a valid date.");
                return false;
            }

            if (expirationDate.Date <= DateTime.Today)
            {
                Log.Warning("Validation failed: The expiry date for overtime must be in the future.");
                return false;
            }

            // --- Tous les tests sont passés ---
            Log.Information("Configuration validation: all required fields are present and valid.");
            return true;
        }


        /// <summary>
        /// Fonction d'aide pour valider un format d'e-mail simple.
        /// </summary>
        private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <param name="configData">L'objet contenant les données de configuration variables.</param>
    /// <returns>Un objet prêt à être sérialisé en JSON.</returns>
    public static object CreateFullJsonStructure(AppSettingsHistory configData)
        {
            return new
            {
                UserRole = configData.UserRole,
                WorkingHours = new
                {
                    Start = configData.WorkingHoursStart,
                    End = configData.WorkingHoursEnd,
                    Tolerance = new
                    {
                        StartMinutes = configData.ToleranceStartMinutes,
                        EndMinutes = configData.ToleranceEndMinutes
                    }
                },
                BreakHours = new
                {
                    Start = configData.BreakHoursStart,
                    End = configData.BreakHoursEnd
                },
                Email = new
                {
                    Recipient = configData.EmailRecipient,
                    Sender = configData.EmailSender,

                    // ---- Données techniques "fixes" qui ne sont pas en BDD ----
                    SmtpServer = "smtp.gmail.com",
                    SmtpPort = 587,
                    SmtpUsername = "anass.smaiki123@gmail.com",
                    SmtpPassword = "sjrm gdhr zkqt sigw" 
                },
                ExpirationOverTime = configData.ExpirationOverTime.ToString("yyyy-MM-dd"), 

                FilePath = "appsetings.json", 
                LogFilePath = "log.txt"         
            };
        }

        /// <summary>
        /// Crée ou écrase le fichier appsettings.json local à partir d'un objet de configuration.
        /// </summary>
        /// <param name="backup">L'objet de configuration (venant de la BDD) à utiliser.</param>
        private static void RestoreFileFromBackup(AppSettingsHistory backup)
        {
            Log.Information("Restoration of the configuration file from the database (VersionId {Id})...", backup.VersionId);

            // On utilise la fonction utilitaire pour construire la structure JSON complète.
            var fullJsonStructure = CreateFullJsonStructure(backup);

            try
            {
                // On sérialise (convertit en texte) et on écrit le fichier.
                string jsonToRestore = JsonConvert.SerializeObject(fullJsonStructure, Formatting.Indented);
                File.WriteAllText(_appSettingsPath, jsonToRestore);
                Log.Information("The appsettings.json file has been successfully restored.");
            }
            catch (Exception ex)
            {
                // Gestion du cas rare où l'écriture du fichier échoue (problème de droits, disque plein...)
                Log.Fatal(ex, "Failed to write the restored configuration file. This is a critical error.");
                MessageBox.Show("Critical error: unable to write the restored configuration file.", "Writing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit(); // Si on ne peut pas écrire la config, on ne peut pas continuer.
            }
        }

        /// <summary>
        /// Compare la configuration actuelle (chargée du fichier) avec la dernière version
        /// en base de données, et crée une nouvelle version si des changements sont détectés.
        /// </summary>
        /// <param name="currentConfig">La configuration chargée depuis le fichier.</param>
        private static void SyncLatestConfigToDatabase(IConfiguration currentConfig)
        {
            try
            {
                // On désérialise le fichier JSON dans un objet AppSettingsHistory pour pouvoir le comparer et le sauvegarder.
                var fileConfig = new AppSettingsHistory
                {
                    UserRole = currentConfig["UserRole"],
                    WorkingHoursStart = currentConfig["WorkingHours:Start"],
                    WorkingHoursEnd = currentConfig["WorkingHours:End"],
                    ToleranceStartMinutes = int.Parse(currentConfig["WorkingHours:Tolerance:StartMinutes"]),
                    ToleranceEndMinutes = int.Parse(currentConfig["WorkingHours:Tolerance:EndMinutes"]),
                    BreakHoursStart = currentConfig["BreakHours:Start"],
                    BreakHoursEnd = currentConfig["BreakHours:End"],
                    EmailRecipient = currentConfig["Email:Recipient"],
                    EmailSender = currentConfig["Email:Sender"],
                    ExpirationOverTime = DateTime.Parse(currentConfig["ExpirationOverTime"]),
                };

                using (var db = new YotTrackerDbContext(_databasePath))
                {
                    var latestDbConfig = db.AppSettingsHistory.Where(v => v.isActive).FirstOrDefault();

                    // On ne crée une nouvelle version que si la BDD est vide ou si la config a réellement changé.
                    if (latestDbConfig == null || HasConfigChanged(fileConfig, latestDbConfig))
                    {
                        Log.Information("Configuration change detected. Creating a new backup version in the database.");

                        var currentlyActiveConfigs = db.AppSettingsHistory.Where(c => c.isActive).ToList();

                        foreach (var config in currentlyActiveConfigs)
                        {
                            config.isActive = false;
                        }

                        // 3. On définit la nouvelle configuration comme étant la seule active.
                        fileConfig.isActive = true;
                        // ------------------------


                        fileConfig.ActivationDate = DateTime.Now;
                        db.AppSettingsHistory.Add(fileConfig);
                        db.SaveChanges();
                        Log.Information("New version saved successfully (VersionId {Id}).", fileConfig.VersionId);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Configuration synchronization to the database failed.");
                // Ce n'est pas une erreur fatale, l'application peut continuer. On logue juste l'erreur.
            }
        }

        // Fonction utilitaire de comparaison
        private static bool HasConfigChanged(AppSettingsHistory fileConfig, AppSettingsHistory dbConfig)
        {
            return fileConfig.UserRole != dbConfig.UserRole ||
                   fileConfig.WorkingHoursStart != dbConfig.WorkingHoursStart ||
                   fileConfig.WorkingHoursEnd != dbConfig.WorkingHoursEnd ||
                   fileConfig.ToleranceStartMinutes != dbConfig.ToleranceStartMinutes ||
                   fileConfig.ToleranceEndMinutes != dbConfig.ToleranceEndMinutes ||
                   fileConfig.BreakHoursStart != dbConfig.BreakHoursStart ||
                   fileConfig.BreakHoursEnd != dbConfig.BreakHoursEnd ||
                   fileConfig.EmailRecipient != dbConfig.EmailRecipient ||
                   fileConfig.EmailSender != dbConfig.EmailSender ||
                   fileConfig.ExpirationOverTime.Date != dbConfig.ExpirationOverTime.Date; // On compare juste la date, pas l'heure

        }


    }
}