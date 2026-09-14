"use strict";
var TrainTimetableScripts;
(function (TrainTimetableScripts) {
    class ThemeCookies {
        static SetThemeCookie(isDarkTheme) {
            const date = new Date();
            date.setTime(date.getTime() + (24 * 60 * 60 * 1000));
            document.cookie = `IsDarkMode=${isDarkTheme};path=/;expires=${date.toUTCString()};`;
        }
        static GetThemeCookie() {
            const cookie = document.cookie.split(";").find(_ => _ == "IsDarkMode=true");
            if (cookie == undefined) {
                return false;
            }
            else {
                return true;
            }
        }
    }
    TrainTimetableScripts.ThemeCookies = ThemeCookies;
})(TrainTimetableScripts || (TrainTimetableScripts = {}));
//# sourceMappingURL=Cookies.js.map