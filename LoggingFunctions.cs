using System;
using System.IO.Ports;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Reflection.Metadata;

namespace MyNamespace
{
    public class Logger
    {
        public static void logging1000Val()
        {
            string portName = "COM10";
            // Console.WriteLine("Welches Messgerät?");
            string measurementDevice =  "4" ;// Console.ReadLine() ?? "NaN";  // Nimmt den Messwert in mm vom Benutzer entgegen.

            Console.WriteLine("Welche Werte in mm?");
            string measurementValue = Console.ReadLine() ?? "NaN";  // Nimmt den Messwert in mm vom Benutzer entgegen.3

            string fileName = $"{measurementValue}mm_M{measurementDevice}_Coated_{DateTime.Now:ddMMyyyy_HHmm}.txt";
            using (SerialPort port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One))
            {
                try
                {
                    port.Open();
                    Console.WriteLine($"Port {portName} geöffnet. Empfangene Daten werden in die Datei {fileName} geschrieben.");
                    using (StreamWriter writer = new StreamWriter("C:/Users/marco/OneDrive - OST/General/03_HW/03_SENSOREINHEIT/08_Logging_Auswertung/03_Primitivo/"+ fileName, true)) 
/*                     using (StreamWriter writer = new StreamWriter("../../../calibrationData/Calibration_Supply_1V8/"+ fileName, true)) */
                    {
                        writer.WriteLine($"COM-Port: {portName}");
                        writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        writer.WriteLine($"Messgerät: {measurementDevice}");
                        writer.WriteLine($"Versorgungsspannung 2.7997 V");
                        writer.WriteLine($"Referenzspannung 0.55 V");
                        writer.WriteLine($"Widerstand: 1MOhm");
                        int count = 0;
                        string dataToWrite = "";

                        while (true)
                        {
                            string line = port.ReadLine();
                            string logEntry = $"{line}";
                            dataToWrite += logEntry + "\n";

                            count++;
                            if (count >= 10004)
                            {
                                writer.Write(dataToWrite);
                                writer.Flush(); // Stellen Sie sicher, dass alle Daten sofort in die Datei geschrieben werden
                                dataToWrite = ""; // Reset the data buffer
                                count = 0; // Reset the counter
                                Console.WriteLine("Programm wird beendet...");
                                Environment.Exit(0); // 0 üblicherweise für erfolgreiches Beenden
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
                }
            }
        }

        public static void LoggingCapacitor()
        {
            string portName = "COM5";
            Console.WriteLine("Welcher Kondensator?");
            string measurementDevice = Console.ReadLine() ?? "NaN";  // Nimmt den Messwert in mm vom Benutzer entgegen.

/*             Console.WriteLine("Welche Werte in mm?");
            string measurementValue = Console.ReadLine() ?? "NaN";  // Nimmt den Messwert in mm vom Benutzer entgegen.3 */
            string measurementValue = "80";
            string fileName = $"{measurementValue}mm_T{measurementDevice}_{DateTime.Now:ddMMyyyy_HHmmss}.txt";
            using (SerialPort port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One))
            {
                try
                {
                    port.Open();
                    Console.WriteLine($"Port {portName} geöffnet. Empfangene Daten werden in die Datei {fileName} geschrieben.");
                    using (StreamWriter writer = new StreamWriter("C:/work/dataLogging/calibrationData/Hysteresis/"+ fileName, true))
                    {
/*                         writer.WriteLine($"COM-Port: {portName}"); */
                        writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        writer.WriteLine($"T{measurementDevice}");
                        writer.WriteLine($"Versorgungsspannung 2.8030 V");
                        writer.WriteLine($"Referenzspannung 0.55 V");
                        writer.WriteLine($"Widerstand: 1MOhm");
                        int count = 0;
                        string dataToWrite = "";

                        while (true)
                        {
                            string line = port.ReadLine();
                            string logEntry = $"{line}";
                            dataToWrite += logEntry;

                            count++;
                            if (count >= 1000)
                            {
                                writer.Write(dataToWrite);
                                writer.Flush(); // Stellen Sie sicher, dass alle Daten sofort in die Datei geschrieben werden
                                dataToWrite = ""; // Reset the data buffer
                                count = 0; // Reset the counter
                                Console.WriteLine("Programm wird beendet...");
                                Environment.Exit(0); // 0 üblicherweise für erfolgreiches Beenden
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
                }
            }
        }

        public static void fileInit(string portName)
        {
            
            Console.WriteLine($"Bitte geben Sie das Messgerät für {portName} ein:");
            string measurementDevice = Console.ReadLine() ?? "NaN";

            string filename = $"M{measurementDevice}_ERDE_{DateTime.Now:yyyyMMddHHmmss}.txt";
            Console.WriteLine("Es wurde ein File erstellt");
            try{
                string path = Path.Combine("..", "..", "..", "calibrationData", "refMeasInEarth", filename);
                using (StreamWriter writer = new StreamWriter(path, true)){
                        writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
                        writer.WriteLine($"Messgerät: {measurementDevice}");
                        writer.WriteLine($"Versorgungsspannung 2.8002 V");
                        writer.WriteLine($"Referenzspannung 0.55 V");
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Der angegebene Pfad wurde nicht gefunden: " + e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Keine Zugriffsberechtigung auf den Dateipfad: " + e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine("Fehler beim Schreiben in die Datei: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ein unerwarteter Fehler ist aufgetreten: " + e.Message);
            }
        }


        // neu
        public static LoggingSession InitLoggingSession(string portName)
        {
            Console.WriteLine($"Bitte geben Sie das Messgerät für {portName} ein:");
            string measurementDevice = Console.ReadLine() ?? "NaN";
            string filename = $"M{measurementDevice}_ERDE_{DateTime.Now:ddMMHHmmss}.csv";
            string filePath = Path.Combine(@"C:\Users\marco\OneDrive - OST\General\03_HW\03_SENSOREINHEIT\08_Logging_Auswertung\01_PCB\04_Auswertung_Langzeitmessung", filename);

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Messgerät: {measurementDevice}");
                    writer.WriteLine($"Port: {portName}");
                    writer.WriteLine($"Erstellungszeitpunkt: {DateTime.Now:dd-MM-yyyy HH:mm}");
                }
                return new LoggingSession(portName, filePath); // Stellen Sie sicher, dass das Rückgabeobjekt erstellt wird
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ein unerwarteter Fehler ist aufgetreten: {e.Message}");
                return null; // Geben Sie null oder eine alternative Fehlerbehandlung zurück
            }
        }

        public static void StartLogging(LoggingSession session, int nOfRuns)
        {
            using (SerialPort port = new SerialPort(session.PortName, 9600, Parity.None, 8, StopBits.One))
            {
                try
                {
                    port.Open();
                    Console.WriteLine($"Port {session.PortName} geöffnet. Empfangene Daten werden in die Datei {session.FilePath} geschrieben.");
                    using (StreamWriter writer = new StreamWriter(session.FilePath, true))
                    {
                        int dataPoints = nOfRuns; // Anzahl der Messpunkte pro Minute

                        while (true)
                        {
                            if (DateTime.Now.Second == 20)
                            {
                                break;
                            }
                            Thread.Sleep(100); // Überprüfe jede 100 Millisekunden, ob die Sekunde gleich 0 ist
                        }

                            while (true)
                        {
                            // Beginne eine neue Zeile mit dem aktuellen Zeitstempel
                            DateTime startTime = DateTime.Now; // Startzeit erfassen
                            string dataToWrite = DateTime.Now.ToString("dd.MM.yyyy ")+ DateTime.Now.ToString("HH:mm:ss") + "," ;

                            // Sammle die Datenpunkte in derselben Zeile
                            for (int i = 0; i < dataPoints; i++)
                            {
                                string line = port.ReadLine();
                                dataToWrite += line + (i < (dataPoints - 1) ? "," : ""); // Fügt Daten hinzu und trennt sie durch Kommas, außer am Ende
                            }

                            // Schreibe die vollständige Zeile mit Zeitstempel und Daten11
                            writer.WriteLine(dataToWrite);
                            writer.Flush(); // Sicherstellen, dass die Daten sofort in die Datei geschrieben werden

                            Console.WriteLine("Daten geschrieben. Warte 1 Minute bis zum nächsten Durchgang.");
                        DateTime endTime = DateTime.Now; // Endzeit erfassen
                        TimeSpan readDuration = endTime - startTime; // Differenz berechnen
                        int readDurationMilliseconds = (int)readDuration.TotalMilliseconds; // In Ganzzahl umwandeln

                        Console.WriteLine($"Daten geschrieben. Lesevorgang dauerte {readDurationMilliseconds} Millisekunden. Warte 1 Minute bis zum nächsten Durchgang.");

                        // Warte bis zur nächsten vollen Minute
                        int millisecondsUntilNextMinute = 60000 - readDurationMilliseconds;
                        Thread.Sleep(millisecondsUntilNextMinute);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ein Fehler ist aufgetreten: {ex.Message}");
                }
            }
        }
    }

    public class LoggingSession
    {
        public string PortName { get; set; }
        public string FilePath { get; set; }

        public LoggingSession(string portName, string filePath)
        {
            PortName = portName;
            FilePath = filePath;
        }
    }
}


