// moment 
//import moment from 'moment';

Vue.filter("numeric",
    function (value) {
        if (value === undefined || value === null) {
            return value;
        }
        return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    });

Vue.filter("currency", function (value) {
    if (value === undefined || value === null) return "";
    return "$" + parseFloat(value).toFixed(2);
});

Vue.filter("ordinal", function (value) {
    if (value === undefined || value === null || isNaN(value)) return value;
    let s = ["th", "st", "nd", "rd"]
    let v = value % 100;
    return value + (s[(v - 20) % 10] || s[v] || s[0]);
});

Vue.filter("pluralize", function(value, singular, plural) {
    if (!value) return plural;
    if (value === 1) return singular;
    return plural;
});

Vue.filter("pluralizer", function (value, singular, plural) {
    if (!value) return plural.replace("{0}", 0);
    if (value === 1) return singular.replace("{0}", value);
    return plural.replace("{0}", value);
});

Vue.filter("formatDate", function(value) {
    if (!value) return "";
    return moment(String(value)).format("MM/DD/YYYY");
});

Vue.filter("formatFullDate", function (value) {
    if (!value) return "";
    return moment(String(value)).format("MMMM D, YYYY");
});

Vue.filter("formatFullDateWithTime", function (value) {
    if (!value) return "";
    return moment(String(value)).format("MMMM D, YYYY h:mma");
});

Vue.filter("compactDateWithTime", function (value) {
    if (!value) return "";
    return moment(String(value)).format("MM/DD/YYYY h:mma");
});

Vue.filter("formatGameTime", function (value) {
    if (!value) return "";
    var oMoment = moment(String(value));
    var minutes = oMoment.minutes() + (oMoment.hours() * 60);
    if (minutes === 0) {
        return "TBA";
    }
    return oMoment.format("h:mm a");
});