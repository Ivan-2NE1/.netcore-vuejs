// Import any components that you need to use, and make sure to expose them in the components section, too!
import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../components/base/input-field/input-field.vue";
import InputCalendar from "../components/base/input-calendar/input-calendar.vue";
import InputTime from "../components/base/input-time/input-time.vue";
import vSelect from "../components/base/vue-selectbase/vue-selectbase.vue";
import ParticipatingTeams from "../components/gamereport/participating-teams.vue";
import GameReportMeta from "../components/gamereport/gamereport-meta.vue";
import EventTypeList from "../components/select-lists/eventtype-list.vue";

// Give your app a unique name!
var gamePlayByPlay = new Vue({
    // The main element that app will attach itself to
    el: '#vueApp',

    components: {
        // Register components that are used
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "input-calendar": InputCalendar,
        "input-time": InputTime,
        "vue-selectbase": vSelect,
        "participating-teams": ParticipatingTeams,
        "gamereport-meta": GameReportMeta,
        "eventtype-list": EventTypeList,
    },

    data: {
        // Reactive properties
        gameReport: window.gameReport,
        validationMsgs: [],
        isAdmin: false,
    },

    created() {
        // Load whatever we need to get via ajax (not included in the Model)
        // Setup any non-reactive properties here
        // load the sport + game meta configuration
        var vm = this;

        $.get("/siteapi/auth/isadmin", function (response) {
            var admin = (response !== undefined && response !== null && (response === "true" || response === true));
            vm.isAdmin = admin;
        });
    },

    mounted() {
        // Anything that needs to take place after the app is mounted
        var vm = this;

    },

    computed: {
        // Computed properties
    },

    watch: {
        // Data and Computed properties to watch
    },

    methods: {
        // Do stuff!
        setEventType(event) {
            if (event === undefined || event === null) {
                console.log("event is undefined or null in setEventType");
            }

            console.log(event.id);
        },

        setGameDate(newval) {
            var m = moment(newval, "MM/DD/YYYY");
            var valid = m.isValid(); // false            
            var refDate = new Date(this.gameReport.gameDate);
            if (valid) {
                refDate.setFullYear(m.year());
                refDate.setMonth(m.month());
                refDate.setDate(m.date());
            } else {
                refDate.setFullYear(1);
                refDate.setMonth(0);
                refDate.setDate(1);
            }
            this.gameReport.gameDate = refDate.toJSON();
        },

        setGameTime(newval) {
            var m = moment(newval, "h:mm A");
            var valid = m.isValid();
            var refDate = new Date(this.gameReport.gameDate);
            if (valid) {
                refDate.setHours(m.hour());
                refDate.setMinutes(m.minute());
            } else {
                refDate.setHours(0);
                refDate.setMinutes(0);
            }
            this.gameReport.gameDate = refDate.toJSON();
        },

        onGameTeamsUpdated(newval) {
            this.gameReport.participatingTeams = newVal;
        },

        onGameMetaUpdated(newval) {
            this.gameReport.meta = newVal;
        },

        saveGameReport() {
            var vm = this;

            var msgs = [];

            var initDate = new moment("1/1/0001");
            var gameDate = new moment(vm.gameReport.gameDate);
            if (vm.gameReport.gameDate === null || vm.gameReport.gameDate === "" || gameDate.year() < 1900) {
                msgs.push("The date of the game is required (date the game was played)");
            } else {
                var now = new moment().endOf("day");
                if (!gameDate.isValid) {
                    msgs.push("Could not parse the game date. Check your entry and try again.");
                } else if (gameDate.isAfter(now)) {
                    msgs.push("A game you are reporting as final must take place either today, or in the past");
                }
            }

            //TODO: validate opponents (must have at least one opp that can eventually resolve back to something?)

            if (msgs.length > 0) {
                // there are errors in the game report, do not proceed
                this.validationMsgs = msgs;
                return;
            }

            // we are ready to submit the game report

            var oRequest = JSON.parse(JSON.stringify(this.gameReport));
            // clear out ancillary parts we don't want
            oRequest.sport = null;

            $.ajax({
                method: "POST",
                url: "/SiteApi/Game/" + vm.gameReport.gameReportId,
                contentType: "application/json; charset=utf-8",
                data: oRequest,
                dataType: "json"
            })
                .done(function (data) {
                    if (data.success) {
                        // message that the changes were saved
                    } else {
                        // there was an error somewhere in the process that we need to let them know about
                        // message that there was a problem saving the changes
                    }
                }, "json");
        },
    }
});
