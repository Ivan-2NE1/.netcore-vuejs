// Import any components that you need to use, and make sure to expose them in the components section, too!
import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../components/base/input-field/input-field.vue";
import InputTextarea from "../components/base/input-textarea.vue";
import InputCalendar from "../components/base/input-calendar/input-calendar.vue";
import InputTime from "../components/base/input-time/input-time.vue";
import InputDateTime from "../components/base/input-datetime/input-datetime.vue";
import vSelect from "../components/base/vue-selectbase/vue-selectbase.vue";
import ParticipatingTeams from "../components/gamereport/participating-teams.vue";
import GameReportMeta from "../components/gamereport/gamereport-meta.vue";
import EventTypeList from "../components/select-lists/eventtype-list.vue";
import AuditButton from "../components/base/audit-button.vue";

// Give your app a unique name!
var gameOverview = new Vue({
    // The main element that app will attach itself to
    el: '#vueApp',

    components: {
        // Register components that are used
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "input-textarea": InputTextarea,
        "input-calendar": InputCalendar,
        "input-time": InputTime,
        "input-datetime": InputDateTime,
        "vue-selectbase": vSelect,
        "participating-teams": ParticipatingTeams,
        "gamereport-meta": GameReportMeta,
        "eventtype-list": EventTypeList,
        "audit-button": AuditButton,
    },

    data: {
        // Reactive properties
        gameReport: window.gameReport,
        validationMsgs: [],
        isAdmin: false,
        loading: false,
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
        nextScreenName() {
            if (this.gameReport.sport.enablePeriodScoring !== null && this.gameReport.sport.enablePeriodScoring === true) {
                return "Period Scores";
            } else {
                return "Events";
            }
        }
    },

    watch: {
        // Data and Computed properties to watch
    },

    methods: {
        // Do stuff!
        setEventType(e) {
            if (e === undefined || e === null || !e.id) {
                return;
            }

            var oTypes = e.id.split("|");
            this.gameReport.EventTypeId = parseInt(oTypes[0]);
            this.gameReport.RoundId = parseInt(oTypes[1]);
            this.gameReport.SectionId = parseInt(oTypes[2]);
            this.gameReport.GroupId = parseInt(oTypes[3]);
        },

        onGameTeamsUpdated(newval) {
            this.gameReport.participatingTeams = newval;
        },

        onGameMetaUpdated(newval) {
            this.gameReport.meta = newval;
        },

        nextScreen() {
            if (this.gameReport.sport.enablePeriodScoring !== null && this.gameReport.sport.enablePeriodScoring === true) {
                window.location = "/Game/Scoring/" + this.gameReport.gameReportId;
            } else {
                window.location = "/Game/Events/" + this.gameReport.gameReportId;
            }
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
            vm.loading = true;

            var oRequest = JSON.parse(JSON.stringify(this.gameReport));
            // clear out ancillary parts we don't want
            oRequest.sport = null;

            $.ajax({
                method: "POST",
                url: "/SiteApi/Game/" + vm.gameReport.gameReportId,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(oRequest),
                dataType: "json"
            })
                .done(function (data) {
                    vm.loading = false;
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
