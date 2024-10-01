using System;
using System.Collections.Generic;
using System.Threading;

namespace Quiz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Benutzer nach ihrem Namen fragen
            Console.WriteLine("Bitte geben Sie Ihren Namen ein: ");
            string name = Console.ReadLine();

            // Aktuelle Stunde abrufen
            int time = DateTime.Now.Hour;

            // Begrüßung basierend auf der Tageszeit
            if (time < 12)
            {
                Console.WriteLine("Guten Morgen, " + name + "!");
            }
            else if (time < 18)
            {
                Console.WriteLine("Guten Tag, " + name + "!");
            }
            else
            {
                Console.WriteLine("Guten Abend, " + name + "!");
            }

            // Willkommensnachricht für das Quiz
            Console.WriteLine("Willkommen beim Quizgame. Sie können zwischen Logikrätseln und mathematischen Rätseln entscheiden.\n Für jede Frage haben Sie 30 Sekunden Zeit.");
            Console.WriteLine("1. Logikrätsel");
            Console.WriteLine("2. Mathematikrätsel");

            int totalPoints = 0;
            bool playAgain = true;

            while (playAgain)
            {
                Console.Write("Bitte wählen Sie eine Kategorie (1 oder 2): ");
                string choice = Console.ReadLine();

                int pointsEarned = 0; // Variable für die in der Runde gewonnenen Punkte

                if (choice == "1")
                {
                    pointsEarned = PlayLogicGame(); // Punkte aus Logikspiel
                }
                else if (choice == "2")
                {
                    pointsEarned = PlayMathGame(); // Punkte aus Mathespiel
                }
                else
                {
                    Console.WriteLine("Ungültige Eingabe. Bitte wählen Sie 1 oder 2.");
                    continue;
                }

                totalPoints += pointsEarned; // Aktualisiere die Gesamtpunkte mit den in der Runde erhaltenen Punkten

                // Verhindern, dass die Gesamtpunktzahl unter 0 fällt
                if (totalPoints < 0) totalPoints = 0;

                Console.WriteLine($"Ihre Gesamtpunkte: {totalPoints}");
                Console.Write("Möchten Sie eine weitere Runde spielen? (ja/nein): ");
                string playAgainInput = Console.ReadLine().ToLower();
                playAgain = playAgainInput == "ja";
            }

            Console.WriteLine("Danke fürs Spielen! Ihre Endpunktzahl ist: " + totalPoints);
        }

        static int PlayMathGame()
        {
            int score = 0;
            int rounds = 5;

            Console.WriteLine("\n--- Mathematische Rätsel ---");
            for (int i = 0; i < rounds; i++)
            {
                int num1 = new Random().Next(1, 20);
                int num2 = new Random().Next(1, 20);

                // Erstellen eines Arrays von Operatoren
                char[] signs = { '+', '-', '*', '/' };
                // Wählen eines zufälligen Operators aus dem Array
                char sign = signs[new Random().Next(0, signs.Length)];

                // Berechnung der richtigen Antwort basierend auf dem Operator
                int correctAnswer;
                switch (sign)
                {
                    case '+':
                        correctAnswer = num1 + num2;
                        break;
                    case '-':
                        correctAnswer = num1 - num2;
                        break;
                    case '*':
                        correctAnswer = num1 * num2;
                        break;
                    case '/':
                        // Stellen Sie sicher, dass wir durch eine Zahl > 0 teilen
                        // (hier wird num2 auf einen Wert > 0 beschränkt)
                        correctAnswer = num2 != 0 ? num1 / num2 : num1;
                        break;
                    default:
                        throw new InvalidOperationException("Unbekannter Operator");
                }

                // Ausgabe der Frage
                Console.WriteLine($"Was ist {num1} {sign} {num2}?");

                // Setze ein Zeitlimit von 8 Sekunden
                bool answered = false; // initialisiert answered mit false, was bedeutet, dass die Antwort des Benutzers noch nicht gegeben wurde
                Timer timer = new Timer(_ => //eine Klasse, die es ermöglicht, einen Zeitgeber zu erstellen, der einen bestimmten Code nach einer festgelegten Zeit ausführt
                {
                    Console.WriteLine("\nZeit abgelaufen! Diese Antwort wird als falsch gewertet.");
                    answered = true;
                }, null, 30000, Timeout.Infinite); // gibt an, dass der Timer nicht automatisch wiederholt werden soll

                //Antwortin Variable speichern 
                string answer = Console.ReadLine();
                timer.Change(Timeout.Infinite, Timeout.Infinite); // Stoppe den Timer, Startzeit, Wiederholungszeit

                if (answered) continue; // Wenn die Zeit abgelaufen ist, gehe zur nächsten Frage
                //Benutzerantwort überprüfen 
                if (int.TryParse(answer, out int userAnswer)) // out bedeutet auserhalb des Aufrufs sichtbar
                {
                    if (userAnswer == correctAnswer)
                    {
                        Console.WriteLine("Richtig! Sie erhalten 2 Punkte.");
                        score += 2;
                    }
                    else
                    {
                        Console.WriteLine($"Falsch! Die richtige Antwort ist {correctAnswer}. Sie verlieren 1 Punkt.");
                        score -= 1;
                    }
                }
                else
                {
                    Console.WriteLine("Ungültige Eingabe. Sie verlieren 1 Punkt.");
                    score -= 1;
                }
                // Verhindern, dass die Punktzahl unter 0 fällt
                if (score < 0) score = 0;
            }

            return score;
        }

        static int PlayLogicGame()
        {
            int score = 0;

            Console.WriteLine("\n--- Logische Rätsel ---");
            List<LogicQuestion> questions = new List<LogicQuestion>
            {
                new LogicQuestion("Was ist schwerer als ein Elefant, aber wiegt nichts?", "Schatten"),
                new LogicQuestion("Je mehr du hast, desto weniger siehst du. Was ist das?", "Dunkelheit"),
                new LogicQuestion("Was hat ein Herz, schlägt aber nicht?", "Artischocke"),
            };

            foreach (var question in questions)
            {
                Console.WriteLine(question.Question);

                // Setze ein Zeitlimit von 8 Sekunden
                bool answered = false;
                Timer timer = new Timer(_ =>
                {
                    Console.WriteLine("\nZeit abgelaufen! Diese Antwort wird als falsch gewertet.");
                    answered = true;
                }, null, 30000, Timeout.Infinite);

                string answer = Console.ReadLine();
                timer.Change(Timeout.Infinite, Timeout.Infinite); // Stoppe den Timer

                if (answered) continue; // Wenn die Zeit abgelaufen ist, gehe zur nächsten Frage

                // Wenn die Antwort richtig ist
                if (answer.Trim().Equals(question.CorrectAnswer, StringComparison.OrdinalIgnoreCase)) // StringComparison.OrdinalIgnoreCase stellt sicher, dass Groß- und Kleinschreibung beim Vergleich ignoriert werden
                {
                    Console.WriteLine("Richtig! Sie erhalten 2 Punkte.");
                    score += 2;
                }
                else
                {
                    Console.WriteLine($"Falsch! Die richtige Antwort ist {question.CorrectAnswer}. Sie verlieren 1 Punkt.");
                    score -= 1;
                }
                // Verhindern, dass die Punktzahl unter 0 fällt
                if (score < 0) score = 0;
            }

            return score;
        }
    }

    public class LogicQuestion
    {
        public string Question { get; }
        public string CorrectAnswer { get; }

        public LogicQuestion(string question, string correctAnswer)
        {
            Question = question;
            CorrectAnswer = correctAnswer;
        }
    }
}
