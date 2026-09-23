namespace TrainTimetableScripts {
    export class ThemeCookies {
        public static SetThemeCookie(isDarkTheme : boolean) {
            const date = new Date();
            date.setTime(date.getTime() + (24 * 60 * 60 * 1000));

            document.cookie = `IsDarkMode=${isDarkTheme};path=/;expires=${date.toUTCString()};`;
        }

        public static GetThemeCookie() : boolean {
            const cookie = document.cookie.split(";").find(_ => _ == "IsDarkMode=true");
            if (cookie == undefined) {
                return false;
            } else {
                return true;
            }
        }
    }
}