using System.Windows.Input;

namespace EdytorEtykiet.Helpers
{
    public static class CommandHandler
    {
        public static readonly RoutedUICommand ZapiszPlik = new RoutedUICommand
            (
                "Zapisz",
                "ZapiszPlik",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.S, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand OtworzPlik = new RoutedUICommand
            (
                "Otwórz",
                "OtworzPlik",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.O, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand NowaEtykieta = new RoutedUICommand
            (
                "Nowa etykieta",
                "NowaEtykieta",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.N, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand Zamknij = new RoutedUICommand
            (
                "Zamknij program",
                "ZamknijProgram",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
                }
            );

        public static readonly RoutedUICommand Ok = new RoutedUICommand
            (
                "OK",
                "Ok",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F1)
                }
            );

        public static readonly RoutedUICommand Anuluj = new RoutedUICommand
            (
                "Anuluj",
                "Anuluj",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Escape)
                }
            );
        public static readonly RoutedUICommand NowyTekst = new RoutedUICommand
            (
                "Tekst",
                "NowyTekst",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D1, ModifierKeys.Control)
                }
            );
        public static readonly RoutedUICommand NowyObraz = new RoutedUICommand
            (
                "Obraz",
                "NowyObraz",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D2, ModifierKeys.Control)
                }
            );
        public static readonly RoutedUICommand NowyKodKr = new RoutedUICommand
            (
                "Kod kreskowy",
                "NowyKodKr",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D3, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand EdytujElement = new RoutedUICommand
            (
                "Edytuj",
                "EdytujElement",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.E, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand UsunElement = new RoutedUICommand
            (
                "Usuń",
                "UsunElement",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Delete)
                }
            );

        public static readonly RoutedUICommand Drukuj = new RoutedUICommand
            (
                "Drukuj",
                "Drukuj",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                            new KeyGesture(Key.P, ModifierKeys.Control)
                }
            );
    }
}
